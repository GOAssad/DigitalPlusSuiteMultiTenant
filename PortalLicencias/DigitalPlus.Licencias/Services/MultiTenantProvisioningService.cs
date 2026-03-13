using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace DigitalPlus.Licencias.Services;

public class MultiTenantProvisioningService
{
    private readonly string _connectionString;
    private readonly ILogger<MultiTenantProvisioningService> _logger;

    public MultiTenantProvisioningService(IConfiguration config, ILogger<MultiTenantProvisioningService> logger)
    {
        var server = config["CloudSql:Server"]
            ?? throw new InvalidOperationException("Missing CloudSql:Server.");
        var user = config["CloudSql:User"]
            ?? throw new InvalidOperationException("Missing CloudSql:User.");
        var password = config["CloudSql:Password"]
            ?? throw new InvalidOperationException("Missing CloudSql:Password.");
        var dbName = config["MultiTenant:DatabaseName"] ?? "DigitalPlusMultiTenant";

        _connectionString = $"Server={server};Database={dbName};User Id={user};Password={password};" +
                            "Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
        _logger = logger;
    }

    public async Task<(int empresaId, string adminEmail, string tempPassword)> CreateEmpresaAndAdminAsync(
        string codigo, string nombre, string? nombreFantasia, string? contactEmail)
    {
        var adminEmail = !string.IsNullOrWhiteSpace(contactEmail)
            ? contactEmail
            : $"admin@{codigo}.com";

        var tempPassword = "Admin123";

        var hasher = new PasswordHasher<object>();
        var passwordHash = hasher.HashPassword(null!, tempPassword);

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        await using var tx = (SqlTransaction)await conn.BeginTransactionAsync();

        try
        {
            // 1. Crear Empresa en tabla Empresa del Portal MT
            var empresaId = await InsertEmpresaAsync(conn, tx, codigo, nombre, nombreFantasia);

            // 1b. Crear datos default (Sucursal, Categoria, Horario, Sector)
            await InsertDefaultEntitiesAsync(conn, tx, empresaId);

            // 2. Asegurar que el rol AdminEmpresa existe
            var roleId = await EnsureRoleExistsAsync(conn, tx, "AdminEmpresa");

            // 3. Crear usuario admin
            var userId = Guid.NewGuid().ToString();
            await InsertUserAsync(conn, tx, userId, adminEmail, passwordHash, empresaId,
                $"Administrador {nombre}");

            // 4. Asignar rol
            await InsertUserRoleAsync(conn, tx, userId, roleId);

            await tx.CommitAsync();

            _logger.LogInformation(
                "Empresa '{Nombre}' (Id={EmpresaId}) y admin '{Email}' creados en Portal MT.",
                nombre, empresaId, adminEmail);

            return (empresaId, adminEmail, tempPassword);
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    private static async Task<int> InsertEmpresaAsync(SqlConnection conn, SqlTransaction tx,
        string codigo, string nombre, string? nombreFantasia)
    {
        const string sql = @"
            INSERT INTO Empresa (Codigo, Nombre, NombreFantasia, IsActive, CreatedAt, CreatedBy)
            OUTPUT INSERTED.Id
            VALUES (@Codigo, @Nombre, @NombreFantasia, 1, @Now, 'portal-licencias')";

        await using var cmd = new SqlCommand(sql, conn, tx);
        cmd.Parameters.AddWithValue("@Codigo", codigo);
        cmd.Parameters.AddWithValue("@Nombre", nombre);
        cmd.Parameters.AddWithValue("@NombreFantasia", (object?)nombreFantasia ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Now", DateTime.UtcNow);

        return (int)(await cmd.ExecuteScalarAsync())!;
    }

    private static async Task<string> EnsureRoleExistsAsync(SqlConnection conn, SqlTransaction tx, string roleName)
    {
        const string selectSql = "SELECT Id FROM AspNetRoles WHERE NormalizedName = @NormalizedName";
        await using var selectCmd = new SqlCommand(selectSql, conn, tx);
        selectCmd.Parameters.AddWithValue("@NormalizedName", roleName.ToUpperInvariant());

        var existingId = await selectCmd.ExecuteScalarAsync();
        if (existingId != null)
            return existingId.ToString()!;

        var newId = Guid.NewGuid().ToString();
        const string insertSql = @"
            INSERT INTO AspNetRoles (Id, [Name], NormalizedName, ConcurrencyStamp)
            VALUES (@Id, @Name, @NormalizedName, @Stamp)";

        await using var insertCmd = new SqlCommand(insertSql, conn, tx);
        insertCmd.Parameters.AddWithValue("@Id", newId);
        insertCmd.Parameters.AddWithValue("@Name", roleName);
        insertCmd.Parameters.AddWithValue("@NormalizedName", roleName.ToUpperInvariant());
        insertCmd.Parameters.AddWithValue("@Stamp", Guid.NewGuid().ToString());
        await insertCmd.ExecuteNonQueryAsync();

        return newId;
    }

    private static async Task InsertUserAsync(SqlConnection conn, SqlTransaction tx,
        string userId, string email, string passwordHash, int empresaId, string nombreCompleto)
    {
        const string sql = @"
            INSERT INTO AspNetUsers (
                Id, UserName, NormalizedUserName, Email, NormalizedEmail,
                EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp,
                PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount,
                EmpresaId, NombreCompleto, IsActive, AccesoAdminDesktop, MustChangePassword, CreatedAt)
            VALUES (
                @Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail,
                1, @PasswordHash, @SecurityStamp, @ConcurrencyStamp,
                0, 0, 1, 0,
                @EmpresaId, @NombreCompleto, 1, 1, 1, @Now)";

        await using var cmd = new SqlCommand(sql, conn, tx);
        cmd.Parameters.AddWithValue("@Id", userId);
        cmd.Parameters.AddWithValue("@UserName", email);
        cmd.Parameters.AddWithValue("@NormalizedUserName", email.ToUpperInvariant());
        cmd.Parameters.AddWithValue("@Email", email);
        cmd.Parameters.AddWithValue("@NormalizedEmail", email.ToUpperInvariant());
        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
        cmd.Parameters.AddWithValue("@SecurityStamp", Guid.NewGuid().ToString());
        cmd.Parameters.AddWithValue("@ConcurrencyStamp", Guid.NewGuid().ToString());
        cmd.Parameters.AddWithValue("@EmpresaId", empresaId);
        cmd.Parameters.AddWithValue("@NombreCompleto", nombreCompleto);
        cmd.Parameters.AddWithValue("@Now", DateTime.UtcNow);

        await cmd.ExecuteNonQueryAsync();
    }

    private static async Task InsertUserRoleAsync(SqlConnection conn, SqlTransaction tx,
        string userId, string roleId)
    {
        const string sql = "INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (@UserId, @RoleId)";
        await using var cmd = new SqlCommand(sql, conn, tx);
        cmd.Parameters.AddWithValue("@UserId", userId);
        cmd.Parameters.AddWithValue("@RoleId", roleId);
        await cmd.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Busca el EmpresaId en DigitalPlusMultiTenant por el Codigo de la empresa.
    /// </summary>
    public async Task<int?> BuscarEmpresaIdPorCodigoAsync(string codigo)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        const string sql = "SELECT Id FROM Empresa WHERE Codigo = @Codigo AND IsActive = 1";
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Codigo", codigo);

        var result = await cmd.ExecuteScalarAsync();
        return result != null ? (int)result : null;
    }

    private static async Task InsertDefaultEntitiesAsync(SqlConnection conn, SqlTransaction tx, int empresaId)
    {
        var now = DateTime.UtcNow;
        const string createdBy = "portal-licencias";

        // Sucursal default
        const string sqlSucursal = @"
            INSERT INTO Sucursal (Codigo, Nombre, IsActive, CreatedAt, CreatedBy, EmpresaId)
            VALUES ('0001', 'Casa Central', 1, @Now, @CreatedBy, @EmpresaId)";
        await using (var cmd = new SqlCommand(sqlSucursal, conn, tx))
        {
            cmd.Parameters.AddWithValue("@Now", now);
            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
            cmd.Parameters.AddWithValue("@EmpresaId", empresaId);
            await cmd.ExecuteNonQueryAsync();
        }

        // Categoria default
        const string sqlCategoria = @"
            INSERT INTO Categoria (Nombre, IsActive, CreatedAt, CreatedBy, EmpresaId)
            VALUES ('General', 1, @Now, @CreatedBy, @EmpresaId)";
        await using (var cmd = new SqlCommand(sqlCategoria, conn, tx))
        {
            cmd.Parameters.AddWithValue("@Now", now);
            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
            cmd.Parameters.AddWithValue("@EmpresaId", empresaId);
            await cmd.ExecuteNonQueryAsync();
        }

        // Horario default
        const string sqlHorario = @"
            INSERT INTO Horario (Nombre, IsActive, CreatedAt, CreatedBy, EmpresaId)
            VALUES ('General', 1, @Now, @CreatedBy, @EmpresaId)";
        await using (var cmd = new SqlCommand(sqlHorario, conn, tx))
        {
            cmd.Parameters.AddWithValue("@Now", now);
            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
            cmd.Parameters.AddWithValue("@EmpresaId", empresaId);
            await cmd.ExecuteNonQueryAsync();
        }

        // Sector default
        const string sqlSector = @"
            INSERT INTO Sector (Nombre, IsActive, CreatedAt, CreatedBy, EmpresaId)
            VALUES ('General', 1, @Now, @CreatedBy, @EmpresaId)";
        await using (var cmd = new SqlCommand(sqlSector, conn, tx))
        {
            cmd.Parameters.AddWithValue("@Now", now);
            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
            cmd.Parameters.AddWithValue("@EmpresaId", empresaId);
            await cmd.ExecuteNonQueryAsync();
        }
    }

    private static string GenerateTempPassword()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz23456789";
        Span<byte> bytes = stackalloc byte[10];
        RandomNumberGenerator.Fill(bytes);
        return string.Create(10, bytes.ToArray(), (span, b) =>
        {
            for (int i = 0; i < span.Length; i++)
                span[i] = chars[b[i] % chars.Length];
        });
    }
}
