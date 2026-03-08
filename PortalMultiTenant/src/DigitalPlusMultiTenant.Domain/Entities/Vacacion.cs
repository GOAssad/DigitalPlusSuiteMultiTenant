using DigitalPlusMultiTenant.Domain.Common;

namespace DigitalPlusMultiTenant.Domain.Entities;

public class Vacacion : BaseEntity, ITenantEntity
{
    public int EmpresaId { get; set; }
    public int LegajoId { get; set; }
    public DateOnly FechaDesde { get; set; }
    public DateOnly FechaHasta { get; set; }
    public string? Nota { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }

    // Navigation
    public Empresa Empresa { get; set; } = null!;
    public Legajo Legajo { get; set; } = null!;
}
