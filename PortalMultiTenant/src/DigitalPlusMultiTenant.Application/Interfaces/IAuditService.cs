namespace DigitalPlusMultiTenant.Application.Interfaces;

public interface IAuditService
{
    Task LogAsync(
        int empresaId,
        string action,
        string? entityType = null,
        string? entityId = null,
        string? description = null,
        string? userId = null,
        string? userName = null,
        string? ipAddress = null,
        string? source = null);

    Task LogLoginAsync(int empresaId, string userName, string? ipAddress, string source, bool success);
    Task LogFichadaManualAsync(int empresaId, string userName, int legajoId, string legajoNombre, string? ipAddress);
    Task LogImportAsync(int empresaId, string userName, int cantidad, string? ipAddress);
}
