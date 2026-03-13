using DigitalPlusMultiTenant.Domain.Common;

namespace DigitalPlusMultiTenant.Domain.Entities;

public class CodigoActivacionMovil : BaseEntity, ITenantEntity
{
    public int EmpresaId { get; set; }
    public int LegajoId { get; set; }
    public string Codigo { get; set; } = null!;
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaExpira { get; set; }
    public bool Usado { get; set; }
    public DateTime? UsadoEn { get; set; }
    public string? DeviceId { get; set; }

    // Navigation
    public Empresa Empresa { get; set; } = null!;
    public Legajo Legajo { get; set; } = null!;
}
