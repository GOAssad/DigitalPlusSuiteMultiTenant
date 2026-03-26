using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DigitalPlus.Provisioning.Services;

namespace DigitalPlus.Provisioning.Functions;

/// <summary>
/// Checkout Pro (pago único) para Enterprise manual.
/// IntegraIA genera el link desde Portal Licencias y lo envía al cliente.
/// </summary>
public class MpCreateCheckoutFunction
{
    private readonly MercadoPagoService _mpService;
    private readonly ILogger<MpCreateCheckoutFunction> _logger;
    private readonly string? _apiKey;

    public MpCreateCheckoutFunction(
        MercadoPagoService mpService,
        IConfiguration config,
        ILogger<MpCreateCheckoutFunction> logger)
    {
        _mpService = mpService;
        _logger = logger;
        _apiKey = config["ProvisioningApiKey"];
    }

    [Function("MpCreateCheckout")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "mp/create-checkout")]
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

        if (!body.TryGetProperty("empresaId", out var eidProp))
            return new BadRequestObjectResult(new { error = "empresaId is required." });

        if (!body.TryGetProperty("monto", out var montoProp))
            return new BadRequestObjectResult(new { error = "monto is required." });

        var empresaId = eidProp.GetInt32();
        var monto = montoProp.GetDecimal();
        var descripcion = body.TryGetProperty("descripcion", out var desc)
            ? desc.GetString() ?? "DigitalPlus Enterprise"
            : "DigitalPlus Enterprise";
        var successUrl = body.TryGetProperty("successUrl", out var su) ? su.GetString() ?? "" : "";

        _logger.LogInformation("MP CreateCheckout: empresaId={EmpresaId}, monto={Monto}",
            empresaId, monto);

        try
        {
            var (initPoint, error) = await _mpService.CreateCheckoutAsync(empresaId, monto, descripcion, successUrl);

            if (string.IsNullOrEmpty(initPoint))
                return new ObjectResult(new { error = error ?? "Error al crear checkout en MercadoPago." })
                    { StatusCode = 502 };

            return new OkObjectResult(new { checkoutUrl = initPoint });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MP CreateCheckout error");
            return new ObjectResult(new { error = "Error interno al crear checkout." })
                { StatusCode = 500 };
        }
    }
}
