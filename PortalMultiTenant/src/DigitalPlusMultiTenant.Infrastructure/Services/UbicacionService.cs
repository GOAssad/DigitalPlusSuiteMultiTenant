namespace DigitalPlusMultiTenant.Infrastructure.Services;

/// <summary>
/// Resuelve la sucursal del empleado por ubicación (WiFi BSSID o GPS).
/// </summary>
public class UbicacionService
{
    /// <summary>
    /// Resultado de la resolución de sucursal por ubicación.
    /// </summary>
    public record UbicacionResult(bool Ok, int? SucursalId = null, string? SucursalNombre = null, string? Error = null);

    /// <summary>
    /// Intenta resolver la sucursal a partir de WiFi BSSID y/o coordenadas GPS,
    /// comparando contra las geoconfiguraciones activas de la empresa.
    /// </summary>
    public UbicacionResult ResolverSucursal(
        IEnumerable<GeoConfigInfo> configs,
        string? wifiBssid,
        decimal? latitud,
        decimal? longitud)
    {
        if (!configs.Any())
            return new UbicacionResult(false, Error: "No hay sucursales con fichado móvil habilitado.");

        foreach (var config in configs.OrderBy(c => c.SucursalId))
        {
            bool match = config.MetodoValidacion switch
            {
                "SoloWifi" => MatchWifi(wifiBssid, config.WifiBSSID),
                "SoloGPS" => MatchGps(latitud, longitud, config.Latitud, config.Longitud, config.RadioMetros),
                "WifiYGPS" => MatchWifi(wifiBssid, config.WifiBSSID)
                              && MatchGps(latitud, longitud, config.Latitud, config.Longitud, config.RadioMetros),
                "Ninguno" => true,
                _ => MatchWifi(wifiBssid, config.WifiBSSID)   // "WifiOGPS" (default)
                     || MatchGps(latitud, longitud, config.Latitud, config.Longitud, config.RadioMetros),
            };

            if (match)
                return new UbicacionResult(true, config.SucursalId, config.SucursalNombre);
        }

        return new UbicacionResult(false, Error: "No se detectó ninguna sucursal habilitada para fichado móvil en tu ubicación actual.");
    }

    private static bool MatchWifi(string? bssidRequest, string? bssidConfig)
    {
        if (string.IsNullOrEmpty(bssidRequest) || string.IsNullOrEmpty(bssidConfig))
            return false;
        return string.Equals(bssidRequest.Trim(), bssidConfig.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    private static bool MatchGps(decimal? lat, decimal? lon, decimal? configLat, decimal? configLon, int radioMetros)
    {
        if (!lat.HasValue || !lon.HasValue || !configLat.HasValue || !configLon.HasValue)
            return false;

        double distancia = CalcularDistanciaMetros(
            (double)lat.Value, (double)lon.Value,
            (double)configLat.Value, (double)configLon.Value);

        return distancia <= radioMetros;
    }

    /// <summary>
    /// Calcula la distancia en metros entre dos puntos geográficos usando la fórmula de Haversine.
    /// </summary>
    private static double CalcularDistanciaMetros(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371000; // Radio de la Tierra en metros
        double dLat = ToRad(lat2 - lat1);
        double dLon = ToRad(lon2 - lon1);
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                   + Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2))
                   * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    private static double ToRad(double deg) => deg * Math.PI / 180.0;

    /// <summary>
    /// DTO con la info de geoconfiguración necesaria para resolver ubicación.
    /// </summary>
    public record GeoConfigInfo(
        int SucursalId,
        string SucursalNombre,
        string? WifiBSSID,
        decimal? Latitud,
        decimal? Longitud,
        int RadioMetros,
        string MetodoValidacion);
}
