using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace DigitalPlusMultiTenant.Infrastructure.Services;

public class MercadoPagoService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<MercadoPagoService> _logger;

    public MercadoPagoService(
        IHttpClientFactory httpClientFactory,
        ILogger<MercadoPagoService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <summary>
    /// Crea un Checkout Pro para contratar un plan via Azure Function.
    /// Retorna la URL de checkout (init_point) o null si falla.
    /// </summary>
    public async Task<string?> CreateCheckoutAsync(string companyId, string plan, bool anual, string successUrl)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("AzureFunctions");

            var request = new
            {
                companyId,
                plan,
                anual,
                successUrl
            };

            var response = await client.PostAsJsonAsync("mp/create-subscription", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError("MP CreateCheckout failed: {Status} {Error}",
                    response.StatusCode, error);
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<CheckoutResponse>();
            return result?.checkoutUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating MP checkout for company {CompanyId}", companyId);
            return null;
        }
    }

    /// <summary>
    /// Cancela el plan MercadoPago de una empresa (estado local).
    /// </summary>
    public async Task<CancelResult> CancelSubscriptionAsync(string companyId)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("AzureFunctions");
            var response = await client.PostAsJsonAsync("mp/cancel-subscription", new { companyId });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError("MP CancelSubscription failed: {Status} {Error}", response.StatusCode, error);
                return new CancelResult { Ok = false, Error = "Error al cancelar la suscripción." };
            }

            var result = await response.Content.ReadFromJsonAsync<CancelResult>();
            return result ?? new CancelResult { Ok = false, Error = "Respuesta vacía." };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling MP subscription for {CompanyId}", companyId);
            return new CancelResult { Ok = false, Error = ex.Message };
        }
    }

    private class CheckoutResponse
    {
        public string? checkoutUrl { get; set; }
    }

    public class CancelResult
    {
        public bool Ok { get; set; }
        public string? Error { get; set; }
    }
}
