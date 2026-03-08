using DigitalPlusMultiTenant.Domain.Common;

namespace DigitalPlusMultiTenant.Domain.Entities;

public class Terminal : TenantEntity
{
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
    public int SucursalId { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public Sucursal Sucursal { get; set; } = null!;
}
