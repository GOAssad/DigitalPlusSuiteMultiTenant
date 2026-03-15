using Dapper;
using DigitalPlusMultiTenant.Application.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DigitalPlusMultiTenant.Infrastructure.Services;

public class LicenciaService : ILicenciaService
{
    private readonly string _adminConnectionString;
    private readonly string _defaultConnectionString;
    private readonly IMemoryCache _cache;
    private readonly ILogger<LicenciaService> _logger;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public LicenciaService(
        IConfiguration configuration,
        IMemoryCache cache,
        ILogger<LicenciaService> logger)
    {
        _adminConnectionString = configuration.GetConnectionString("AdminConnection")
            ?? throw new InvalidOperationException("AdminConnection string not configured.");
        _defaultConnectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection string not configured.");
        _cache = cache;
        _logger = logger;
    }

    public async Task<LicenciaInfo> GetLicenciaInfoAsync(int empresaId)
    {
        var cacheKey = $"licencia_{empresaId}";

        if (_cache.TryGetValue<LicenciaInfo>(cacheKey, out var cached) && cached is not null)
            return cached;

        try
        {
            // 1. Obtener Codigo de la empresa en DigitalPlusMultiTenant
            string? companyId;
            await using (var conn = new SqlConnection(_defaultConnectionString))
            {
                companyId = await conn.QueryFirstOrDefaultAsync<string>(
                    "SELECT Codigo FROM Empresa WHERE Id = @EmpresaId",
                    new { EmpresaId = empresaId });
            }

            if (string.IsNullOrEmpty(companyId))
            {
                _logger.LogWarning("Empresa {EmpresaId} no tiene Codigo asignado, usando plan free por defecto", empresaId);
                return CacheAndReturn(cacheKey, new LicenciaInfo());
            }

            // 2. Buscar licencia en DigitalPlusAdmin
            LicenciaInfo? licencia;
            await using (var conn = new SqlConnection(_adminConnectionString))
            {
                licencia = await conn.QueryFirstOrDefaultAsync<LicenciaInfo>(
                    @"SELECT l.[Plan], l.MaxLegajos, l.MaxSucursales, l.MaxFichadasMes, l.LicenseType, l.ExpiresAt,
                             ISNULL((SELECT TOP 1 CAST(Valor AS int) FROM PlanConfig WHERE [Plan] = l.[Plan] AND Parametro = 'MaxTerminalesMoviles'), 1) AS MaxTerminalesMoviles
                      FROM Licencias l
                      WHERE l.CompanyId = @CompanyId",
                    new { CompanyId = companyId });
            }

            if (licencia is null)
            {
                _logger.LogWarning("No se encontro licencia en DigitalPlusAdmin para CompanyId={CompanyId} (EmpresaId={EmpresaId}), usando plan free",
                    companyId, empresaId);
                return CacheAndReturn(cacheKey, new LicenciaInfo());
            }

            return CacheAndReturn(cacheKey, licencia);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener licencia para EmpresaId={EmpresaId}", empresaId);
            // Ante error de conexion, devolver free para no bloquear la app
            return new LicenciaInfo();
        }
    }

    public async Task<(bool permitido, string? mensaje)> PuedeCrearLegajoAsync(int empresaId, int legajosActuales)
    {
        var licencia = await GetLicenciaInfoAsync(empresaId);

        if (licencia.EsIlimitado(licencia.MaxLegajos))
            return (true, null);

        if (legajosActuales >= licencia.MaxLegajos)
            return (false, $"El plan {licencia.PlanDisplay} permite un máximo de {licencia.MaxLegajos} legajos. Actualmente tiene {legajosActuales}.");

        return (true, null);
    }

    public async Task<(bool permitido, string? mensaje)> PuedeCrearSucursalAsync(int empresaId, int sucursalesActuales)
    {
        var licencia = await GetLicenciaInfoAsync(empresaId);

        if (licencia.EsIlimitado(licencia.MaxSucursales))
            return (true, null);

        if (sucursalesActuales >= licencia.MaxSucursales)
            return (false, $"El plan {licencia.PlanDisplay} permite un máximo de {licencia.MaxSucursales} sucursales. Actualmente tiene {sucursalesActuales}.");

        return (true, null);
    }

    public async Task<(bool permitido, string? mensaje)> PuedeRegistrarFichadaAsync(int empresaId, int fichadasUlt30d)
    {
        var licencia = await GetLicenciaInfoAsync(empresaId);

        if (licencia.EsIlimitado(licencia.MaxFichadasMes))
            return (true, null);

        if (fichadasUlt30d >= licencia.MaxFichadasMes)
            return (false, $"El plan {licencia.PlanDisplay} permite un máximo de {licencia.MaxFichadasMes} fichadas por mes. Se alcanzó el límite ({fichadasUlt30d}).");

        return (true, null);
    }

    public async Task<(bool permitido, string? mensaje)> PuedeRegistrarTerminalMovilAsync(int empresaId, int terminalesActivas)
    {
        var licencia = await GetLicenciaInfoAsync(empresaId);

        if (licencia.EsIlimitado(licencia.MaxTerminalesMoviles))
            return (true, null);

        if (terminalesActivas >= licencia.MaxTerminalesMoviles)
            return (false, $"El plan {licencia.PlanDisplay} permite un máximo de {licencia.MaxTerminalesMoviles} terminales móviles. Actualmente tiene {terminalesActivas}.");

        return (true, null);
    }

    public void InvalidarCache(int empresaId)
    {
        _cache.Remove($"licencia_{empresaId}");
    }

    private LicenciaInfo CacheAndReturn(string cacheKey, LicenciaInfo info)
    {
        _cache.Set(cacheKey, info, CacheDuration);
        return info;
    }
}
