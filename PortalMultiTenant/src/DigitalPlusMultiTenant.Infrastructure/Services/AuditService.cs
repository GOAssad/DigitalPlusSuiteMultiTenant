using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using DigitalPlusMultiTenant.Application.Interfaces;

namespace DigitalPlusMultiTenant.Infrastructure.Services;

public class AuditService : IAuditService
{
    private readonly string _connectionString;

    public AuditService(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    public async Task LogAsync(
        int empresaId,
        string action,
        string? entityType = null,
        string? entityId = null,
        string? description = null,
        string? userId = null,
        string? userName = null,
        string? ipAddress = null,
        string? source = null)
    {
        if (empresaId <= 0) return;

        try
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.ExecuteAsync(
                @"INSERT INTO AuditLog (EmpresaId, [Timestamp], UserId, UserName, Action, EntityType, EntityId, Description, IpAddress, Source)
                  VALUES (@EmpresaId, @Timestamp, @UserId, @UserName, @Action, @EntityType, @EntityId, @Description, @IpAddress, @Source)",
                new
                {
                    EmpresaId = empresaId,
                    Timestamp = DateTime.UtcNow,
                    UserId = userId,
                    UserName = userName,
                    Action = action,
                    EntityType = entityType,
                    EntityId = entityId,
                    Description = description,
                    IpAddress = ipAddress,
                    Source = source ?? "Portal"
                });
        }
        catch
        {
            // Auditoria no debe romper la operacion principal
        }
    }

    public Task LogLoginAsync(int empresaId, string userName, string? ipAddress, string source, bool success)
    {
        return LogAsync(
            empresaId,
            success ? "Login" : "LoginFailed",
            entityType: "Usuario",
            description: success
                ? $"{userName} inició sesión desde {source}"
                : $"Intento fallido de login para {userName}",
            userName: userName,
            ipAddress: ipAddress,
            source: source);
    }

    public Task LogFichadaManualAsync(int empresaId, string userName, int legajoId, string legajoNombre, string? ipAddress)
    {
        return LogAsync(
            empresaId,
            "FichadaManual",
            entityType: "Fichada",
            entityId: legajoId.ToString(),
            description: $"{userName} cargó fichada manual para {legajoNombre}",
            userName: userName,
            ipAddress: ipAddress,
            source: "Portal");
    }

    public Task LogImportAsync(int empresaId, string userName, int cantidad, string? ipAddress)
    {
        return LogAsync(
            empresaId,
            "Import",
            entityType: "Legajo",
            description: $"{userName} importó {cantidad} legajo(s) desde Excel",
            userName: userName,
            ipAddress: ipAddress,
            source: "Portal");
    }
}
