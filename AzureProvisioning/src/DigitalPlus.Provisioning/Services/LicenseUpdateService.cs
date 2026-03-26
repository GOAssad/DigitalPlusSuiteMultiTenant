using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DigitalPlus.Provisioning.Services;

/// <summary>
/// Servicio compartido para actualizar licencias desde cualquier pasarela de pago (LSQ, MP, manual).
/// </summary>
public class LicenseUpdateService
{
    private readonly string _connectionString;
    private readonly string _mtConnectionString;
    private readonly ILogger<LicenseUpdateService> _logger;

    public LicenseUpdateService(IConfiguration config, ILogger<LicenseUpdateService> logger)
    {
        _connectionString = config["ProvisioningDb"]
            ?? throw new InvalidOperationException("ProvisioningDb not configured.");
        _logger = logger;

        // Derivar connection string a BD MT (mismo servidor, distinta BD)
        var builder = new SqlConnectionStringBuilder(_connectionString);
        builder.InitialCatalog = config["MultiTenantDb"] ?? "DigitalPlusMultiTenant";
        _mtConnectionString = builder.ConnectionString;
    }

    public string ConnectionString => _connectionString;

    /// <summary>
    /// Actualiza la licencia de una empresa al plan especificado usando PlanConfig.
    /// Reutilizado por LemonSqueezyService y MercadoPagoService.
    /// </summary>
    public async Task ActualizarLicenciaAsync(SqlConnection conn, SqlTransaction tx, int empresaId, string plan)
    {
        // 1. Obtener CompanyId
        await using var cmdCompany = new SqlCommand(
            "SELECT CompanyId FROM Empresas WHERE Id = @Id", conn, tx);
        cmdCompany.Parameters.AddWithValue("@Id", empresaId);
        var companyId = await cmdCompany.ExecuteScalarAsync() as string;

        if (string.IsNullOrEmpty(companyId))
        {
            _logger.LogError("ActualizarLicencia: empresa {EmpresaId} no encontrada", empresaId);
            return;
        }

        // 2. Leer limites de PlanConfig
        await using var cmdConfig = new SqlCommand(
            "SELECT Parametro, Valor FROM PlanConfig WHERE [Plan] = @Plan", conn, tx);
        cmdConfig.Parameters.AddWithValue("@Plan", plan);

        var valores = new Dictionary<string, decimal>();
        await using var reader = await cmdConfig.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            valores[reader.GetString(0)] = reader.GetDecimal(1);
        reader.Close();

        var maxLegajos = (int)valores.GetValueOrDefault("MaxLegajos", 5);
        var maxSucursales = (int)valores.GetValueOrDefault("MaxSucursales", 1);
        var maxFichadasMes = (int)valores.GetValueOrDefault("MaxFichadasRolling30d", 200);
        var duracionDias = (int)valores.GetValueOrDefault("DuracionDias", 0);

        DateTime? expiresAt = duracionDias > 0 ? DateTime.UtcNow.AddDays(duracionDias) : null;

        // 3. Actualizar licencia
        await using var cmdLic = new SqlCommand(
            @"UPDATE Licencias SET
                [Plan] = @Plan, MaxLegajos = @MaxLegajos,
                MaxSucursales = @MaxSucursales, MaxFichadasMes = @MaxFichadasMes,
                ExpiresAt = @ExpiresAt, LicenseType = 'active',
                UpdatedAt = SYSUTCDATETIME()
              WHERE CompanyId = @CompanyId", conn, tx);
        cmdLic.Parameters.AddWithValue("@Plan", plan);
        cmdLic.Parameters.AddWithValue("@MaxLegajos", maxLegajos);
        cmdLic.Parameters.AddWithValue("@MaxSucursales", maxSucursales);
        cmdLic.Parameters.AddWithValue("@MaxFichadasMes", maxFichadasMes);
        cmdLic.Parameters.AddWithValue("@ExpiresAt", (object?)expiresAt ?? DBNull.Value);
        cmdLic.Parameters.AddWithValue("@CompanyId", companyId);
        await cmdLic.ExecuteNonQueryAsync();

        // 4. Registrar en LicenciasLog
        try
        {
            await using var cmdLog = new SqlCommand(
                @"INSERT INTO LicenciasLog (LicenciaId, Action, App, Details, Timestamp)
                  SELECT Id, @Action, @App, @Details, SYSUTCDATETIME()
                  FROM Licencias WHERE CompanyId = @CompanyId", conn, tx);
            cmdLog.Parameters.AddWithValue("@Action", "payment_approved");
            cmdLog.Parameters.AddWithValue("@App", "AzureFunctions");
            cmdLog.Parameters.AddWithValue("@Details",
                $"Plan actualizado a {plan} (maxLeg={maxLegajos}, maxSuc={maxSucursales}, maxFich={maxFichadasMes})");
            cmdLog.Parameters.AddWithValue("@CompanyId", companyId);
            await cmdLog.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se pudo registrar log de licencia para empresa {EmpresaId}", empresaId);
        }

        _logger.LogInformation("Licencia actualizada: empresa {EmpresaId} -> plan {Plan}, maxLeg={MaxLeg}, maxSuc={MaxSuc}",
            empresaId, plan, maxLegajos, maxSucursales);
    }

    /// <summary>
    /// Registra un evento en LicenciasLog buscando la licencia por empresaId.
    /// Best-effort: no lanza excepción si falla.
    /// </summary>
    public async Task LogEventAsync(int empresaId, string action, string app, string? details = null)
    {
        try
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(
                @"INSERT INTO LicenciasLog (LicenciaId, Action, App, Details, Timestamp)
                  SELECT l.Id, @Action, @App, @Details, SYSUTCDATETIME()
                  FROM Licencias l
                  JOIN Empresas e ON l.CompanyId = e.CompanyId
                  WHERE e.Id = @EmpresaId", conn);
            cmd.Parameters.AddWithValue("@Action", action);
            cmd.Parameters.AddWithValue("@App", app);
            cmd.Parameters.AddWithValue("@Details", (object?)details ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EmpresaId", empresaId);
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "LogEvent failed: empresa={EmpresaId}, action={Action}", empresaId, action);
        }
    }

    /// <summary>
    /// Crea una Noticia en la BD Multi-Tenant para notificar al usuario sobre un cambio de plan.
    /// Best-effort: no lanza excepcion si falla. Busca el EmpresaId en la BD MT por CompanyId (Codigo).
    /// </summary>
    public async Task NotificarCambioPlanMTAsync(int adminEmpresaId, string planAnterior, string planNuevo, string origen)
    {
        try
        {
            // Buscar CompanyId en Admin DB
            string? companyId;
            await using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                await using var cmd = new SqlCommand("SELECT CompanyId FROM Empresas WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", adminEmpresaId);
                companyId = await cmd.ExecuteScalarAsync() as string;
            }
            if (string.IsNullOrEmpty(companyId)) return;

            // Buscar EmpresaId en MT DB por Codigo
            await using var mtConn = new SqlConnection(_mtConnectionString);
            await mtConn.OpenAsync();

            int? mtEmpresaId;
            await using (var cmd = new SqlCommand("SELECT Id FROM Empresa WHERE Codigo = @Codigo", mtConn))
            {
                cmd.Parameters.AddWithValue("@Codigo", companyId);
                var result = await cmd.ExecuteScalarAsync();
                mtEmpresaId = result as int?;
            }
            if (!mtEmpresaId.HasValue) return;

            var cap = (string s) => string.IsNullOrEmpty(s) ? s : char.ToUpper(s[0]) + s[1..];
            var titulo = planAnterior == planNuevo
                ? $"Plan {cap(planNuevo)} renovado"
                : $"Plan actualizado a {cap(planNuevo)}";
            var contenido = planAnterior == planNuevo
                ? $"Su plan {cap(planNuevo)} fue renovado exitosamente via {origen}."
                : $"Su plan fue cambiado de {cap(planAnterior)} a {cap(planNuevo)} via {origen}.";

            var ahora = DateTime.UtcNow;
            await using var cmdIns = new SqlCommand(@"
                INSERT INTO Noticia (EmpresaId, Titulo, Contenido, FechaDesde, FechaHasta, IsPrivada, CreatedAt, CreatedBy)
                VALUES (@EmpresaId, @Titulo, @Contenido, @Hoy, @Hasta, 0, @Ahora, 'Sistema')", mtConn);
            cmdIns.Parameters.AddWithValue("@EmpresaId", mtEmpresaId.Value);
            cmdIns.Parameters.AddWithValue("@Titulo", titulo);
            cmdIns.Parameters.AddWithValue("@Contenido", contenido);
            cmdIns.Parameters.AddWithValue("@Hoy", ahora.ToString("yyyy-MM-dd"));
            cmdIns.Parameters.AddWithValue("@Hasta", ahora.AddDays(30).ToString("yyyy-MM-dd"));
            cmdIns.Parameters.AddWithValue("@Ahora", ahora);
            await cmdIns.ExecuteNonQueryAsync();

            _logger.LogInformation("Noticia MT: empresa {EmpresaId}, {Plan} via {Origen}", mtEmpresaId, planNuevo, origen);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se pudo crear noticia MT para empresa admin {EmpresaId}", adminEmpresaId);
        }
    }

    /// <summary>
    /// Registra un evento en LicenciasLog buscando la licencia por companyId (string).
    /// </summary>
    public async Task LogEventByCompanyAsync(string companyId, string action, string app, string? details = null)
    {
        try
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(
                @"INSERT INTO LicenciasLog (LicenciaId, Action, App, Details, Timestamp)
                  SELECT Id, @Action, @App, @Details, SYSUTCDATETIME()
                  FROM Licencias WHERE CompanyId = @CompanyId", conn);
            cmd.Parameters.AddWithValue("@Action", action);
            cmd.Parameters.AddWithValue("@App", app);
            cmd.Parameters.AddWithValue("@Details", (object?)details ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CompanyId", companyId);
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "LogEvent failed: company={CompanyId}, action={Action}", companyId, action);
        }
    }
}
