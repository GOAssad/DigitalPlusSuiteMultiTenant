using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DigitalPlus.Provisioning.Models;
using DigitalPlus.Provisioning.Services;

namespace DigitalPlus.Provisioning.Functions;

public class LicenseActivateFunction
{
    private readonly LicenseService _licenseService;
    private readonly ILogger<LicenseActivateFunction> _logger;
    private readonly string? _apiKey;

    public LicenseActivateFunction(
        LicenseService licenseService,
        IConfiguration config,
        ILogger<LicenseActivateFunction> logger)
    {
        _licenseService = licenseService;
        _logger = logger;
        _apiKey = config["ProvisioningApiKey"];
    }

    [Function("LicenseActivate")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "license/activate")]
        HttpRequest req)
    {
        // --- API Key validation ---
        if (!string.IsNullOrEmpty(_apiKey))
        {
            var authHeader = req.Headers["X-Api-Key"].FirstOrDefault();
            if (authHeader != _apiKey)
            {
                _logger.LogWarning("License activate: invalid API key from {IP}",
                    req.HttpContext.Connection.RemoteIpAddress);
                return new UnauthorizedResult();
            }
        }

        // --- Parse request ---
        LicenseActivateRequest? request;
        try
        {
            request = await JsonSerializer.DeserializeAsync<LicenseActivateRequest>(
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
        if (string.IsNullOrWhiteSpace(request.CompanyName))
            return new BadRequestObjectResult(new { error = "companyName is required." });

        if (string.IsNullOrWhiteSpace(request.MachineId))
            return new BadRequestObjectResult(new { error = "machineId is required." });

        // activationCode is optional (null = trial)

        _logger.LogInformation(
            "License activate request: company={Company}, machine={Machine}, hasCode={HasCode}",
            request.CompanyName, request.MachineId,
            !string.IsNullOrWhiteSpace(request.ActivationCode));

        // --- Activate ---
        try
        {
            var (response, error, statusCode) = await _licenseService.ActivateAsync(request);

            if (error != null)
                return new ObjectResult(new { error }) { StatusCode = statusCode };

            return new OkObjectResult(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "License activate error");
            return new ObjectResult(new { error = "Error interno al activar licencia." })
                { StatusCode = 500 };
        }
    }
}
