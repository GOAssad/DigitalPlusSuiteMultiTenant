using DigitalPlusMultiTenant.Domain.Common;

namespace DigitalPlusMultiTenant.Domain.Entities;

public class Horario : TenantEntity
{
    public string Nombre { get; set; } = null!;
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<HorarioDetalle> Detalles { get; set; } = [];
    public ICollection<Legajo> Legajos { get; set; } = [];
}
