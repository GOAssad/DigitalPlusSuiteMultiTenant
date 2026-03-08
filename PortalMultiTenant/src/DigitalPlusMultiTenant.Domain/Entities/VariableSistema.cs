using DigitalPlusMultiTenant.Domain.Common;

namespace DigitalPlusMultiTenant.Domain.Entities;

public class VariableSistema : BaseEntity, ITenantEntity
{
    public int EmpresaId { get; set; }
    public string Clave { get; set; } = null!;
    public string? Descripcion { get; set; }
    public string? TipoValor { get; set; }
    public string? Valor { get; set; }
    public bool RequiereReinicio { get; set; }

    // Navigation
    public Empresa Empresa { get; set; } = null!;
}
