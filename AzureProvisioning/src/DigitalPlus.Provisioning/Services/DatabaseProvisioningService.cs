using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DigitalPlus.Provisioning.Services;

public class DatabaseProvisioningService
{
    private readonly string _cloudServer;
    private readonly string _cloudUser;
    private readonly string _cloudPassword;
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
        _cloudServer = config["CloudSqlServer"]
            ?? throw new InvalidOperationException("Missing CloudSqlServer.");
        _cloudUser = config["CloudSqlUser"]
            ?? throw new InvalidOperationException("Missing CloudSqlUser.");
        _cloudPassword = config["CloudSqlPassword"]
            ?? throw new InvalidOperationException("Missing CloudSqlPassword.");
        _logger = logger;
    }

    public async Task<bool> DatabaseExistsAsync(string dbName)
    {
        var connStr = BuildMasterConnectionString();
        await using var conn = new SqlConnection(connStr);
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM sys.databases WHERE name = @name";
        cmd.Parameters.AddWithValue("@name", dbName);

        var count = (int)(await cmd.ExecuteScalarAsync())!;
        return count > 0;
    }

    public async Task CreateDatabaseAsync(string dbName)
    {
        _logger.LogInformation("Creating cloud database: {DbName}", dbName);

        var masterConnStr = BuildMasterConnectionString();
        await using (var masterConn = new SqlConnection(masterConnStr))
        {
            await masterConn.OpenAsync();
            await using var createCmd = masterConn.CreateCommand();
            createCmd.CommandText = $"CREATE DATABASE [{dbName}]";
            createCmd.CommandTimeout = 300;
            await createCmd.ExecuteNonQueryAsync();
        }

        if (!await DatabaseExistsAsync(dbName))
            throw new InvalidOperationException(
                $"CREATE DATABASE executed but '{dbName}' not found in sys.databases.");

        var schemaScript = ReadEmbeddedScript();

        schemaScript = schemaScript.Replace("[DigitalPlus]", $"[{dbName}]");
        schemaScript = schemaScript.Replace("N'DigitalPlus'", $"N'{dbName}'");

        var dbConnStr = BuildDatabaseConnectionString(dbName);
        await using var dbConn = new SqlConnection(dbConnStr);
        await dbConn.OpenAsync();

        var batches = Regex.Split(schemaScript, @"(?mi)^\s*GO\s*$");
        int errorCount = 0;

        foreach (var batch in batches)
        {
            var trimmed = batch.Trim();
            if (string.IsNullOrEmpty(trimmed) || trimmed.Equals("GO", StringComparison.OrdinalIgnoreCase))
                continue;

            if (SkipPatterns.Any(p => p.IsMatch(trimmed)))
                continue;

            try
            {
                await using var cmd = dbConn.CreateCommand();
                cmd.CommandText = trimmed;
                cmd.CommandTimeout = 300;
                await cmd.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                errorCount++;
                if (errorCount <= 10)
                    _logger.LogWarning("Batch error (non-fatal) in {DbName}: {Message}",
                        dbName, ex.Message);
            }
        }

        _logger.LogInformation(
            "Database {DbName} created. Schema applied with {ErrorCount} non-fatal errors.",
            dbName, errorCount);
    }

    public string BuildClientConnectionString(string dbName)
    {
        return $"Data Source={_cloudServer};" +
               $"Initial Catalog={dbName};" +
               $"User Id={_cloudUser};" +
               $"Password={_cloudPassword};";
    }

    private string BuildMasterConnectionString()
    {
        return $"Server={_cloudServer};Database=master;" +
               $"User Id={_cloudUser};Password={_cloudPassword};" +
               "Encrypt=True;TrustServerCertificate=True;Connect Timeout=30;";
    }

    private string BuildDatabaseConnectionString(string dbName)
    {
        return $"Server={_cloudServer};Database={dbName};" +
               $"User Id={_cloudUser};Password={_cloudPassword};" +
               "Encrypt=True;TrustServerCertificate=True;Connect Timeout=300;";
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
