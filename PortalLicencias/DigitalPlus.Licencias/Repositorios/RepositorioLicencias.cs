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
        await using var conn = new SqlConnection(_connectionString);
        await conn.ExecuteAsync(
            @"UPDATE Licencias SET [Plan] = @Plan, MaxLegajos = @MaxLegajos,
              ExpiresAt = @ExpiresAt, UpdatedAt = SYSUTCDATETIME() WHERE Id = @Id",
            new { Id = id, Plan = plan, MaxLegajos = maxLegajos, ExpiresAt = expiresAt });
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

    public async Task GuardarPlanConfigAsync(int id, int valor)
    {
        var config = await _context.PlanConfigs.FindAsync(id);
        if (config != null)
        {
            config.Valor = valor;
            await _context.SaveChangesAsync();
        }
    }

    public async Task AgregarPlanConfigAsync(string plan, string parametro, int valor, string? descripcion)
    {
        _context.PlanConfigs.Add(new PlanConfig
        {
            Plan = plan,
            Parametro = parametro,
            Valor = valor,
            Descripcion = descripcion
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
