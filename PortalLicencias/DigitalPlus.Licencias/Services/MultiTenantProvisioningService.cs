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
    /// Obtiene estadísticas de uso de la empresa desde DigitalPlusMultiTenant.
    /// </summary>
    public async Task<EmpresaStats?> GetEmpresaStatsAsync(string codigo)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        // Buscar EmpresaId
        int empresaId;
        await using (var cmd = new SqlCommand("SELECT Id FROM Empresa WHERE Codigo = @Codigo", conn))
        {
            cmd.Parameters.AddWithValue("@Codigo", codigo);
            var result = await cmd.ExecuteScalarAsync();
            if (result == null) return null;
            empresaId = (int)result;
        }

        var stats = new EmpresaStats();

        const string sql = @"
            -- Legajos
            SELECT @TotalLegajos = COUNT(*), @LegajosActivos = SUM(CASE WHEN IsActive=1 THEN 1 ELSE 0 END)
            FROM Legajo WHERE EmpresaId = @EmpresaId;

            -- Fichadas totales
            SELECT @TotalFichadas = COUNT(*) FROM Fichada WHERE EmpresaId = @EmpresaId;

            -- Sucursales
            SELECT @TotalSucursales = COUNT(*) FROM Sucursal WHERE EmpresaId = @EmpresaId AND IsActive=1;

            -- Terminales desktop
            SELECT @TotalTerminales = COUNT(*) FROM Terminal WHERE EmpresaId = @EmpresaId AND IsActive=1;

            -- Terminales moviles
            SELECT @TotalMoviles = COUNT(*) FROM TerminalesMoviles WHERE EmpresaId = @EmpresaId AND Activo=1;

            -- Usuarios portal
            SELECT @TotalUsuarios = COUNT(*) FROM AspNetUsers WHERE EmpresaId = @EmpresaId AND IsActive=1;

            -- Ultima fichada y stats del ultimo dia
            DECLARE @UltimaFecha DATETIME;
            SELECT @UltimaFecha = MAX(FechaHora) FROM Fichada WHERE EmpresaId = @EmpresaId;
            SET @UltimaFichada = @UltimaFecha;

            IF @UltimaFecha IS NOT NULL
            BEGIN
                DECLARE @FechaDia DATE = CAST(@UltimaFecha AS DATE);

                SELECT @FichadasUltimoDia = COUNT(*),
                       @LegajosUltimoDia = COUNT(DISTINCT LegajoId),
                       @FichadasHuella = SUM(CASE WHEN Origen = 'Huella' THEN 1 ELSE 0 END),
                       @FichadasPIN = SUM(CASE WHEN Origen = 'PIN' THEN 1 ELSE 0 END),
                       @FichadasMovil = SUM(CASE WHEN Origen = 'Movil' THEN 1 ELSE 0 END),
                       @FichadasManual = SUM(CASE WHEN Origen = 'Manual' THEN 1 ELSE 0 END),
                       @FichadasWeb = SUM(CASE WHEN Origen = 'Web' THEN 1 ELSE 0 END),
                       @FichadasDemo = SUM(CASE WHEN Origen = 'Demo' THEN 1 ELSE 0 END)
                FROM Fichada
                WHERE EmpresaId = @EmpresaId AND CAST(FechaHora AS DATE) = @FechaDia;

                -- Dias con actividad (ultimos 30 dias)
                SELECT @DiasActivos30 = COUNT(DISTINCT CAST(FechaHora AS DATE))
                FROM Fichada
                WHERE EmpresaId = @EmpresaId AND FechaHora >= DATEADD(DAY, -30, GETDATE());
            END

            -- Fichadas por dispositivo ultimos 15 dias
            SELECT @Fichadas15dTotal = COUNT(*),
                   @Fichadas15dHuella = SUM(CASE WHEN Origen = 'Huella' THEN 1 ELSE 0 END),
                   @Fichadas15dPIN = SUM(CASE WHEN Origen = 'PIN' THEN 1 ELSE 0 END),
                   @Fichadas15dMovil = SUM(CASE WHEN Origen = 'Movil' THEN 1 ELSE 0 END)
            FROM Fichada
            WHERE EmpresaId = @EmpresaId AND FechaHora >= DATEADD(DAY, -15, GETDATE());";

        await using var cmd2 = new SqlCommand(sql, conn);
        cmd2.Parameters.AddWithValue("@EmpresaId", empresaId);
        cmd2.Parameters.Add("@TotalLegajos", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
        cmd2.Parameters.Add("@LegajosActivos", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
        cmd2.Parameters.Add("@TotalFichadas", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
        cmd2.Parameters.Add("@TotalSucursales", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
        cmd2.Parameters.Add("@TotalTerminales", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
        cmd2.Parameters.Add("@TotalMoviles", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
        cmd2.Parameters.Add("@TotalUsuarios", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
        cmd2.Parameters.Add("@UltimaFichada", System.Data.SqlDbType.DateTime).Direction = System.Data.ParameterDirection.Output;
        cmd2.Parameters.Add("@FichadasUltimoDia", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
        cmd2.Parameters.Add("@LegajosUltimoDia", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
        cmd2.Parameters.Add("@FichadasHuella", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
        cmd2.Parameters.Add("@FichadasPIN", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
        cmd2.Parameters.Add("@FichadasMovil", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
        cmd2.Parameters.Add("@FichadasManual", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
        cmd2.Parameters.Add("@FichadasWeb", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
        cmd2.Parameters.Add("@FichadasDemo", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
        cmd2.Parameters.Add("@DiasActivos30", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
        cmd2.Parameters.Add("@Fichadas15dTotal", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
        cmd2.Parameters.Add("@Fichadas15dHuella", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
        cmd2.Parameters.Add("@Fichadas15dPIN", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
        cmd2.Parameters.Add("@Fichadas15dMovil", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;

        await cmd2.ExecuteNonQueryAsync();

        stats.TotalLegajos = (int)(cmd2.Parameters["@TotalLegajos"].Value ?? 0);
        stats.LegajosActivos = (int)(cmd2.Parameters["@LegajosActivos"].Value ?? 0);
        stats.TotalFichadas = (int)(cmd2.Parameters["@TotalFichadas"].Value ?? 0);
        stats.TotalSucursales = (int)(cmd2.Parameters["@TotalSucursales"].Value ?? 0);
        stats.TotalTerminales = (int)(cmd2.Parameters["@TotalTerminales"].Value ?? 0);
        stats.TotalMoviles = (int)(cmd2.Parameters["@TotalMoviles"].Value ?? 0);
        stats.TotalUsuarios = (int)(cmd2.Parameters["@TotalUsuarios"].Value ?? 0);
        stats.UltimaFichada = cmd2.Parameters["@UltimaFichada"].Value as DateTime?;
        stats.FichadasUltimoDia = cmd2.Parameters["@FichadasUltimoDia"].Value as int? ?? 0;
        stats.LegajosUltimoDia = cmd2.Parameters["@LegajosUltimoDia"].Value as int? ?? 0;
        stats.FichadasHuella = cmd2.Parameters["@FichadasHuella"].Value as int? ?? 0;
        stats.FichadasPIN = cmd2.Parameters["@FichadasPIN"].Value as int? ?? 0;
        stats.FichadasMovil = cmd2.Parameters["@FichadasMovil"].Value as int? ?? 0;
        stats.FichadasManual = cmd2.Parameters["@FichadasManual"].Value as int? ?? 0;
        stats.FichadasWeb = cmd2.Parameters["@FichadasWeb"].Value as int? ?? 0;
        stats.FichadasDemo = cmd2.Parameters["@FichadasDemo"].Value as int? ?? 0;
        stats.DiasActivos30 = cmd2.Parameters["@DiasActivos30"].Value as int? ?? 0;
        stats.Fichadas15dTotal = cmd2.Parameters["@Fichadas15dTotal"].Value as int? ?? 0;
        stats.Fichadas15dHuella = cmd2.Parameters["@Fichadas15dHuella"].Value as int? ?? 0;
        stats.Fichadas15dPIN = cmd2.Parameters["@Fichadas15dPIN"].Value as int? ?? 0;
        stats.Fichadas15dMovil = cmd2.Parameters["@Fichadas15dMovil"].Value as int? ?? 0;

        return stats;
    }

    /// <summary>
    /// Busca el EmpresaId en DigitalPlusMultiTenant por el Codigo de la empresa.
    /// </summary>
    public async Task<int?> BuscarEmpresaIdPorCodigoAsync(string codigo)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        const string sql = "SELECT Id FROM Empresa WHERE Codigo = @Codigo";
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

    /// <summary>
    /// Elimina datos transaccionales de la empresa en DigitalPlusMultiTenant.
    /// Mantiene: empresa, usuarios, sucursales, sectores, categorías, horarios, terminales,
    /// legajos (con huellas, PINs, sucursales asignadas), incidencias, feriados, noticias, variables.
    /// Elimina: fichadas, vacaciones, incidencias asignadas, eventos calendario,
    /// terminales móviles, códigos activación móvil.
    /// </summary>
    public async Task<int> LimpiarEmpresaAsync(int empresaId)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        await using var tx = (SqlTransaction)await conn.BeginTransactionAsync();

        try
        {
            int totalRows = 0;

            string[] deleteStatements =
            [
                "DELETE FROM CodigosActivacionMovil WHERE EmpresaId = @EmpresaId",
                "DELETE FROM TerminalesMoviles WHERE EmpresaId = @EmpresaId",
                "DELETE FROM IncidenciaLegajo WHERE EmpresaId = @EmpresaId",
                "DELETE FROM EventoCalendario WHERE EmpresaId = @EmpresaId",
                "DELETE FROM Vacacion WHERE EmpresaId = @EmpresaId",
                "DELETE FROM Fichada WHERE EmpresaId = @EmpresaId",
            ];

            foreach (var sql in deleteStatements)
            {
                await using var cmd = new SqlCommand(sql, conn, tx);
                cmd.Parameters.AddWithValue("@EmpresaId", empresaId);
                totalRows += await cmd.ExecuteNonQueryAsync();
            }

            await tx.CommitAsync();

            _logger.LogWarning("LIMPIAR EMPRESA: EmpresaId={EmpresaId}, {TotalRows} registros transaccionales eliminados.",
                empresaId, totalRows);

            return totalRows;
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Elimina TODOS los datos de la empresa en DigitalPlusMultiTenant y en DigitalPlusAdmin.
    /// Esto es irreversible.
    /// </summary>
    public async Task EliminarEmpresaAsync(int empresaId, int adminEmpresaId, string companyId,
        string adminConnectionString)
    {
        // 1. Eliminar todo de DigitalPlusMultiTenant
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        await using var tx = (SqlTransaction)await conn.BeginTransactionAsync();

        try
        {
            // Child tables sin EmpresaId (FK a tablas con EmpresaId)
            string[] childDeletes =
            [
                "DELETE lh FROM LegajoHuella lh INNER JOIN Legajo l ON lh.LegajoId = l.Id WHERE l.EmpresaId = @EmpresaId",
                "DELETE lp FROM LegajoPin lp INNER JOIN Legajo l ON lp.LegajoId = l.Id WHERE l.EmpresaId = @EmpresaId",
                "DELETE ls FROM LegajoSucursal ls INNER JOIN Legajo l ON ls.LegajoId = l.Id WHERE l.EmpresaId = @EmpresaId",
                "DELETE ld FROM LegajoDomicilio ld INNER JOIN Legajo l ON ld.LegajoId = l.Id WHERE l.EmpresaId = @EmpresaId",
                "DELETE hd FROM HorarioDetalle hd INNER JOIN Horario h ON hd.HorarioId = h.Id WHERE h.EmpresaId = @EmpresaId",
            ];

            // Tablas transaccionales con EmpresaId
            string[] transactionalDeletes =
            [
                "DELETE FROM CodigosActivacionMovil WHERE EmpresaId = @EmpresaId",
                "DELETE FROM TerminalesMoviles WHERE EmpresaId = @EmpresaId",
                "DELETE FROM IncidenciaLegajo WHERE EmpresaId = @EmpresaId",
                "DELETE FROM EventoCalendario WHERE EmpresaId = @EmpresaId",
                "DELETE FROM Vacacion WHERE EmpresaId = @EmpresaId",
                "DELETE FROM Fichada WHERE EmpresaId = @EmpresaId",
            ];

            // Tablas estructurales con EmpresaId
            string[] structuralDeletes =
            [
                "DELETE FROM SucursalGeoconfigs WHERE EmpresaId = @EmpresaId",
                "DELETE FROM Legajo WHERE EmpresaId = @EmpresaId",
                "DELETE FROM Terminal WHERE EmpresaId = @EmpresaId",
                "DELETE FROM Sucursal WHERE EmpresaId = @EmpresaId",
                "DELETE FROM Sector WHERE EmpresaId = @EmpresaId",
                "DELETE FROM Categoria WHERE EmpresaId = @EmpresaId",
                "DELETE FROM Horario WHERE EmpresaId = @EmpresaId",
                "DELETE FROM Incidencia WHERE EmpresaId = @EmpresaId",
                "DELETE FROM Feriado WHERE EmpresaId = @EmpresaId",
                "DELETE FROM Noticia WHERE EmpresaId = @EmpresaId",
                "DELETE FROM VariableSistema WHERE EmpresaId = @EmpresaId",
            ];

            // Usuarios de Identity y relaciones
            string[] userDeletes =
            [
                "DELETE us FROM UsuarioSucursal us INNER JOIN AspNetUsers u ON us.UserId = u.Id WHERE u.EmpresaId = @EmpresaId",
                "DELETE ur FROM AspNetUserRoles ur INNER JOIN AspNetUsers u ON ur.UserId = u.Id WHERE u.EmpresaId = @EmpresaId",
                "DELETE uc FROM AspNetUserClaims uc INNER JOIN AspNetUsers u ON uc.UserId = u.Id WHERE u.EmpresaId = @EmpresaId",
                "DELETE ul FROM AspNetUserLogins ul INNER JOIN AspNetUsers u ON ul.UserId = u.Id WHERE u.EmpresaId = @EmpresaId",
                "DELETE ut FROM AspNetUserTokens ut INNER JOIN AspNetUsers u ON ut.UserId = u.Id WHERE u.EmpresaId = @EmpresaId",
                "DELETE FROM AspNetUsers WHERE EmpresaId = @EmpresaId",
            ];

            // La empresa misma
            const string deleteEmpresa = "DELETE FROM Empresa WHERE Id = @EmpresaId";

            var allDeletes = childDeletes
                .Concat(transactionalDeletes)
                .Concat(structuralDeletes)
                .Concat(userDeletes)
                .Append(deleteEmpresa);

            foreach (var sql in allDeletes)
            {
                await using var cmd = new SqlCommand(sql, conn, tx);
                cmd.Parameters.AddWithValue("@EmpresaId", empresaId);
                await cmd.ExecuteNonQueryAsync();
            }

            await tx.CommitAsync();

            _logger.LogWarning("ELIMINAR EMPRESA (MT): EmpresaId={EmpresaId} eliminada completamente de DigitalPlusMultiTenant.",
                empresaId);
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }

        // 2. Eliminar de DigitalPlusAdmin (licencias + empresa)
        await using var adminConn = new SqlConnection(adminConnectionString);
        await adminConn.OpenAsync();
        await using var adminTx = (SqlTransaction)await adminConn.BeginTransactionAsync();

        try
        {
            await using (var cmd = new SqlCommand("DELETE FROM Licencias WHERE CompanyId = @CompanyId", adminConn, adminTx))
            {
                cmd.Parameters.AddWithValue("@CompanyId", companyId);
                await cmd.ExecuteNonQueryAsync();
            }

            await using (var cmd = new SqlCommand("DELETE FROM Empresas WHERE Id = @Id", adminConn, adminTx))
            {
                cmd.Parameters.AddWithValue("@Id", adminEmpresaId);
                await cmd.ExecuteNonQueryAsync();
            }

            await adminTx.CommitAsync();

            _logger.LogWarning("ELIMINAR EMPRESA (Admin): AdminId={AdminId}, CompanyId={CompanyId} eliminada de DigitalPlusAdmin.",
                adminEmpresaId, companyId);
        }
        catch
        {
            await adminTx.RollbackAsync();
            throw;
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

public class EmpresaStats
{
    public int TotalLegajos { get; set; }
    public int LegajosActivos { get; set; }
    public int TotalFichadas { get; set; }
    public int TotalSucursales { get; set; }
    public int TotalTerminales { get; set; }
    public int TotalMoviles { get; set; }
    public int TotalUsuarios { get; set; }
    public DateTime? UltimaFichada { get; set; }
    public int FichadasUltimoDia { get; set; }
    public int LegajosUltimoDia { get; set; }
    public int FichadasHuella { get; set; }
    public int FichadasPIN { get; set; }
    public int FichadasMovil { get; set; }
    public int FichadasManual { get; set; }
    public int FichadasWeb { get; set; }
    public int FichadasDemo { get; set; }
    public int DiasActivos30 { get; set; }
    public int Fichadas15dHuella { get; set; }
    public int Fichadas15dPIN { get; set; }
    public int Fichadas15dMovil { get; set; }
    public int Fichadas15dTotal { get; set; }
}
