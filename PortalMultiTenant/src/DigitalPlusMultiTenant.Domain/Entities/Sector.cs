using DigitalPlusMultiTenant.Domain.Common;

namespace DigitalPlusMultiTenant.Domain.Entities;

public class Sector : TenantEntity
{
    public string Nombre { get; set; } = null!;
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<Legajo> Legajos { get; set; } = [];
}
