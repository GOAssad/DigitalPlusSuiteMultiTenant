using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using DigitalPlus.Provisioning.Services;

namespace DigitalPlus.Provisioning.Functions;

public class MpWebhookFunction
{
    private readonly MercadoPagoService _mpService;
    private readonly ILogger<MpWebhookFunction> _logger;

    public MpWebhookFunction(
        MercadoPagoService mpService,
        ILogger<MpWebhookFunction> logger)
    {
        _mpService = mpService;
        _logger = logger;
    }

    [Function("MpWebhook")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "mp/webhook")]
        HttpRequest req)
    {
        // Siempre retornar 200 (MP reintenta si no recibe 200)
        try
        {
            // Headers de firma
            var xSignature = req.Headers["x-signature"].FirstOrDefault();
            var xRequestId = req.Headers["x-request-id"].FirstOrDefault();

            // Leer body
            using var reader = new StreamReader(req.Body);
            var body = await reader.ReadToEndAsync();

            // Parsear JSON
            using var doc = JsonDocument.Parse(body);
            var root = doc.RootElement;

            // Obtener tipo y data.id
            var tipo = root.TryGetProperty("type", out var typeProp) ? typeProp.GetString() : null;
            var action = root.TryGetProperty("action", out var actionProp) ? actionProp.GetString() : null;
            string? dataId = null;

            if (root.TryGetProperty("data", out var dataProp) && dataProp.TryGetProperty("id", out var idProp))
                dataId = idProp.ToString();

            _logger.LogInformation("MP webhook received: type={Type}, action={Action}, dataId={DataId}",
                tipo, action, dataId);

            // Verificar firma
            if (!_mpService.VerifyWebhookSignature(xSignature, xRequestId, dataId))
            {
                _logger.LogWarning("MP webhook: firma invalida (type={Type}, dataId={DataId})", tipo, dataId);
                // Retornar 200 igualmente para evitar reintentos
                return new OkResult();
            }

            if (string.IsNullOrEmpty(tipo) || string.IsNullOrEmpty(dataId))
            {
                _logger.LogWarning("MP webhook: tipo o dataId faltante");
                return new OkResult();
            }

            await _mpService.ProcessWebhookAsync(tipo, dataId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MP webhook processing error");
        }

        return new OkResult();
    }
}
