using DigitalPlusMultiTenant.Domain.Common;

namespace DigitalPlusMultiTenant.Domain.Entities;

public class SucursalGeoconfig : BaseEntity, ITenantEntity
{
    public int SucursalId { get; set; }
    public int EmpresaId { get; set; }
    public string? WifiBSSID { get; set; }
    public string? WifiSSID { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public int RadioMetros { get; set; } = 100;
    public string MetodoValidacion { get; set; } = "WifiOGPS";
    public bool Activo { get; set; } = true;

    // Navigation
    public Empresa Empresa { get; set; } = null!;
    public Sucursal Sucursal { get; set; } = null!;
}
