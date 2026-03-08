using DigitalPlusMultiTenant.Domain.Common;

namespace DigitalPlusMultiTenant.Domain.Entities;

public class EventoCalendario : BaseEntity, ITenantEntity
{
    public int EmpresaId { get; set; }
    public int LegajoId { get; set; }
    public DateOnly FechaDesde { get; set; }
    public DateOnly FechaHasta { get; set; }
    public string? Nota { get; set; }

    // Navigation
    public Empresa Empresa { get; set; } = null!;
    public Legajo Legajo { get; set; } = null!;
}
