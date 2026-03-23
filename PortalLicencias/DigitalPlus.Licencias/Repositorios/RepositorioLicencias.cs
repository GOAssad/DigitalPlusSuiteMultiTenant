using System.Security.Cryptography;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using DigitalPlus.Licencias.Data;
using DigitalPlus.Licencias.Entidades;

namespace DigitalPlus.Licencias.Repositorios;

public class RepositorioLicencias
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;

    public RepositorioLicencias(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    // --- Licencias ---

    public async Task<List<Licencia>> GetLicenciasAsync(string? filtro = null)
    {
        var query = _context.Licencias.AsQueryable();
        if (!string.IsNullOrWhiteSpace(filtro))
            query = query.Where(l => l.CompanyId.Contains(filtro) || l.Plan.Contains(filtro));
        return await query.OrderByDescending(l => l.UpdatedAt).ToListAsync();
    }

    public async Task<Licencia?> GetLicenciaAsync(int id)
    {
        return await _context.Licencias.FindAsync(id);
    }

    public async Task<DashboardStats> GetDashboardStatsAsync()
    {
        var now = DateTime.UtcNow;
        var licencias = await _context.Licencias.ToListAsync();

        return new DashboardStats
        {
            TotalLicencias = licencias.Count,
            Activas = licencias.Count(l => l.LicenseType == "active" && (!l.ExpiresAt.HasValue || l.ExpiresAt > now)),
            Trials = licencias.Count(l => l.LicenseType == "trial" && (!l.TrialEndsAt.HasValue || l.TrialEndsAt > now)),
            Vencidas = licencias.Count(l =>
                (l.LicenseType == "trial" && l.TrialEndsAt.HasValue && l.TrialEndsAt < now) ||
                (l.LicenseType == "active" && l.ExpiresAt.HasValue && l.ExpiresAt < now)),
            Suspendidas = licencias.Count(l => l.LicenseType == "suspended"),
            PorVencer7d = licencias.Count(l =>
                l.LicenseType == "active" && l.ExpiresAt.HasValue &&
                l.ExpiresAt > now && l.ExpiresAt < now.AddDays(7))
        };
    }

    public async Task ActualizarLicenciaAsync(int id, string plan, int maxLegajos, DateTime? expiresAt)
    {
        // Si cambió el plan, aplicar límites de PlanConfig automáticamente
        var lic = await _context.Licencias.FindAsync(id);
        if (lic != null && lic.Plan != plan)
        {
            var valores = await GetPlanValoresAsync(plan);
            if (valores.TryGetValue("MaxLegajos", out var ml)) maxLegajos = ml;
            var maxSuc = valores.GetValueOrDefault("MaxSucursales", 0);
            var maxFich = valores.GetValueOrDefault("MaxFichadasRolling30d", 0);

            await using var conn = new SqlConnection(_connectionString);
            await conn.ExecuteAsync(
                @"UPDATE Licencias SET [Plan] = @Plan, MaxLegajos = @MaxLegajos,
                  MaxSucursales = @MaxSucursales, MaxFichadasMes = @MaxFichadasMes,
                  ExpiresAt = @ExpiresAt, UpdatedAt = SYSUTCDATETIME() WHERE Id = @Id",
                new { Id = id, Plan = plan, MaxLegajos = maxLegajos,
                      MaxSucursales = maxSuc, MaxFichadasMes = maxFich, ExpiresAt = expiresAt });
        }
        else
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.ExecuteAsync(
                @"UPDATE Licencias SET [Plan] = @Plan, MaxLegajos = @MaxLegajos,
                  ExpiresAt = @ExpiresAt, UpdatedAt = SYSUTCDATETIME() WHERE Id = @Id",
                new { Id = id, Plan = plan, MaxLegajos = maxLegajos, ExpiresAt = expiresAt });
        }
    }

    public async Task ExtenderLicenciaAsync(int id, int dias)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.ExecuteAsync(
            @"UPDATE Licencias SET ExpiresAt = DATEADD(DAY, @Dias,
              CASE WHEN ExpiresAt IS NULL OR ExpiresAt < SYSUTCDATETIME()
                   THEN SYSUTCDATETIME() ELSE ExpiresAt END),
              LicenseType = 'active', UpdatedAt = SYSUTCDATETIME() WHERE Id = @Id",
            new { Id = id, Dias = dias });
    }

    public async Task SuspenderLicenciaAsync(int id, bool suspender)
    {
        await using var conn = new SqlConnection(_connectionString);
        if (suspender)
        {
            await conn.ExecuteAsync(
                @"UPDATE Licencias SET LicenseType = 'suspended', SuspendedAt = SYSUTCDATETIME(),
                  GraceEndsAt = DATEADD(DAY, 7, SYSUTCDATETIME()), UpdatedAt = SYSUTCDATETIME()
                  WHERE Id = @Id", new { Id = id });
        }
        else
        {
            await conn.ExecuteAsync(
                @"UPDATE Licencias SET LicenseType = 'active', SuspendedAt = NULL,
                  GraceEndsAt = NULL, UpdatedAt = SYSUTCDATETIME()
                  WHERE Id = @Id", new { Id = id });
        }
    }

    // --- Codigos ---

    public async Task<List<LicenseCode>> GetCodigosAsync(bool incluirUsados = false)
    {
        var query = _context.LicenseCodes.AsQueryable();
        if (!incluirUsados)
            query = query.Where(c => c.UsedAt == null && c.ExpiresAt >= DateTime.UtcNow);
        return await query.OrderByDescending(c => c.CreatedAt).ToListAsync();
    }

    public async Task<string> GenerarCodigoAsync(string plan, int maxLegajos, int durationDays,
        int codeExpiryHours, string createdBy, string? notes)
    {
        var bytes = RandomNumberGenerator.GetBytes(16);
        var hex = Convert.ToHexString(bytes);
        var code = $"{hex[..4]}-{hex[4..8]}-{hex[8..12]}-{hex[12..16]}";

        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(code));
        var hashHex = Convert.ToHexString(hashBytes).ToLower();

        var entity = new LicenseCode
        {
            CodeHash = hashHex,
            Plan = plan,
            MaxLegajos = maxLegajos,
            DurationDays = durationDays,
            ExpiresAt = DateTime.UtcNow.AddHours(codeExpiryHours),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
            Notes = notes
        };

        _context.LicenseCodes.Add(entity);
        await _context.SaveChangesAsync();

        return code;
    }

    // --- Empresas ---

    public async Task<List<Empresa>> GetEmpresasAsync(string? filtro = null)
    {
        var query = _context.Empresas.Include(e => e.Pais).AsQueryable();
        if (!string.IsNullOrWhiteSpace(filtro))
            query = query.Where(e => e.Nombre.Contains(filtro) || e.CompanyId.Contains(filtro));
        return await query.OrderByDescending(e => e.CreatedAt).ToListAsync();
    }

    public async Task<Empresa?> GetEmpresaAsync(int id)
    {
        return await _context.Empresas
            .Include(e => e.Pais)
            .Include(e => e.TipoIdentificacionFiscal)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task ActualizarEmpresaAsync(Empresa empresa)
    {
        empresa.UpdatedAt = DateTime.UtcNow;
        _context.Empresas.Update(empresa);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Pais>> GetPaisesAsync()
    {
        return await _context.Paises.OrderBy(p => p.Nombre).ToListAsync();
    }

    public async Task<List<TipoIdentificacionFiscal>> GetTiposIdentificacionAsync(int? paisId = null)
    {
        var query = _context.TiposIdentificacionFiscal.AsQueryable();
        if (paisId.HasValue)
            query = query.Where(t => t.PaisId == paisId.Value);
        return await query.OrderBy(t => t.Nombre).ToListAsync();
    }

    public async Task<List<TipoIdentificacionFiscal>> GetTiposIdentificacionConPaisAsync()
    {
        return await _context.TiposIdentificacionFiscal
            .Include(t => t.Pais)
            .OrderBy(t => t.Pais!.Nombre).ThenBy(t => t.Nombre)
            .ToListAsync();
    }

    // --- ABM Paises ---

    public async Task GuardarPaisAsync(int id, string nombre, string codigoISO)
    {
        if (id == 0)
        {
            _context.Paises.Add(new Pais { Nombre = nombre, CodigoISO = codigoISO });
        }
        else
        {
            var pais = await _context.Paises.FindAsync(id)
                ?? throw new InvalidOperationException("Pais no encontrado");
            pais.Nombre = nombre;
            pais.CodigoISO = codigoISO;
        }
        await _context.SaveChangesAsync();
    }

    public async Task EliminarPaisAsync(int id)
    {
        var tieneEmpresas = await _context.Empresas.AnyAsync(e => e.PaisId == id);
        if (tieneEmpresas)
            throw new InvalidOperationException("No se puede eliminar: hay empresas asociadas a este pais.");

        var tieneTipos = await _context.TiposIdentificacionFiscal.AnyAsync(t => t.PaisId == id);
        if (tieneTipos)
            throw new InvalidOperationException("No se puede eliminar: hay tipos de identificacion fiscal asociados a este pais.");

        var pais = await _context.Paises.FindAsync(id);
        if (pais != null)
        {
            _context.Paises.Remove(pais);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Dictionary<int, int>> ContarTiposPorPaisAsync()
    {
        return await _context.TiposIdentificacionFiscal
            .GroupBy(t => t.PaisId)
            .Select(g => new { PaisId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.PaisId, x => x.Count);
    }

    public async Task<Dictionary<int, int>> ContarEmpresasPorPaisAsync()
    {
        return await _context.Empresas
            .Where(e => e.PaisId.HasValue)
            .GroupBy(e => e.PaisId!.Value)
            .Select(g => new { PaisId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.PaisId, x => x.Count);
    }

    // --- ABM Tipos Identificacion Fiscal ---

    public async Task GuardarTipoIdentificacionAsync(int id, int paisId, string nombre, string? formato, string? ejemplo)
    {
        if (id == 0)
        {
            _context.TiposIdentificacionFiscal.Add(new TipoIdentificacionFiscal
            {
                PaisId = paisId, Nombre = nombre, Formato = formato, Ejemplo = ejemplo
            });
        }
        else
        {
            var tipo = await _context.TiposIdentificacionFiscal.FindAsync(id)
                ?? throw new InvalidOperationException("Tipo no encontrado");
            tipo.PaisId = paisId;
            tipo.Nombre = nombre;
            tipo.Formato = formato;
            tipo.Ejemplo = ejemplo;
        }
        await _context.SaveChangesAsync();
    }

    public async Task EliminarTipoIdentificacionAsync(int id)
    {
        var tieneEmpresas = await _context.Empresas.AnyAsync(e => e.TipoIdentificacionFiscalId == id);
        if (tieneEmpresas)
            throw new InvalidOperationException("No se puede eliminar: hay empresas usando este tipo de identificacion.");

        var tipo = await _context.TiposIdentificacionFiscal.FindAsync(id);
        if (tipo != null)
        {
            _context.TiposIdentificacionFiscal.Remove(tipo);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Dictionary<int, int>> ContarEmpresasPorTipoIdFiscalAsync()
    {
        return await _context.Empresas
            .Where(e => e.TipoIdentificacionFiscalId.HasValue)
            .GroupBy(e => e.TipoIdentificacionFiscalId!.Value)
            .Select(g => new { TipoId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.TipoId, x => x.Count);
    }

    public async Task<Empresa> CrearEmpresaAsync(string nombre, string companyId, string databaseName,
        string? contacto, string? email, string? telefono)
    {
        var empresa = new Empresa
        {
            Nombre = nombre,
            CompanyId = companyId,
            DatabaseName = databaseName,
            Contacto = contacto,
            Email = email,
            Telefono = telefono,
            Estado = "activa",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _context.Empresas.Add(empresa);
        await _context.SaveChangesAsync();
        return empresa;
    }

    public async Task<Licencia> CrearLicenciaParaEmpresaAsync(string companyId, string plan, int maxLegajos, int durationDays)
    {
        var now = DateTime.UtcNow;
        await using var conn = new SqlConnection(_connectionString);
        await conn.ExecuteAsync(
            @"INSERT INTO Licencias (CompanyId, MachineId, LicenseType, [Plan], MaxLegajos, ExpiresAt, CreatedAt, UpdatedAt)
              VALUES (@CompanyId, 'pending', 'active', @Plan, @MaxLegajos, @ExpiresAt, SYSUTCDATETIME(), SYSUTCDATETIME())",
            new { CompanyId = companyId, Plan = plan, MaxLegajos = maxLegajos,
                  ExpiresAt = now.AddDays(durationDays) });

        return (await _context.Licencias.FirstAsync(l => l.CompanyId == companyId && l.MachineId == "pending"));
    }

    public async Task<Licencia> CrearLicenciaParaEmpresaConLimitesAsync(
        string companyId, string plan, int maxLegajos, int maxSucursales, int maxFichadasMes, int durationDays)
    {
        var now = DateTime.UtcNow;
        DateTime? expiresAt = durationDays > 0 ? now.AddDays(durationDays) : null;

        await using var conn = new SqlConnection(_connectionString);
        await conn.ExecuteAsync(
            @"INSERT INTO Licencias (CompanyId, MachineId, LicenseType, [Plan], MaxLegajos, MaxSucursales, MaxFichadasMes, ExpiresAt, CreatedAt, UpdatedAt)
              VALUES (@CompanyId, 'pending', 'active', @Plan, @MaxLegajos, @MaxSucursales, @MaxFichadasMes, @ExpiresAt, SYSUTCDATETIME(), SYSUTCDATETIME())",
            new { CompanyId = companyId, Plan = plan, MaxLegajos = maxLegajos,
                  MaxSucursales = maxSucursales, MaxFichadasMes = maxFichadasMes, ExpiresAt = expiresAt });

        return (await _context.Licencias.FirstAsync(l => l.CompanyId == companyId && l.MachineId == "pending"));
    }

    // --- Activacion por codigo (para instalador liviano) ---

    public async Task<Empresa?> GetEmpresaPorCompanyIdAsync(string companyId)
    {
        return await _context.Empresas
            .FirstOrDefaultAsync(e => e.CompanyId == companyId);
    }

    public async Task<Empresa?> BuscarEmpresaPorCodigoActivacionAsync(string codigo)
    {
        return await _context.Empresas
            .FirstOrDefaultAsync(e => e.CodigoActivacion == codigo && e.Estado == "activa");
    }

    public string GenerarCodigoActivacion()
    {
        var bytes = System.Security.Cryptography.RandomNumberGenerator.GetBytes(12);
        var hex = Convert.ToHexString(bytes);
        return $"{hex[..4]}-{hex[4..8]}-{hex[8..12]}-{hex[12..16]}-{hex[16..20]}-{hex[20..24]}";
    }

    public async Task RegenerarCodigoActivacionAsync(int empresaId)
    {
        var empresa = await _context.Empresas.FindAsync(empresaId)
            ?? throw new InvalidOperationException("Empresa no encontrada");
        empresa.CodigoActivacion = GenerarCodigoActivacion();
        empresa.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    // --- Plan Config ---

    public async Task<List<PlanConfig>> GetPlanConfigsAsync()
    {
        return await _context.PlanConfigs.OrderBy(p => p.Plan).ThenBy(p => p.Parametro).ToListAsync();
    }

    public async Task<List<string>> GetPlanesAsync()
    {
        return await _context.PlanConfigs.Select(p => p.Plan).Distinct().OrderBy(p => p).ToListAsync();
    }

    public async Task<Dictionary<string, int>> GetPlanValoresAsync(string plan)
    {
        return await _context.PlanConfigs
            .Where(p => p.Plan == plan)
            .ToDictionaryAsync(p => p.Parametro, p => p.Valor);
    }

    private static readonly string[] ParametrosSistema = ["OrdenPlan", "DuracionDias", "GraciaDias"];

    public async Task GuardarPlanConfigAsync(int id, int valor,
        string? categoria = null, string? tipoVisualizacion = null,
        string? labelAmigable = null, string? icono = null,
        int? ordenVisualizacion = null, bool? visibleEnComparacion = null)
    {
        var config = await _context.PlanConfigs.FindAsync(id);
        if (config != null)
        {
            config.Valor = valor;
            if (categoria != null) config.Categoria = categoria;
            if (tipoVisualizacion != null) config.TipoVisualizacion = tipoVisualizacion;
            if (labelAmigable != null) config.LabelAmigable = labelAmigable;
            if (icono != null) config.Icono = icono;
            if (ordenVisualizacion.HasValue) config.OrdenVisualizacion = ordenVisualizacion.Value;
            if (visibleEnComparacion.HasValue) config.VisibleEnComparacion = visibleEnComparacion.Value;
            await _context.SaveChangesAsync();
        }
    }

    public async Task AgregarPlanConfigAsync(string plan, string parametro, int valor, string? descripcion,
        string? categoria = null, string? tipoVisualizacion = null,
        string? labelAmigable = null, string? icono = null,
        int ordenVisualizacion = 50, bool visibleEnComparacion = true)
    {
        _context.PlanConfigs.Add(new PlanConfig
        {
            Plan = plan,
            Parametro = parametro,
            Valor = valor,
            Descripcion = descripcion,
            Categoria = categoria,
            TipoVisualizacion = tipoVisualizacion,
            LabelAmigable = labelAmigable,
            Icono = icono,
            OrdenVisualizacion = ordenVisualizacion,
            VisibleEnComparacion = visibleEnComparacion
        });
        await _context.SaveChangesAsync();
    }

    public async Task EliminarPlanConfigAsync(int id)
    {
        var config = await _context.PlanConfigs.FindAsync(id);
        if (config != null)
        {
            _context.PlanConfigs.Remove(config);
            await _context.SaveChangesAsync();
        }
    }

    public async Task EliminarParametroAsync(int id)
    {
        var config = await _context.PlanConfigs.FindAsync(id)
            ?? throw new InvalidOperationException("Parametro no encontrado.");
        if (ParametrosSistema.Contains(config.Parametro))
            throw new InvalidOperationException($"No se puede eliminar el parametro de sistema '{config.Parametro}'.");
        _context.PlanConfigs.Remove(config);
        await _context.SaveChangesAsync();
    }

    // --- Planes comparacion (API publica) ---

    public async Task<List<object>> GetPlanesComparacionAsync()
    {
        await using var conn = new SqlConnection(_connectionString);
        var configs = await conn.QueryAsync<dynamic>(
            @"SELECT [Plan], Parametro, Valor, Descripcion,
                     ISNULL(Categoria, 'Limite') AS Categoria,
                     ISNULL(TipoVisualizacion, 'cantidad') AS TipoVisualizacion,
                     ISNULL(LabelAmigable, Parametro) AS LabelAmigable,
                     Icono,
                     ISNULL(OrdenVisualizacion, 50) AS OrdenVisualizacion,
                     ISNULL(VisibleEnComparacion, 1) AS VisibleEnComparacion
              FROM PlanConfig
              ORDER BY [Plan], OrdenVisualizacion");

        var grouped = configs.GroupBy(c => (string)c.Plan);

        var result = grouped.Select(g =>
        {
            var parametros = g.Select(item => new
            {
                parametro = (string)item.Parametro,
                valor = (int)item.Valor,
                categoria = (string)item.Categoria,
                tipoVisualizacion = (string)item.TipoVisualizacion,
                labelAmigable = (string)item.LabelAmigable,
                icono = item.Icono as string,
                ordenVisualizacion = (int)item.OrdenVisualizacion,
                visibleEnComparacion = (bool)item.VisibleEnComparacion
            }).ToList();

            var ordenParam = parametros.FirstOrDefault(p => p.parametro == "OrdenPlan");
            var orden = ordenParam?.valor ?? 99;

            return new
            {
                plan = g.Key,
                orden,
                parametros
            } as object;
        })
        .OrderBy(x => ((dynamic)x).orden)
        .ToList();

        return result;
    }

    // --- Solicitud Upgrade ---

    public async Task<(int id, string token)> CrearSolicitudUpgradeAsync(int empresaId, string companyId, string planActual, string planSolicitado, string solicitadoPor, decimal? importeMensual)
    {
        await using var conn = new SqlConnection(_connectionString);
        var token = Guid.NewGuid().ToString("N");
        var id = await conn.QuerySingleAsync<int>(
            @"INSERT INTO SolicitudUpgrade (EmpresaId, CompanyId, PlanActual, PlanSolicitado, Estado, SolicitadoPor, FechaSolicitud, ImporteMensual, Token, TokenExpiresAt, CreatedAt, UpdatedAt)
              VALUES (@EmpresaId, @CompanyId, @PlanActual, @PlanSolicitado, 'PendientePago', @SolicitadoPor, SYSUTCDATETIME(), @ImporteMensual, @Token, DATEADD(DAY, 7, SYSUTCDATETIME()), SYSUTCDATETIME(), SYSUTCDATETIME());
              SELECT CAST(SCOPE_IDENTITY() AS INT);",
            new { EmpresaId = empresaId, CompanyId = companyId, PlanActual = planActual,
                  PlanSolicitado = planSolicitado, SolicitadoPor = solicitadoPor,
                  ImporteMensual = importeMensual, Token = token });
        return (id, token);
    }

    // --- Solicitud Upgrade: consulta y aplicar ---

    public async Task<dynamic?> GetSolicitudUpgradeByTokenAsync(string token)
    {
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryFirstOrDefaultAsync<dynamic>(
            @"SELECT s.*, e.Nombre AS EmpresaNombre
              FROM SolicitudUpgrade s
              LEFT JOIN Empresas e ON e.Id = s.EmpresaId
              WHERE s.Token = @Token",
            new { Token = token });
    }

    public async Task<bool> AplicarUpgradeAsync(string token)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        using var tx = conn.BeginTransaction();

        try
        {
            // 1. Obtener solicitud
            var solicitud = await conn.QueryFirstOrDefaultAsync<dynamic>(
                "SELECT * FROM SolicitudUpgrade WHERE Token = @Token AND Estado = 'PendientePago'",
                new { Token = token }, tx);

            if (solicitud == null) return false;

            string companyId = solicitud.CompanyId;
            string planSolicitado = solicitud.PlanSolicitado;

            // 2. Obtener valores del plan nuevo desde PlanConfig
            var configs = await conn.QueryAsync<dynamic>(
                "SELECT Parametro, Valor FROM PlanConfig WHERE [Plan] = @Plan",
                new { Plan = planSolicitado }, tx);
            var valores = configs.ToDictionary(c => (string)c.Parametro, c => (int)c.Valor);

            int maxLegajos = valores.GetValueOrDefault("MaxLegajos", 5);
            int maxSucursales = valores.GetValueOrDefault("MaxSucursales", 1);
            int maxFichadasMes = valores.GetValueOrDefault("MaxFichadasRolling30d", 200);
            int duracionDias = valores.GetValueOrDefault("DuracionDias", 0);

            // 3. Calcular ExpiresAt
            DateTime? expiresAt = duracionDias > 0
                ? DateTime.UtcNow.AddDays(duracionDias)
                : null; // 0 = sin vencimiento

            // 4. Actualizar licencia
            await conn.ExecuteAsync(
                @"UPDATE Licencias SET [Plan] = @Plan, MaxLegajos = @MaxLegajos,
                  MaxSucursales = @MaxSucursales, MaxFichadasMes = @MaxFichadasMes,
                  ExpiresAt = @ExpiresAt, LicenseType = 'active',
                  UpdatedAt = SYSUTCDATETIME()
                  WHERE CompanyId = @CompanyId",
                new { Plan = planSolicitado, MaxLegajos = maxLegajos,
                      MaxSucursales = maxSucursales, MaxFichadasMes = maxFichadasMes,
                      ExpiresAt = expiresAt, CompanyId = companyId }, tx);

            // 5. Marcar solicitud como aprobada
            await conn.ExecuteAsync(
                @"UPDATE SolicitudUpgrade SET Estado = 'Aprobada', UpdatedAt = SYSUTCDATETIME()
                  WHERE Token = @Token",
                new { Token = token }, tx);

            tx.Commit();
            return true;
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    public async Task<string?> GetEmailSolicitanteAsync(string token)
    {
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryFirstOrDefaultAsync<string>(
            "SELECT SolicitadoPor FROM SolicitudUpgrade WHERE Token = @Token",
            new { Token = token });
    }

    public async Task<List<dynamic>> GetSolicitudesUpgradeAsync()
    {
        await using var conn = new SqlConnection(_connectionString);
        return (await conn.QueryAsync<dynamic>(
            @"SELECT s.*, e.Nombre AS EmpresaNombre
              FROM SolicitudUpgrade s
              LEFT JOIN Empresas e ON e.Id = s.EmpresaId
              ORDER BY s.FechaSolicitud DESC")).ToList();
    }

    // --- Log ---

    public async Task<List<LicenciaLog>> GetLogsAsync(int? licenciaId = null, int cantidad = 100)
    {
        var query = _context.LicenciasLog.Include(l => l.Licencia).AsQueryable();
        if (licenciaId.HasValue)
            query = query.Where(l => l.LicenciaId == licenciaId.Value);
        return await query.OrderByDescending(l => l.Timestamp).Take(cantidad).ToListAsync();
    }
}

public class DashboardStats
{
    public int TotalLicencias { get; set; }
    public int Activas { get; set; }
    public int Trials { get; set; }
    public int Vencidas { get; set; }
    public int Suspendidas { get; set; }
    public int PorVencer7d { get; set; }
}
