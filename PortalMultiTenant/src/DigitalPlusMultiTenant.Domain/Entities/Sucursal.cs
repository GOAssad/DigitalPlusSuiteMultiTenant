using DigitalPlusMultiTenant.Domain.Common;

namespace DigitalPlusMultiTenant.Domain.Entities;

public class Sucursal : TenantEntity
{
    public string Codigo { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string? Direccion { get; set; }
    public string? Localidad { get; set; }
    public string? Provincia { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<Terminal> Terminales { get; set; } = [];
    public ICollection<LegajoSucursal> LegajoSucursales { get; set; } = [];
    public ICollection<Fichada> Fichadas { get; set; } = [];
    public ICollection<UsuarioSucursal> UsuarioSucursales { get; set; } = [];
    public SucursalGeoconfig? Geoconfig { get; set; }
}
