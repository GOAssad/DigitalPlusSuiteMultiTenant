using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DigitalPlus.Provisioning.Models;
using DigitalPlus.Provisioning.Services;

namespace DigitalPlus.Provisioning.Functions;

public class ProvisionFunction
{
    private readonly ActivationCodeService _activationService;
    private readonly DatabaseProvisioningService _dbService;
    private readonly CompanyNameSanitizer _sanitizer;
    private readonly ILogger<ProvisionFunction> _logger;
    private readonly string? _apiKey;

    public ProvisionFunction(
        ActivationCodeService activationService,
        DatabaseProvisioningService dbService,
        CompanyNameSanitizer sanitizer,
        IConfiguration config,
        ILogger<ProvisionFunction> logger)
    {
        _activationService = activationService;
        _dbService = dbService;
        _sanitizer = sanitizer;
        _logger = logger;
        _apiKey = config["ProvisioningApiKey"];
    }

    [Function("Provision")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "provision")]
        HttpRequest req)
    {
        // --- API Key validation ---
        if (!string.IsNullOrEmpty(_apiKey))
        {
            var authHeader = req.Headers["X-Api-Key"].FirstOrDefault();
            if (authHeader != _apiKey)
            {
                _logger.LogWarning("Invalid or missing API key from {IP}",
                    req.HttpContext.Connection.RemoteIpAddress);
                return new UnauthorizedResult();
            }
        }

        // --- Parse request ---
        ProvisionRequest? request;
        try
        {
            request = await JsonSerializer.DeserializeAsync<ProvisionRequest>(
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
        if (string.IsNullOrWhiteSpace(request.ActivationCode))
            return new BadRequestObjectResult(new { error = "activationCode is required." });

        if (string.IsNullOrWhiteSpace(request.CompanyName))
            return new BadRequestObjectResult(new { error = "companyName is required." });

        var installType = request.InstallType?.ToLowerInvariant();
        if (installType is not ("cloud" or "local"))
            return new BadRequestObjectResult(
                new { error = "installType must be 'cloud' or 'local'." });

        if (string.IsNullOrWhiteSpace(request.MachineId))
            return new BadRequestObjectResult(new { error = "machineId is required." });

        // --- Sanitize company name ---
        string sanitized, dbName;
        try
        {
            (sanitized, dbName) = _sanitizer.Sanitize(request.CompanyName);
        }
        catch (ArgumentException ex)
        {
            return new BadRequestObjectResult(new { error = ex.Message });
        }

        _logger.LogInformation(
            "Provision request: company={Company}, sanitized={Sanitized}, db={DbName}, type={Type}",
            request.CompanyName, sanitized, dbName, installType);

        // --- Validate activation code ---
        var (resultCode, expiresAt) = await _activationService.ValidateAndConsumeAsync(
            request.ActivationCode,
            request.CompanyName,
            dbName,
            request.MachineId,
            installType);

        switch (resultCode)
        {
            case 1:
                return new ObjectResult(new { error = "Invalid activation code." })
                    { StatusCode = 403 };
            case 2:
                return new ObjectResult(
                    new { error = "Activation code has expired.", expiresAt })
                    { StatusCode = 403 };
            case 3:
                return new ObjectResult(new { error = "Activation code has already been used." })
                    { StatusCode = 403 };
        }

        // --- Handle cloud provisioning ---
        if (installType == "cloud")
        {
            if (await _dbService.DatabaseExistsAsync(dbName))
            {
                _logger.LogWarning("Cloud DB {DbName} already exists. Returning 409.", dbName);
                return new ConflictObjectResult(new
                {
                    error = $"La base de datos '{dbName}' ya existe en el servidor cloud.",
                    dbName,
                    companySanitized = sanitized,
                    hint = "Cambie el nombre de empresa y solicite un nuevo codigo de activacion."
                });
            }

            try
            {
                await _dbService.CreateDatabaseAsync(dbName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create cloud DB {DbName}", dbName);
                return new ObjectResult(new
                {
                    error = "Error al crear la base de datos en la nube. Contacte soporte.",
                    detail = ex.Message
                })
                { StatusCode = 500 };
            }

            var connStr = _dbService.BuildClientConnectionString(dbName);

            return new OkObjectResult(new ProvisionResponse
            {
                CompanySanitized = sanitized,
                DbName = dbName,
                Mode = "cloud",
                Server = null,
                ConnectionString = connStr,
                Policy = new ProvisionPolicy
                {
                    CloudMustFailIfDbExists = true,
                    LocalMustFailIfDbExists = true
                }
            });
        }

        // --- Handle local provisioning ---
        return new OkObjectResult(new ProvisionResponse
        {
            CompanySanitized = sanitized,
            DbName = dbName,
            Mode = "local",
            Server = null,
            ConnectionString = null,
            Policy = new ProvisionPolicy
            {
                CloudMustFailIfDbExists = true,
                LocalMustFailIfDbExists = true
            }
        });
    }
}
