namespace DigitalPlusMultiTenant.Application.Interfaces;

public interface ITenantService
{
    int EmpresaId { get; }
    string? UserId { get; }
    string? UserName { get; }
}
