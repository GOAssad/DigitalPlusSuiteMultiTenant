using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DigitalPlusMultiTenant.Infrastructure.Services;

public class LemonSqueezyService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<LemonSqueezyService> _logger;

    public LemonSqueezyService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<LemonSqueezyService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene el variant ID de Lemon Squeezy para un plan y período.
    /// </summary>
    public string? GetVariantId(string plan, bool anual)
    {
        var key = $"{char.ToUpper(plan[0])}{plan[1..].ToLower()}{(anual ? "Anual" : "Mensual")}";
        return _configuration[$"LemonSqueezy:Variants:{key}"];
    }

    /// <summary>
    /// Crea un checkout en Lemon Squeezy a través de la Azure Function.
    /// Retorna la URL de checkout o null si falla.
    /// </summary>
    public async Task<string?> CreateCheckoutAsync(string companyId, string variantId, string successUrl)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("AzureFunctions");

            var request = new
            {
                companyId,
                variantId,
                successUrl
            };

            var response = await client.PostAsJsonAsync("lsq/create-checkout", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError("CreateCheckout failed: {Status} {Error}",
                    response.StatusCode, error);
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<CheckoutResponse>();
            return result?.checkoutUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating LSQ checkout for company {CompanyId}", companyId);
            return null;
        }
    }

    private class CheckoutResponse
    {
        public string? checkoutUrl { get; set; }
    }
}
