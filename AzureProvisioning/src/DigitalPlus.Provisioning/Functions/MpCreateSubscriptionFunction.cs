using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DigitalPlus.Provisioning.Services;

namespace DigitalPlus.Provisioning.Functions;

/// <summary>
/// Crea un Checkout Pro para contratar un plan (Basic, Pro, Enterprise).
/// Ruta mantenida como /mp/create-subscription por backward compat con Portal MT.
/// </summary>
public class MpCreateSubscriptionFunction
{
    private readonly MercadoPagoService _mpService;
    private readonly ILogger<MpCreateSubscriptionFunction> _logger;
    private readonly string? _apiKey;

    public MpCreateSubscriptionFunction(
        MercadoPagoService mpService,
        IConfiguration config,
        ILogger<MpCreateSubscriptionFunction> logger)
    {
        _mpService = mpService;
        _logger = logger;
        _apiKey = config["ProvisioningApiKey"];
    }

    [Function("MpCreateSubscription")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "mp/create-subscription")]
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
        int? empresaId = body.TryGetProperty("empresaId", out var eidProp) ? eidProp.GetInt32() : null;

        if (string.IsNullOrEmpty(companyId) && !empresaId.HasValue)
            return new BadRequestObjectResult(new { error = "companyId or empresaId is required." });

        if (!body.TryGetProperty("plan", out var planProp))
            return new BadRequestObjectResult(new { error = "plan is required." });

        var plan = planProp.GetString() ?? "basic";
        var anual = body.TryGetProperty("anual", out var anualProp) && anualProp.GetBoolean();
        var successUrl = body.TryGetProperty("successUrl", out var su) ? su.GetString() ?? "" : "";

        _logger.LogInformation("MP CreatePlanCheckout: companyId={CompanyId}, empresaId={EmpresaId}, plan={Plan}, anual={Anual}",
            companyId, empresaId, plan, anual);

        try
        {
            var (initPoint, error) = await _mpService.CreatePlanCheckoutAsync(companyId, empresaId, plan, anual, successUrl);

            if (string.IsNullOrEmpty(initPoint))
                return new ObjectResult(new { error = error ?? "Error al crear checkout en MercadoPago." })
                    { StatusCode = 502 };

            return new OkObjectResult(new { checkoutUrl = initPoint });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MP CreatePlanCheckout error");
            return new ObjectResult(new { error = "Error interno al crear checkout." })
                { StatusCode = 500 };
        }
    }
}
