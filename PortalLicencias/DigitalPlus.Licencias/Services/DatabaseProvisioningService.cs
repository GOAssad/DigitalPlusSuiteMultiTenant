using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace DigitalPlus.Licencias.Services;

public class DatabaseProvisioningService
{
    private readonly string _server;
    private readonly string _user;
    private readonly string _password;
    private readonly ILogger<DatabaseProvisioningService> _logger;

    private static readonly Regex[] SkipPatterns =
    [
        new(@"^\s*CREATE\s+DATABASE\b", RegexOptions.IgnoreCase),
        new(@"^\s*ALTER\s+DATABASE\b", RegexOptions.IgnoreCase),
        new(@"^\s*USE\s+", RegexOptions.IgnoreCase),
        new(@"^\s*CREATE\s+USER\b", RegexOptions.IgnoreCase),
        new(@"^\s*ALTER\s+ROLE\b", RegexOptions.IgnoreCase),
        new(@"^\s*IF\s+.*FULLTEXTSERVICEPROPERTY", RegexOptions.IgnoreCase),
    ];

    public DatabaseProvisioningService(IConfiguration config, ILogger<DatabaseProvisioningService> logger)
    {
        _server = config["CloudSql:Server"]
            ?? throw new InvalidOperationException("Missing CloudSql:Server.");
        _user = config["CloudSql:User"]
            ?? throw new InvalidOperationException("Missing CloudSql:User.");
        _password = config["CloudSql:Password"]
            ?? throw new InvalidOperationException("Missing CloudSql:Password.");
        _logger = logger;
    }

    public async Task<bool> DatabaseExistsAsync(string dbName)
    {
        await using var conn = new SqlConnection(BuildConnectionString("master"));
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM sys.databases WHERE name = @name";
        cmd.Parameters.AddWithValue("@name", dbName);
        return (int)(await cmd.ExecuteScalarAsync())! > 0;
    }

    public async Task CreateDatabaseAsync(string dbName)
    {
        _logger.LogInformation("Creando base de datos: {DbName}", dbName);

        await using (var conn = new SqlConnection(BuildConnectionString("master")))
        {
            await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = $"CREATE DATABASE [{dbName}]";
            cmd.CommandTimeout = 300;
            await cmd.ExecuteNonQueryAsync();
        }

        // Esperar a que la DB este disponible
        for (int i = 0; i < 10; i++)
        {
            if (await DatabaseExistsAsync(dbName)) break;
            await Task.Delay(1000);
        }

        var schemaScript = ReadEmbeddedScript();
        schemaScript = schemaScript.Replace("[DigitalPlus]", $"[{dbName}]");
        schemaScript = schemaScript.Replace("N'DigitalPlus'", $"N'{dbName}'");

        await using var dbConn = new SqlConnection(BuildConnectionString(dbName));
        await dbConn.OpenAsync();

        var batches = Regex.Split(schemaScript, @"(?mi)^\s*GO\s*$");
        int executed = 0, errors = 0;

        foreach (var batch in batches)
        {
            var trimmed = batch.Trim();
            if (string.IsNullOrEmpty(trimmed)) continue;
            if (SkipPatterns.Any(p => p.IsMatch(trimmed))) continue;

            try
            {
                await using var cmd = dbConn.CreateCommand();
                cmd.CommandText = trimmed;
                cmd.CommandTimeout = 300;
                await cmd.ExecuteNonQueryAsync();
                executed++;
            }
            catch (SqlException ex)
            {
                errors++;
                if (errors <= 10)
                    _logger.LogWarning("Error en batch (no fatal) en {DbName}: {Message}", dbName, ex.Message);
            }
        }

        _logger.LogInformation("Base {DbName} creada. {Executed} batches ejecutados, {Errors} errores.", dbName, executed, errors);
    }

    public string BuildClientConnectionString(string dbName)
    {
        return $"Server={_server};Database={dbName};User Id={_user};Password={_password};" +
               "Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
    }

    private string BuildConnectionString(string dbName)
    {
        return $"Server={_server};Database={dbName};User Id={_user};Password={_password};" +
               "Encrypt=True;TrustServerCertificate=True;Connect Timeout=30;";
    }

    private static string ReadEmbeddedScript()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = assembly.GetManifestResourceNames()
            .FirstOrDefault(n => n.EndsWith("SchemaScript.sql"))
            ?? throw new InvalidOperationException("Embedded SchemaScript.sql not found.");

        using var stream = assembly.GetManifestResourceStream(resourceName)!;
        using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
        return reader.ReadToEnd();
    }
}
