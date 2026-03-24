using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DigitalPlus.Provisioning.Services;

namespace DigitalPlus.Provisioning.Functions;

public class LsqCancelSubscriptionFunction
{
    private readonly LemonSqueezyService _lsqService;
    private readonly ILogger<LsqCancelSubscriptionFunction> _logger;
    private readonly string? _apiKey;

    public LsqCancelSubscriptionFunction(
        LemonSqueezyService lsqService,
        IConfiguration config,
        ILogger<LsqCancelSubscriptionFunction> logger)
    {
        _lsqService = lsqService;
        _logger = logger;
        _apiKey = config["ProvisioningApiKey"];
    }

    [Function("LsqCancelSubscription")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "lsq/cancel-subscription")]
        HttpRequest req)
    {
        // API Key validation
        if (!string.IsNullOrEmpty(_apiKey))
        {
            var authHeader = req.Headers["X-Api-Key"].FirstOrDefault();
            if (authHeader != _apiKey)
                return new UnauthorizedResult();
        }

        JsonElement body;
        try
        {
            body = await JsonSerializer.DeserializeAsync<JsonElement>(req.Body);
        }
        catch (JsonException)
        {
            return new BadRequestObjectResult(new { error = "Invalid JSON payload." });
        }

        // Aceptar companyId o empresaId
        string? companyId = body.TryGetProperty("companyId", out var cid) ? cid.GetString() : null;
        int? empresaId = body.TryGetProperty("empresaId", out var eid) ? eid.GetInt32() : null;

        if (string.IsNullOrEmpty(companyId) && !empresaId.HasValue)
            return new BadRequestObjectResult(new { error = "companyId or empresaId is required." });

        _logger.LogInformation("LSQ CancelSubscription: companyId={CompanyId}, empresaId={EmpresaId}",
            companyId, empresaId);

        try
        {
            var (ok, vencimiento, error) = await _lsqService.CancelSubscriptionAsync(companyId, empresaId);

            if (!ok)
                return new BadRequestObjectResult(new { error });

            return new OkObjectResult(new { ok = true, vencimiento = vencimiento?.ToString("yyyy-MM-dd") });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LSQ CancelSubscription error");
            return new ObjectResult(new { error = "Error interno al cancelar suscripcion." })
                { StatusCode = 500 };
        }
    }
}
