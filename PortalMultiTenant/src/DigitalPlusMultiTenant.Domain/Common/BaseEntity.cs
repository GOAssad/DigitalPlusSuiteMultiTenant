namespace DigitalPlusMultiTenant.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
}

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}

public interface ITenantEntity
{
    int EmpresaId { get; set; }
}

public abstract class TenantEntity : AuditableEntity, ITenantEntity
{
    public int EmpresaId { get; set; }
    public Empresa Empresa { get; set; } = null!;
}
