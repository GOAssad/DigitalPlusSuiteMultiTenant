using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DigitalPlus.Provisioning.Services;

public class ActivationCodeService
{
    private readonly string _connectionString;
    private readonly ILogger<ActivationCodeService> _logger;

    public ActivationCodeService(IConfiguration config, ILogger<ActivationCodeService> logger)
    {
        _connectionString = config["ProvisioningDb"]
            ?? throw new InvalidOperationException("Missing ProvisioningDb configuration.");
        _logger = logger;
    }

    public async Task<(int resultCode, DateTime? expiresAt)> ValidateAndConsumeAsync(
        string activationCode,
        string companyName,
        string dbName,
        string machineId,
        string installType)
    {
        var codeHash = ComputeSha256(activationCode);

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "Provisioning_ValidateAndConsumeCode";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.CommandTimeout = 30;

        cmd.Parameters.AddWithValue("@CodeHash", codeHash);
        cmd.Parameters.AddWithValue("@CompanyName", companyName);
        cmd.Parameters.AddWithValue("@DbName", dbName);
        cmd.Parameters.AddWithValue("@MachineId", machineId);
        cmd.Parameters.AddWithValue("@InstallType", installType);

        var resultParam = new SqlParameter("@Result", System.Data.SqlDbType.Int)
            { Direction = System.Data.ParameterDirection.Output };
        cmd.Parameters.Add(resultParam);

        var expiresParam = new SqlParameter("@ExpiresAt", System.Data.SqlDbType.DateTime2)
            { Direction = System.Data.ParameterDirection.Output, IsNullable = true };
        cmd.Parameters.Add(expiresParam);

        await cmd.ExecuteNonQueryAsync();

        var resultCode = (int)resultParam.Value;
        var expiresAt = expiresParam.Value == DBNull.Value
            ? (DateTime?)null
            : (DateTime)expiresParam.Value;

        _logger.LogInformation(
            "Activation code validation: hash={HashPrefix}..., result={Result}",
            codeHash[..8], resultCode);

        return (resultCode, expiresAt);
    }

    public static string ComputeSha256(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
