using DigitalPlusMultiTenant.Domain.Common;

namespace DigitalPlusMultiTenant.Domain.Entities;

public class Noticia : BaseEntity, ITenantEntity
{
    public int EmpresaId { get; set; }
    public string Titulo { get; set; } = null!;
    public string? Contenido { get; set; }
    public DateOnly FechaDesde { get; set; }
    public DateOnly FechaHasta { get; set; }
    public bool IsPrivada { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }

    // Navigation
    public Empresa Empresa { get; set; } = null!;
}
