using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DigitalPlus.Provisioning.Services;

namespace DigitalPlus.Provisioning.Functions;

public class LsqCreateCheckoutFunction
{
    private readonly LemonSqueezyService _lsqService;
    private readonly ILogger<LsqCreateCheckoutFunction> _logger;
    private readonly string? _apiKey;

    public LsqCreateCheckoutFunction(
        LemonSqueezyService lsqService,
        IConfiguration config,
        ILogger<LsqCreateCheckoutFunction> logger)
    {
        _lsqService = lsqService;
        _logger = logger;
        _apiKey = config["ProvisioningApiKey"];
    }

    [Function("LsqCreateCheckout")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "lsq/create-checkout")]
        HttpRequest req)
    {
        // API Key validation
        if (!string.IsNullOrEmpty(_apiKey))
        {
            var authHeader = req.Headers["X-Api-Key"].FirstOrDefault();
            if (authHeader != _apiKey)
            {
                _logger.LogWarning("LSQ CreateCheckout: invalid API key from {IP}",
                    req.HttpContext.Connection.RemoteIpAddress);
                return new UnauthorizedResult();
            }
        }

        // Parse request
        JsonElement body;
        try
        {
            body = await JsonSerializer.DeserializeAsync<JsonElement>(req.Body);
        }
        catch (JsonException)
        {
            return new BadRequestObjectResult(new { error = "Invalid JSON payload." });
        }

        if (!body.TryGetProperty("variantId", out var variantIdProp))
            return new BadRequestObjectResult(new { error = "variantId is required." });

        // Aceptar companyId (string) o empresaId (int) para compatibilidad
        string? companyId = body.TryGetProperty("companyId", out var cid) ? cid.GetString() : null;
        int? empresaId = body.TryGetProperty("empresaId", out var eid) ? eid.GetInt32() : null;

        if (string.IsNullOrEmpty(companyId) && !empresaId.HasValue)
            return new BadRequestObjectResult(new { error = "companyId or empresaId is required." });

        var variantId = variantIdProp.GetString() ?? "";
        var successUrl = body.TryGetProperty("successUrl", out var su) ? su.GetString() ?? "" : "";

        _logger.LogInformation("LSQ CreateCheckout: companyId={CompanyId}, empresaId={EmpresaId}, variant={Variant}",
            companyId, empresaId, variantId);

        try
        {
            var (checkoutUrl, lsqError) = await _lsqService.CreateCheckoutAsync(companyId, empresaId, variantId, successUrl);

            if (string.IsNullOrEmpty(checkoutUrl))
                return new ObjectResult(new { error = lsqError ?? "Error al crear checkout en Lemon Squeezy." })
                    { StatusCode = 502 };

            return new OkObjectResult(new { checkoutUrl });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LSQ CreateCheckout error");
            return new ObjectResult(new { error = "Error interno al crear checkout." })
                { StatusCode = 500 };
        }
    }
}
