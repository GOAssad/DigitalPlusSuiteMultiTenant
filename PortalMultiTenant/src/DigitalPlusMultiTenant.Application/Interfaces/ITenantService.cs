namespace DigitalPlusMultiTenant.Application.Interfaces;

public interface ITenantService
{
    int EmpresaId { get; }
    string? EmpresaNombre { get; }
    bool MobileHabilitado { get; }
    bool KioskoHabilitado { get; }
    string? UserId { get; }
    string? UserName { get; }
}
