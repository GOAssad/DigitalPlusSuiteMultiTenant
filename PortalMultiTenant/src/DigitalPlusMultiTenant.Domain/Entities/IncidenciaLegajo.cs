using DigitalPlusMultiTenant.Domain.Common;

namespace DigitalPlusMultiTenant.Domain.Entities;

public class IncidenciaLegajo : BaseEntity, ITenantEntity
{
    public int EmpresaId { get; set; }
    public int IncidenciaId { get; set; }
    public int LegajoId { get; set; }
    public DateOnly Fecha { get; set; }
    public string? Detalle { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }

    // Navigation
    public Empresa Empresa { get; set; } = null!;
    public Incidencia Incidencia { get; set; } = null!;
    public Legajo Legajo { get; set; } = null!;
}
