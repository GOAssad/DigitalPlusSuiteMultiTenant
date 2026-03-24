using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using DigitalPlus.Provisioning.Services;

namespace DigitalPlus.Provisioning.Functions;

public class LsqWebhookFunction
{
    private readonly LemonSqueezyService _lsqService;
    private readonly ILogger<LsqWebhookFunction> _logger;

    public LsqWebhookFunction(
        LemonSqueezyService lsqService,
        ILogger<LsqWebhookFunction> logger)
    {
        _lsqService = lsqService;
        _logger = logger;
    }

    [Function("LsqWebhook")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "lsq/webhook")]
        HttpRequest req)
    {
        // Siempre retornar 200 (LSQ reintenta si no recibe 200)
        try
        {
            // Leer body
            using var reader = new StreamReader(req.Body);
            var body = await reader.ReadToEndAsync();

            // Verificar firma HMAC-SHA256
            var signature = req.Headers["X-Signature"].FirstOrDefault();
            if (!_lsqService.VerifyWebhookSignature(body, signature))
            {
                _logger.LogWarning("LSQ webhook: firma invalida");
                return new OkResult(); // Retornar 200 igualmente
            }

            // Parsear JSON
            using var doc = JsonDocument.Parse(body);
            var root = doc.RootElement;

            // Obtener evento del header o del body
            var eventName = req.Headers["X-Event-Name"].FirstOrDefault();
            if (string.IsNullOrEmpty(eventName) &&
                root.TryGetProperty("meta", out var metaForEvent) &&
                metaForEvent.TryGetProperty("event_name", out var en))
            {
                eventName = en.GetString();
            }

            if (string.IsNullOrEmpty(eventName))
            {
                _logger.LogWarning("LSQ webhook: no event name found");
                return new OkResult();
            }

            var data = root.GetProperty("data");
            var meta = root.GetProperty("meta");

            await _lsqService.ProcessWebhookAsync(eventName, data, meta);
        }
        catch (Exception ex)
        {
            // Loguear pero retornar 200
            _logger.LogError(ex, "LSQ webhook processing error");
        }

        return new OkResult();
    }
}
