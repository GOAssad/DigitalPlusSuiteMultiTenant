using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DigitalPlus.Provisioning.Services;

namespace DigitalPlus.Provisioning.Functions;

public class MpCancelSubscriptionFunction
{
    private readonly MercadoPagoService _mpService;
    private readonly ILogger<MpCancelSubscriptionFunction> _logger;
    private readonly string? _apiKey;

    public MpCancelSubscriptionFunction(
        MercadoPagoService mpService,
        IConfiguration config,
        ILogger<MpCancelSubscriptionFunction> logger)
    {
        _mpService = mpService;
        _logger = logger;
        _apiKey = config["ProvisioningApiKey"];
    }

    [Function("MpCancelSubscription")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "mp/cancel-subscription")]
        HttpRequest req)
    {
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

        string? companyId = body.TryGetProperty("companyId", out var cid) ? cid.GetString() : null;
        int? empresaId = body.TryGetProperty("empresaId", out var eid) ? eid.GetInt32() : null;

        if (string.IsNullOrEmpty(companyId) && !empresaId.HasValue)
            return new BadRequestObjectResult(new { error = "companyId or empresaId is required." });

        _logger.LogInformation("MP CancelSubscription: companyId={CompanyId}, empresaId={EmpresaId}",
            companyId, empresaId);

        try
        {
            var (ok, error) = await _mpService.CancelSubscriptionAsync(companyId, empresaId);

            if (!ok)
                return new BadRequestObjectResult(new { error });

            return new OkObjectResult(new { ok = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MP CancelSubscription error");
            return new ObjectResult(new { error = "Error interno al cancelar suscripción." })
                { StatusCode = 500 };
        }
    }
}
