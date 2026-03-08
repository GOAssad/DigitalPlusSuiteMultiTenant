using DigitalPlusMultiTenant.Domain.Common;

namespace DigitalPlusMultiTenant.Domain.Entities;

public class Incidencia : TenantEntity
{
    public string Nombre { get; set; } = null!;
    public string? Color { get; set; }
    public string? Abreviatura { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<IncidenciaLegajo> IncidenciaLegajos { get; set; } = [];
}
