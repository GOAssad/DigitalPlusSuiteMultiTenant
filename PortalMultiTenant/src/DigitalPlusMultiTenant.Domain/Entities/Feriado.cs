using DigitalPlusMultiTenant.Domain.Common;

namespace DigitalPlusMultiTenant.Domain.Entities;

public class Feriado : BaseEntity, ITenantEntity
{
    public int EmpresaId { get; set; }
    public string Nombre { get; set; } = null!;
    public DateOnly Fecha { get; set; }

    // Navigation
    public Empresa Empresa { get; set; } = null!;
}
