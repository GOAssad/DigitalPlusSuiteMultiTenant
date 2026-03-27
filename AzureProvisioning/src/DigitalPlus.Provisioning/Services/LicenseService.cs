using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DigitalPlus.Provisioning.Models;

namespace DigitalPlus.Provisioning.Services;

public class LicenseService
{
    private readonly string _connectionString;
    private readonly X509Certificate2 _signingCert;
    private readonly ILogger<LicenseService> _logger;

    private const int OfflineGraceHours = 72;

    public LicenseService(IConfiguration config, ILogger<LicenseService> logger)
    {
        _connectionString = config["ProvisioningDb"]
            ?? throw new InvalidOperationException("ProvisioningDb not configured.");

        var pfxBase64 = config["LicenseSigningKey"]
            ?? throw new InvalidOperationException("LicenseSigningKey not configured.");
        var pfxPassword = config["LicenseSigningKeyPassword"] ?? "temp1234!";

        var pfxBytes = Convert.FromBase64String(pfxBase64);
        _signingCert = new X509Certificate2(pfxBytes, pfxPassword,
            X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.EphemeralKeySet);

        _logger = logger;
    }

    /// <summary>
    /// Activa una licencia (trial si no hay code, full si hay code valido).
    /// </summary>
    public async Task<(LicenseResponse? response, string? error, int statusCode)> ActivateAsync(
        LicenseActivateRequest request)
    {
        var now = DateTime.UtcNow;
        var isTrial = string.IsNullOrWhiteSpace(request.ActivationCode);

        // Si tiene activation code, computar hash
        string? codeHash = null;
        if (!isTrial)
        {
            codeHash = ActivationCodeService.ComputeSha256(request.ActivationCode!);
        }

        // Sanitizar company name para CompanyId
        var companyId = SanitizeCompanyId(request.CompanyName);

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        // ── Buscar licencia existente con MachineId="pending" (creada desde Portal Licencias/pagos) ──
        // El Portal usa CompanyId directo (ej: "new-family"), el Fichador sanitiza a "DP_New_Family".
        // Intentamos vincular la licencia existente al MachineId real.
        var pendingLicId = await TryClaimPendingLicenseAsync(conn, request.CompanyName, companyId, request.MachineId);
        if (pendingLicId.HasValue)
        {
            _logger.LogInformation(
                "License activate: claimed pending license {LicId} for company={Company}, machine={Machine}",
                pendingLicId.Value, request.CompanyName, request.MachineId);

            var pendingTicket = await BuildTicketFromDbAsync(conn, pendingLicId.Value, now);
            if (pendingTicket != null)
            {
                var pendingResp = SignTicket(pendingTicket);
                return (pendingResp, null, 200);
            }
        }

        // ── Flujo original: crear o recuperar licencia via SP ──
        string plan;
        int maxLegajos;
        DateTime? expiresAt;

        if (isTrial)
        {
            plan = "free";
            maxLegajos = 5;
            expiresAt = null;  // trial usa TrialEndsAt
        }
        else
        {
            // Validar el codigo contra la tabla LicenseCodes
            var (codeResult, codePlan, codeMaxLegajos, codeDurationDays) =
                await ValidateLicenseCodeAsync(conn, codeHash!, companyId, request.MachineId);

            if (codeResult == 1)
                return (null, "Codigo de activacion no encontrado.", 404);
            if (codeResult == 2)
                return (null, "El codigo de activacion ha expirado.", 410);
            if (codeResult == 3)
                return (null, "El codigo de activacion ya fue utilizado.", 409);

            plan = codePlan;
            maxLegajos = codeMaxLegajos;
            expiresAt = now.AddDays(codeDurationDays);
        }

        int result, licenciaId;

        await using var cmd = new SqlCommand("License_Activate", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CompanyId", companyId);
        cmd.Parameters.AddWithValue("@MachineId", request.MachineId);
        cmd.Parameters.AddWithValue("@ActivationCode", (object?)codeHash ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Plan", plan);
        cmd.Parameters.AddWithValue("@MaxLegajos", maxLegajos);
        cmd.Parameters.AddWithValue("@ExpiresAt", (object?)expiresAt ?? DBNull.Value);

        var pResult = cmd.Parameters.Add("@Result", System.Data.SqlDbType.Int);
        pResult.Direction = System.Data.ParameterDirection.Output;
        var pLicId = cmd.Parameters.Add("@LicenciaId", System.Data.SqlDbType.Int);
        pLicId.Direction = System.Data.ParameterDirection.Output;

        await cmd.ExecuteNonQueryAsync();

        result = (int)pResult.Value;
        licenciaId = (int)pLicId.Value;

        _logger.LogInformation(
            "License activate: company={CompanyId}, machine={MachineId}, trial={IsTrial}, result={Result}, id={Id}",
            companyId, request.MachineId, isTrial, result, licenciaId);

        // Ahora leer la licencia completa para armar el ticket
        var ticket = await BuildTicketFromDbAsync(conn, licenciaId, now);
        if (ticket == null)
            return (null, "Error al leer la licencia creada.", 500);

        var response = SignTicket(ticket);
        return (response, null, 200);
    }

    /// <summary>
    /// Heartbeat: renueva el ticket con el estado actual de la licencia.
    /// </summary>
    public async Task<(LicenseResponse? response, string? error, int statusCode)> HeartbeatAsync(
        LicenseHeartbeatRequest request)
    {
        var now = DateTime.UtcNow;

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand("License_Heartbeat", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CompanyId", request.CompanyId);
        cmd.Parameters.AddWithValue("@MachineId", request.MachineId);
        cmd.Parameters.AddWithValue("@App", string.IsNullOrEmpty(request.App) ? DBNull.Value : request.App);
        cmd.Parameters.AddWithValue("@ActiveLegajos", request.ActiveLegajos);

        var pResult = cmd.Parameters.Add("@Result", System.Data.SqlDbType.Int);
        pResult.Direction = System.Data.ParameterDirection.Output;
        var pLicId = cmd.Parameters.Add("@LicenciaId", System.Data.SqlDbType.Int);
        pLicId.Direction = System.Data.ParameterDirection.Output;
        var pType = cmd.Parameters.Add("@LicenseType", System.Data.SqlDbType.NVarChar, 20);
        pType.Direction = System.Data.ParameterDirection.Output;
        var pPlan = cmd.Parameters.Add("@LicensePlan", System.Data.SqlDbType.NVarChar, 50);
        pPlan.Direction = System.Data.ParameterDirection.Output;
        var pMaxLeg = cmd.Parameters.Add("@MaxLegajos", System.Data.SqlDbType.Int);
        pMaxLeg.Direction = System.Data.ParameterDirection.Output;
        var pTrialEnds = cmd.Parameters.Add("@TrialEndsAt", System.Data.SqlDbType.DateTime2);
        pTrialEnds.Direction = System.Data.ParameterDirection.Output;
        var pExpires = cmd.Parameters.Add("@ExpiresAt", System.Data.SqlDbType.DateTime2);
        pExpires.Direction = System.Data.ParameterDirection.Output;
        var pSuspended = cmd.Parameters.Add("@SuspendedAt", System.Data.SqlDbType.DateTime2);
        pSuspended.Direction = System.Data.ParameterDirection.Output;
        var pGraceEnds = cmd.Parameters.Add("@GraceEndsAt", System.Data.SqlDbType.DateTime2);
        pGraceEnds.Direction = System.Data.ParameterDirection.Output;

        await cmd.ExecuteNonQueryAsync();

        var result = (int)pResult.Value;
        if (result == 1)
        {
            _logger.LogWarning("Heartbeat: license not found for {CompanyId}/{MachineId}",
                request.CompanyId, request.MachineId);
            return (null, "Licencia no encontrada.", 404);
        }

        var ticket = new LicenseTicket
        {
            Version = 1,
            CompanyId = request.CompanyId,
            MachineId = request.MachineId,
            LicenseType = pType.Value as string ?? "trial",
            Plan = pPlan.Value as string ?? "free",
            MaxLegajos = pMaxLeg.Value is int ml ? ml : 5,
            TrialEndsAt = pTrialEnds.Value is DateTime te ? te : null,
            ExpiresAt = pExpires.Value is DateTime ex ? ex : null,
            SuspendedAt = pSuspended.Value is DateTime su ? su : null,
            GraceEndsAt = pGraceEnds.Value is DateTime ge ? ge : null,
            IssuedAt = now,
            NextCheckRequiredAt = now.AddHours(OfflineGraceHours),
            ServerTimeUtc = now,
        };

        _logger.LogInformation(
            "Heartbeat: company={CompanyId}, type={Type}, legajos={Legajos}",
            request.CompanyId, ticket.LicenseType, request.ActiveLegajos);

        var response = SignTicket(ticket);
        return (response, null, 200);
    }

    // -------------------------------------------------------
    // Helpers
    // -------------------------------------------------------

    private async Task<LicenseTicket?> BuildTicketFromDbAsync(SqlConnection conn, int licenciaId, DateTime now)
    {
        await using var cmd = new SqlCommand(
            @"SELECT CompanyId, MachineId, LicenseType, [Plan], MaxLegajos,
                     TrialEndsAt, ExpiresAt, SuspendedAt, GraceEndsAt
              FROM Licencias WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", licenciaId);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return new LicenseTicket
        {
            Version = 1,
            CompanyId = reader.GetString(0),
            MachineId = reader.GetString(1),
            LicenseType = reader.GetString(2),
            Plan = reader.GetString(3),
            MaxLegajos = reader.GetInt32(4),
            TrialEndsAt = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
            ExpiresAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
            SuspendedAt = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
            GraceEndsAt = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
            IssuedAt = now,
            NextCheckRequiredAt = now.AddHours(OfflineGraceHours),
            ServerTimeUtc = now,
        };
    }

    private LicenseResponse SignTicket(LicenseTicket ticket)
    {
        var ticketJson = JsonSerializer.Serialize(ticket, new JsonSerializerOptions
        {
            WriteIndented = false,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never,
        });

        using var rsa = _signingCert.GetRSAPrivateKey()!;
        var data = Encoding.UTF8.GetBytes(ticketJson);
        var sig = rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        return new LicenseResponse
        {
            Ticket = ticketJson,
            Signature = Convert.ToBase64String(sig)
        };
    }

    private async Task<(int result, string plan, int maxLegajos, int durationDays)>
        ValidateLicenseCodeAsync(SqlConnection conn, string codeHash, string companyId, string machineId)
    {
        await using var cmd = new SqlCommand("License_ValidateAndConsumeCode", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CodeHash", codeHash);
        cmd.Parameters.AddWithValue("@CompanyId", companyId);
        cmd.Parameters.AddWithValue("@MachineId", machineId);

        var pResult = cmd.Parameters.Add("@Result", System.Data.SqlDbType.Int);
        pResult.Direction = System.Data.ParameterDirection.Output;
        var pPlan = cmd.Parameters.Add("@Plan", System.Data.SqlDbType.NVarChar, 50);
        pPlan.Direction = System.Data.ParameterDirection.Output;
        var pMaxLeg = cmd.Parameters.Add("@MaxLegajos", System.Data.SqlDbType.Int);
        pMaxLeg.Direction = System.Data.ParameterDirection.Output;
        var pDuration = cmd.Parameters.Add("@DurationDays", System.Data.SqlDbType.Int);
        pDuration.Direction = System.Data.ParameterDirection.Output;

        await cmd.ExecuteNonQueryAsync();

        var result = (int)pResult.Value;
        var plan = pPlan.Value as string ?? "basic";
        var maxLegajos = pMaxLeg.Value is int ml ? ml : 25;
        var durationDays = pDuration.Value is int dd ? dd : 365;

        return (result, plan, maxLegajos, durationDays);
    }

    /// <summary>
    /// Busca una licencia con MachineId="pending" que pertenezca a esta empresa.
    /// Si la encuentra, actualiza el MachineId al real y retorna el Id.
    /// Busca por: CompanyId exacto en Empresas que matchee con Licencias.CompanyId.
    /// </summary>
    private async Task<int?> TryClaimPendingLicenseAsync(
        SqlConnection conn, string companyName, string sanitizedCompanyId, string machineId)
    {
        try
        {
            // Buscar en Empresas por nombre (case-insensitive) para obtener el CompanyId real
            await using var cmdFind = new SqlCommand(
                @"SELECT TOP 1 l.Id
                  FROM Licencias l
                  INNER JOIN Empresas e ON l.CompanyId = e.CompanyId
                  WHERE (e.Nombre LIKE @CompanyName OR e.CompanyId = @SanitizedId)
                    AND l.MachineId = 'pending'
                    AND l.LicenseType = 'active'", conn);
            cmdFind.Parameters.AddWithValue("@CompanyName", companyName);
            cmdFind.Parameters.AddWithValue("@SanitizedId", sanitizedCompanyId);
            var result = await cmdFind.ExecuteScalarAsync();

            if (result is not int licId)
                return null;

            // Vincular: actualizar MachineId de "pending" al real + registrar heartbeat
            await using var cmdClaim = new SqlCommand(
                @"UPDATE Licencias SET MachineId = @MachineId, LastHeartbeat = SYSUTCDATETIME(), UpdatedAt = SYSUTCDATETIME()
                  WHERE Id = @Id AND MachineId = 'pending'", conn);
            cmdClaim.Parameters.AddWithValue("@MachineId", machineId);
            cmdClaim.Parameters.AddWithValue("@Id", licId);
            var rows = await cmdClaim.ExecuteNonQueryAsync();

            if (rows > 0)
            {
                // Log
                await using var cmdLog = new SqlCommand(
                    @"INSERT INTO LicenciasLog (LicenciaId, Action, App, Details, Timestamp)
                      VALUES (@Id, 'claim_pending', 'Fichador', @Details, SYSUTCDATETIME())", conn);
                cmdLog.Parameters.AddWithValue("@Id", licId);
                cmdLog.Parameters.AddWithValue("@Details", $"MachineId vinculado: {machineId}");
                await cmdLog.ExecuteNonQueryAsync();

                return licId;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "TryClaimPendingLicense failed for {CompanyName}", companyName);
        }
        return null;
    }

    private static string SanitizeCompanyId(string companyName)
    {
        if (string.IsNullOrWhiteSpace(companyName)) return "DP_Unknown";

        var sb = new StringBuilder("DP_");
        foreach (var c in companyName.Trim())
        {
            if (char.IsLetterOrDigit(c))
                sb.Append(c);
            else if (c == ' ')
                sb.Append('_');
        }

        var result = sb.ToString();
        if (result == "DP_") result = "DP_Unknown";
        return result.Length > 100 ? result[..100] : result;
    }
}
