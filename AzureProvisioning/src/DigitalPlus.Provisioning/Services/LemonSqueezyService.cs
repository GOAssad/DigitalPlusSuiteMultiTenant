using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DigitalPlus.Provisioning.Services;

public class LemonSqueezyService
{
    private readonly string _connectionString;
    private readonly string _apiKey;
    private readonly string _webhookSecret;
    private readonly string _storeId;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly ILogger<LemonSqueezyService> _logger;

    private const string LsqApiBase = "https://api.lemonsqueezy.com/v1";

    public LemonSqueezyService(IConfiguration config, ILogger<LemonSqueezyService> logger)
    {
        _connectionString = config["ProvisioningDb"]
            ?? throw new InvalidOperationException("ProvisioningDb not configured.");
        _apiKey = config["LemonSqueezy:ApiKey"] ?? config["LemonSqueezy__ApiKey"] ?? "";
        _webhookSecret = config["LemonSqueezy:WebhookSecret"] ?? config["LemonSqueezy__WebhookSecret"] ?? "";
        _storeId = config["LemonSqueezy:StoreId"] ?? config["LemonSqueezy__StoreId"] ?? "";
        _config = config;
        _logger = logger;

        _httpClient = new HttpClient();
        // No setear auth headers en constructor — se agregan por request para garantizar que _apiKey ya esté resuelto
    }

    /// <summary>
    /// Crea un checkout en Lemon Squeezy y retorna la URL.
    /// </summary>
    public async Task<(string? checkoutUrl, string? error)> CreateCheckoutAsync(string? companyId, int? empresaId, string variantId, string successUrl)
    {
        // Buscar email y empresaId de la empresa
        string? email;
        int resolvedEmpresaId = empresaId ?? 0;
        await using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            SqlCommand cmd;
            if (!string.IsNullOrEmpty(companyId))
            {
                cmd = new SqlCommand("SELECT Id, Email FROM Empresas WHERE CompanyId = @CompanyId", conn);
                cmd.Parameters.AddWithValue("@CompanyId", companyId);
            }
            else
            {
                cmd = new SqlCommand("SELECT Id, Email FROM Empresas WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", empresaId!);
            }

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                resolvedEmpresaId = reader.GetInt32(0);
                email = reader.IsDBNull(1) ? null : reader.GetString(1);
            }
            else
            {
                return (null, "Empresa no encontrada");
            }
        }

        // Armar checkout_data — solo incluir email si es válido
        var checkoutData = new Dictionary<string, object>
        {
            ["custom"] = new Dictionary<string, string>
            {
                ["empresaId"] = resolvedEmpresaId.ToString()
            }
        };
        if (!string.IsNullOrEmpty(email) && email.Contains('@'))
            checkoutData["email"] = email;

        var payload = new
        {
            data = new
            {
                type = "checkouts",
                attributes = new
                {
                    checkout_options = new { embed = false, media = false, button_color = "#c9a84c" },
                    checkout_data = checkoutData,
                    product_options = new { redirect_url = successUrl }
                },
                relationships = new
                {
                    store = new { data = new { type = "stores", id = _storeId } },
                    variant = new { data = new { type = "variants", id = variantId } }
                }
            }
        };

        var json = JsonSerializer.Serialize(payload);

        _logger.LogInformation("LSQ CreateCheckout: empresaId={EmpresaId}, storeId={StoreId}, email={Email}",
            resolvedEmpresaId, _storeId, email);

        var request = new HttpRequestMessage(HttpMethod.Post, $"{LsqApiBase}/checkouts");
        request.Content = new StringContent(json, Encoding.UTF8, "application/vnd.api+json");
        request.Headers.Add("Authorization", $"Bearer {_apiKey}");
        request.Headers.Add("Accept", "application/vnd.api+json");

        var response = await _httpClient.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var errorDetail = $"LSQ API {response.StatusCode}: {responseBody}";
            _logger.LogError("LSQ CreateCheckout failed: {Error}", errorDetail);
            return (null, errorDetail);
        }

        using var doc = JsonDocument.Parse(responseBody);
        var checkoutUrl = doc.RootElement
            .GetProperty("data")
            .GetProperty("attributes")
            .GetProperty("url")
            .GetString();

        _logger.LogInformation("LSQ checkout created for empresa {EmpresaId}: {Url}",
            resolvedEmpresaId, checkoutUrl);

        return (checkoutUrl, null);
    }

    /// <summary>
    /// Verifica la firma HMAC-SHA256 del webhook.
    /// </summary>
    public bool VerifyWebhookSignature(string body, string? signature)
    {
        if (string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(_webhookSecret))
            return false;

        var keyBytes = Encoding.UTF8.GetBytes(_webhookSecret);
        var bodyBytes = Encoding.UTF8.GetBytes(body);
        var hash = HMACSHA256.HashData(keyBytes, bodyBytes);
        var expectedSignature = Convert.ToHexString(hash).ToLowerInvariant();

        return string.Equals(signature, expectedSignature, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Procesa un evento de webhook de Lemon Squeezy.
    /// </summary>
    public async Task ProcessWebhookAsync(string eventName, JsonElement data, JsonElement meta)
    {
        _logger.LogInformation("LSQ webhook: {Event}", eventName);

        switch (eventName)
        {
            case "subscription_created":
                await HandleSubscriptionCreatedAsync(data, meta);
                break;
            case "subscription_payment_success":
                await HandlePaymentSuccessAsync(data);
                break;
            case "subscription_updated":
                await HandleSubscriptionUpdatedAsync(data);
                break;
            case "subscription_expired":
            case "subscription_cancelled":
                await HandleSubscriptionEndedAsync(data);
                break;
            default:
                _logger.LogInformation("LSQ webhook: evento no manejado: {Event}", eventName);
                break;
        }
    }

    private async Task HandleSubscriptionCreatedAsync(JsonElement data, JsonElement meta)
    {
        var customData = meta.GetProperty("custom_data");
        var empresaIdStr = customData.GetProperty("empresaId").GetString();
        if (!int.TryParse(empresaIdStr, out var empresaId))
        {
            _logger.LogError("LSQ subscription_created: empresaId invalido: {Id}", empresaIdStr);
            return;
        }

        var attrs = data.GetProperty("attributes");
        var subscriptionId = data.GetProperty("id").ToString();
        var customerId = attrs.GetProperty("customer_id").ToString();
        var variantId = attrs.GetProperty("variant_id").ToString();
        var renewsAt = attrs.TryGetProperty("renews_at", out var ra) ? ra.GetString() : null;

        string? updatePaymentUrl = null;
        string? customerPortalUrl = null;
        if (attrs.TryGetProperty("urls", out var urls))
        {
            updatePaymentUrl = urls.TryGetProperty("update_payment_method", out var upm)
                ? upm.GetString() : null;
            customerPortalUrl = urls.TryGetProperty("customer_portal", out var cp)
                ? cp.GetString() : null;
        }

        DateTime? planVencimiento = null;
        if (!string.IsNullOrEmpty(renewsAt) && DateTime.TryParse(renewsAt, out var rv))
            planVencimiento = rv;

        // Determinar plan desde variantId consultando PlanConfig
        var plan = ResolvePlanFromVariantId(variantId);

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        using var tx = conn.BeginTransaction();

        try
        {
            // 1. Actualizar Empresa con datos LSQ
            await using var cmdEmp = new SqlCommand(
                @"UPDATE Empresas SET
                    LsqCustomerId = @CustomerId,
                    LsqSubscriptionId = @SubscriptionId,
                    LsqVariantId = @VariantId,
                    LsqUpdatePaymentUrl = @UpdatePaymentUrl,
                    LsqCustomerPortalUrl = @CustomerPortalUrl,
                    PlanVencimiento = @PlanVencimiento,
                    PlanOrigen = 'lsq',
                    UpdatedAt = SYSUTCDATETIME()
                  WHERE Id = @EmpresaId", conn, tx);
            cmdEmp.Parameters.AddWithValue("@CustomerId", customerId);
            cmdEmp.Parameters.AddWithValue("@SubscriptionId", subscriptionId);
            cmdEmp.Parameters.AddWithValue("@VariantId", variantId);
            cmdEmp.Parameters.AddWithValue("@UpdatePaymentUrl", (object?)updatePaymentUrl ?? DBNull.Value);
            cmdEmp.Parameters.AddWithValue("@CustomerPortalUrl", (object?)customerPortalUrl ?? DBNull.Value);
            cmdEmp.Parameters.AddWithValue("@PlanVencimiento", (object?)planVencimiento ?? DBNull.Value);
            cmdEmp.Parameters.AddWithValue("@EmpresaId", empresaId);
            await cmdEmp.ExecuteNonQueryAsync();

            // 2. Actualizar Licencia con el plan nuevo
            await ActualizarLicenciaAsync(conn, tx, empresaId, plan);

            tx.Commit();
            _logger.LogInformation(
                "LSQ subscription_created: empresa {EmpresaId} -> plan {Plan}, sub {SubId}",
                empresaId, plan, subscriptionId);
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    private async Task HandlePaymentSuccessAsync(JsonElement data)
    {
        var attrs = data.GetProperty("attributes");
        var subscriptionId = data.GetProperty("id").ToString();
        var renewsAt = attrs.TryGetProperty("renews_at", out var ra) ? ra.GetString() : null;

        string? updatePaymentUrl = null;
        string? customerPortalUrl = null;
        if (attrs.TryGetProperty("urls", out var urls))
        {
            updatePaymentUrl = urls.TryGetProperty("update_payment_method", out var upm)
                ? upm.GetString() : null;
            customerPortalUrl = urls.TryGetProperty("customer_portal", out var cp)
                ? cp.GetString() : null;
        }

        DateTime? planVencimiento = null;
        if (!string.IsNullOrEmpty(renewsAt) && DateTime.TryParse(renewsAt, out var rv))
            planVencimiento = rv;

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand(
            @"UPDATE Empresas SET
                PlanVencimiento = @PlanVencimiento,
                LsqUpdatePaymentUrl = ISNULL(@UpdatePaymentUrl, LsqUpdatePaymentUrl),
                LsqCustomerPortalUrl = ISNULL(@CustomerPortalUrl, LsqCustomerPortalUrl),
                UpdatedAt = SYSUTCDATETIME()
              WHERE LsqSubscriptionId = @SubscriptionId", conn);
        cmd.Parameters.AddWithValue("@PlanVencimiento", (object?)planVencimiento ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@UpdatePaymentUrl", (object?)updatePaymentUrl ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@CustomerPortalUrl", (object?)customerPortalUrl ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@SubscriptionId", subscriptionId);
        await cmd.ExecuteNonQueryAsync();

        // Si empresa estaba suspendida, reactivar
        await using var cmdReactivar = new SqlCommand(
            @"UPDATE Empresas SET Estado = 'activa', UpdatedAt = SYSUTCDATETIME()
              WHERE LsqSubscriptionId = @SubscriptionId AND Estado = 'suspendida'", conn);
        cmdReactivar.Parameters.AddWithValue("@SubscriptionId", subscriptionId);
        var reactivadas = await cmdReactivar.ExecuteNonQueryAsync();

        if (reactivadas > 0)
            _logger.LogInformation("LSQ payment_success: empresa reactivada, sub {SubId}", subscriptionId);

        _logger.LogInformation("LSQ payment_success: sub {SubId} renovada", subscriptionId);
    }

    private async Task HandleSubscriptionUpdatedAsync(JsonElement data)
    {
        var attrs = data.GetProperty("attributes");
        var subscriptionId = data.GetProperty("id").ToString();
        var newVariantId = attrs.GetProperty("variant_id").ToString();
        var renewsAt = attrs.TryGetProperty("renews_at", out var ra) ? ra.GetString() : null;

        string? updatePaymentUrl = null;
        string? customerPortalUrl = null;
        if (attrs.TryGetProperty("urls", out var urls))
        {
            updatePaymentUrl = urls.TryGetProperty("update_payment_method", out var upm)
                ? upm.GetString() : null;
            customerPortalUrl = urls.TryGetProperty("customer_portal", out var cp)
                ? cp.GetString() : null;
        }

        DateTime? planVencimiento = null;
        if (!string.IsNullOrEmpty(renewsAt) && DateTime.TryParse(renewsAt, out var rv))
            planVencimiento = rv;

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        // Leer variante actual
        await using var cmdRead = new SqlCommand(
            "SELECT Id, LsqVariantId FROM Empresas WHERE LsqSubscriptionId = @SubId", conn);
        cmdRead.Parameters.AddWithValue("@SubId", subscriptionId);
        await using var reader = await cmdRead.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            _logger.LogWarning("LSQ subscription_updated: empresa no encontrada para sub {SubId}", subscriptionId);
            return;
        }

        var empresaId = reader.GetInt32(0);
        var oldVariantId = reader.IsDBNull(1) ? "" : reader.GetString(1);
        reader.Close();

        // Actualizar campos
        await using var cmdUpdate = new SqlCommand(
            @"UPDATE Empresas SET
                LsqVariantId = @VariantId,
                PlanVencimiento = @PlanVencimiento,
                LsqUpdatePaymentUrl = ISNULL(@UpdatePaymentUrl, LsqUpdatePaymentUrl),
                LsqCustomerPortalUrl = ISNULL(@CustomerPortalUrl, LsqCustomerPortalUrl),
                UpdatedAt = SYSUTCDATETIME()
              WHERE Id = @EmpresaId", conn);
        cmdUpdate.Parameters.AddWithValue("@VariantId", newVariantId);
        cmdUpdate.Parameters.AddWithValue("@PlanVencimiento", (object?)planVencimiento ?? DBNull.Value);
        cmdUpdate.Parameters.AddWithValue("@UpdatePaymentUrl", (object?)updatePaymentUrl ?? DBNull.Value);
        cmdUpdate.Parameters.AddWithValue("@CustomerPortalUrl", (object?)customerPortalUrl ?? DBNull.Value);
        cmdUpdate.Parameters.AddWithValue("@EmpresaId", empresaId);
        await cmdUpdate.ExecuteNonQueryAsync();

        // Si cambió la variante, actualizar licencia
        if (newVariantId != oldVariantId)
        {
            var plan = ResolvePlanFromVariantId(newVariantId);
            using var tx = conn.BeginTransaction();
            try
            {
                await ActualizarLicenciaAsync(conn, tx, empresaId, plan);
                tx.Commit();
                _logger.LogInformation(
                    "LSQ subscription_updated: empresa {EmpresaId} cambió a plan {Plan}",
                    empresaId, plan);
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }
    }

    private async Task HandleSubscriptionEndedAsync(JsonElement data)
    {
        var subscriptionId = data.GetProperty("id").ToString();

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        // Obtener empresaId
        await using var cmdRead = new SqlCommand(
            "SELECT Id FROM Empresas WHERE LsqSubscriptionId = @SubId", conn);
        cmdRead.Parameters.AddWithValue("@SubId", subscriptionId);
        var empresaIdObj = await cmdRead.ExecuteScalarAsync();

        if (empresaIdObj == null)
        {
            _logger.LogWarning("LSQ subscription ended: empresa no encontrada para sub {SubId}", subscriptionId);
            return;
        }
        var empresaId = (int)empresaIdObj;

        using var tx = conn.BeginTransaction();
        try
        {
            // Limpiar datos LSQ y degradar a manual
            await using var cmdClean = new SqlCommand(
                @"UPDATE Empresas SET
                    LsqSubscriptionId = NULL,
                    LsqVariantId = NULL,
                    LsqUpdatePaymentUrl = NULL,
                    LsqCustomerPortalUrl = NULL,
                    PlanVencimiento = NULL,
                    PlanOrigen = 'manual',
                    UpdatedAt = SYSUTCDATETIME()
                  WHERE Id = @EmpresaId", conn, tx);
            cmdClean.Parameters.AddWithValue("@EmpresaId", empresaId);
            await cmdClean.ExecuteNonQueryAsync();

            // Degradar a free
            await ActualizarLicenciaAsync(conn, tx, empresaId, "free");

            tx.Commit();
            _logger.LogInformation("LSQ subscription ended: empresa {EmpresaId} degradada a free", empresaId);
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    /// <summary>
    /// Actualiza la licencia de una empresa al plan especificado usando PlanConfig.
    /// </summary>
    private async Task ActualizarLicenciaAsync(SqlConnection conn, SqlTransaction tx, int empresaId, string plan)
    {
        // 1. Obtener CompanyId
        await using var cmdCompany = new SqlCommand(
            "SELECT CompanyId FROM Empresas WHERE Id = @Id", conn, tx);
        cmdCompany.Parameters.AddWithValue("@Id", empresaId);
        var companyId = await cmdCompany.ExecuteScalarAsync() as string;

        if (string.IsNullOrEmpty(companyId))
        {
            _logger.LogError("ActualizarLicencia: empresa {EmpresaId} no encontrada", empresaId);
            return;
        }

        // 2. Leer limites de PlanConfig
        await using var cmdConfig = new SqlCommand(
            "SELECT Parametro, Valor FROM PlanConfig WHERE [Plan] = @Plan", conn, tx);
        cmdConfig.Parameters.AddWithValue("@Plan", plan);

        var valores = new Dictionary<string, decimal>();
        await using var reader = await cmdConfig.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            valores[reader.GetString(0)] = reader.GetDecimal(1);
        reader.Close();

        var maxLegajos = (int)valores.GetValueOrDefault("MaxLegajos", 5);
        var maxSucursales = (int)valores.GetValueOrDefault("MaxSucursales", 1);
        var maxFichadasMes = (int)valores.GetValueOrDefault("MaxFichadasRolling30d", 200);
        var duracionDias = (int)valores.GetValueOrDefault("DuracionDias", 0);

        DateTime? expiresAt = duracionDias > 0 ? DateTime.UtcNow.AddDays(duracionDias) : null;

        // 3. Actualizar licencia
        await using var cmdLic = new SqlCommand(
            @"UPDATE Licencias SET
                [Plan] = @Plan, MaxLegajos = @MaxLegajos,
                MaxSucursales = @MaxSucursales, MaxFichadasMes = @MaxFichadasMes,
                ExpiresAt = @ExpiresAt, LicenseType = 'active',
                UpdatedAt = SYSUTCDATETIME()
              WHERE CompanyId = @CompanyId", conn, tx);
        cmdLic.Parameters.AddWithValue("@Plan", plan);
        cmdLic.Parameters.AddWithValue("@MaxLegajos", maxLegajos);
        cmdLic.Parameters.AddWithValue("@MaxSucursales", maxSucursales);
        cmdLic.Parameters.AddWithValue("@MaxFichadasMes", maxFichadasMes);
        cmdLic.Parameters.AddWithValue("@ExpiresAt", (object?)expiresAt ?? DBNull.Value);
        cmdLic.Parameters.AddWithValue("@CompanyId", companyId);
        await cmdLic.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Cancela una suscripción activa en Lemon Squeezy (al final del período actual).
    /// </summary>
    public async Task<(bool ok, DateTime? vencimiento, string? error)> CancelSubscriptionAsync(string? companyId, int? empresaId)
    {
        // Buscar empresa y su LsqSubscriptionId
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        SqlCommand cmd;
        if (!string.IsNullOrEmpty(companyId))
        {
            cmd = new SqlCommand("SELECT LsqSubscriptionId, PlanVencimiento FROM Empresas WHERE CompanyId = @CompanyId", conn);
            cmd.Parameters.AddWithValue("@CompanyId", companyId);
        }
        else
        {
            cmd = new SqlCommand("SELECT LsqSubscriptionId, PlanVencimiento FROM Empresas WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", empresaId!);
        }

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return (false, null, "Empresa no encontrada.");

        var subscriptionId = reader.IsDBNull(0) ? null : reader.GetString(0);
        var vencimiento = reader.IsDBNull(1) ? (DateTime?)null : reader.GetDateTime(1);
        reader.Close();

        if (string.IsNullOrEmpty(subscriptionId))
            return (false, null, "La empresa no tiene una suscripcion activa.");

        // PATCH a Lemon Squeezy para cancelar al final del período
        var payload = new
        {
            data = new
            {
                type = "subscriptions",
                id = subscriptionId,
                attributes = new { cancelled = true }
            }
        };

        var json = JsonSerializer.Serialize(payload);
        var request = new HttpRequestMessage(HttpMethod.Patch, $"{LsqApiBase}/subscriptions/{subscriptionId}");
        request.Content = new StringContent(json, Encoding.UTF8, "application/vnd.api+json");
        request.Headers.Add("Authorization", $"Bearer {_apiKey}");
        request.Headers.Add("Accept", "application/vnd.api+json");

        var response = await _httpClient.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("LSQ CancelSubscription failed: {Status} {Body}", response.StatusCode, responseBody);
            return (false, null, $"Error de Lemon Squeezy: {response.StatusCode}");
        }

        _logger.LogInformation("LSQ subscription {SubId} cancelled, active until {Vencimiento}",
            subscriptionId, vencimiento);

        return (true, vencimiento, null);
    }

    /// <summary>
    /// Resuelve el nombre del plan basado en el variantId.
    /// Mapeo configurado en variables de entorno LemonSqueezy:VariantMap:{variantId}
    /// </summary>
    private string ResolvePlanFromVariantId(string variantId)
    {
        // Buscar en config: LemonSqueezy:VariantMap:{variantId} = "pro"
        var plan = _config[$"LemonSqueezy:VariantMap:{variantId}"];
        if (!string.IsNullOrEmpty(plan))
        {
            _logger.LogInformation("LSQ variant {VariantId} -> plan {Plan} (from config)", variantId, plan);
            return plan;
        }

        _logger.LogWarning("LSQ variant {VariantId} not mapped, defaulting to basic", variantId);
        return "basic";
    }
}
