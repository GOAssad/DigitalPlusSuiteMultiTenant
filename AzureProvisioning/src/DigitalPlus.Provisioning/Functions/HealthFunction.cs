using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DigitalPlus.Provisioning.Functions;

public class HealthFunction
{
    private readonly string _connStr;

    public HealthFunction(IConfiguration config)
    {
        _connStr = config["ProvisioningDb"] ?? "";
    }

    [Function("Health")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")]
        HttpRequest req)
    {
        try
        {
            await using var conn = new SqlConnection(_connStr);
            await conn.OpenAsync();
            return new OkObjectResult(new { status = "healthy", timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            return new ObjectResult(new { status = "unhealthy", error = ex.Message })
                { StatusCode = 503 };
        }
    }
}
