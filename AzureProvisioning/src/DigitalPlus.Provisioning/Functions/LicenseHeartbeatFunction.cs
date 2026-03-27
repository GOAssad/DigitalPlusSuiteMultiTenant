using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DigitalPlus.Provisioning.Models;
using DigitalPlus.Provisioning.Services;

namespace DigitalPlus.Provisioning.Functions;

public class LicenseHeartbeatFunction
{
    private readonly LicenseService _licenseService;
    private readonly ILogger<LicenseHeartbeatFunction> _logger;

    public LicenseHeartbeatFunction(
        LicenseService licenseService,
        ILogger<LicenseHeartbeatFunction> logger)
    {
        _licenseService = licenseService;
        _logger = logger;
    }

    [Function("LicenseHeartbeat")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "license/heartbeat")]
        HttpRequest req)
    {
        // Endpoint publico — los Fichadores no tienen API key

        // --- Parse request ---
        LicenseHeartbeatRequest? request;
        try
        {
            request = await JsonSerializer.DeserializeAsync<LicenseHeartbeatRequest>(
                req.Body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException)
        {
            return new BadRequestObjectResult(new { error = "Invalid JSON payload." });
        }

        if (request is null)
            return new BadRequestObjectResult(new { error = "Empty request body." });

        // --- Validate fields ---
        if (string.IsNullOrWhiteSpace(request.CompanyId))
            return new BadRequestObjectResult(new { error = "companyId is required." });

        if (string.IsNullOrWhiteSpace(request.MachineId))
            return new BadRequestObjectResult(new { error = "machineId is required." });

        // --- Heartbeat ---
        try
        {
            var (response, error, statusCode) = await _licenseService.HeartbeatAsync(request);

            if (error != null)
                return new ObjectResult(new { error }) { StatusCode = statusCode };

            return new OkObjectResult(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "License heartbeat error");
            return new ObjectResult(new { error = "Error interno en heartbeat." })
                { StatusCode = 500 };
        }
    }
}
