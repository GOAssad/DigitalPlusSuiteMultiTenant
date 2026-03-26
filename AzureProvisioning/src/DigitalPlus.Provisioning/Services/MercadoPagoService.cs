using System.Security.Cryptography;
using System.Text;
using MercadoPago.Client;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DigitalPlus.Provisioning.Services;

public class MercadoPagoService
{
    private readonly string _connectionString;
    private readonly string _accessToken;
    private readonly string _webhookSecret;
    private readonly ILogger<MercadoPagoService> _logger;
    private readonly LicenseUpdateService _licenseUpdate;

    private const string WebhookUrl = "https://digitalplus-provision.azurewebsites.net/api/mp/webhook";

    public MercadoPagoService(
        IConfiguration config,
        ILogger<MercadoPagoService> logger,
        LicenseUpdateService licenseUpdate)
    {
        _connectionString = config["ProvisioningDb"]
            ?? throw new InvalidOperationException("ProvisioningDb not configured.");
        _accessToken = config["MercadoPago:AccessToken"] ?? config["MercadoPago__AccessToken"] ?? "";
        _webhookSecret = config["MercadoPago:WebhookSecret"] ?? config["MercadoPago__WebhookSecret"] ?? "";
        _logger = logger;
        _licenseUpdate = licenseUpdate;
    }

    private RequestOptions GetRequestOptions() => new() { AccessToken = _accessToken };

    // ─── Checkout Pro (pago único para cualquier plan) ───

    /// <summary>
    /// Crea un Checkout Pro para contratar un plan (Basic, Pro, Enterprise).
    /// Precio en USD se convierte a ARS via tipo de cambio vigente.
    /// Retorna init_point URL para redirect.
    /// </summary>
    public async Task<(string? initPoint, string? error)> CreatePlanCheckoutAsync(
        string? companyId, int? empresaIdParam, string plan, bool anual, string successUrl)
    {
        // Buscar email y empresaId
        string? email;
        int empresaId;
        await using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            SqlCommand cmd;
            if (!string.IsNullOrEmpty(companyId))
            {
                cmd = new SqlCommand("SELECT Id, Email FROM Empresas WHERE CompanyId = @Key", conn);
                cmd.Parameters.AddWithValue("@Key", companyId);
            }
            else
            {
                cmd = new SqlCommand("SELECT Id, Email FROM Empresas WHERE Id = @Key", conn);
                cmd.Parameters.AddWithValue("@Key", empresaIdParam!);
            }

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                empresaId = reader.GetInt32(0);
                email = reader.IsDBNull(1) ? null : reader.GetString(1);
            }
            else
            {
                return (null, "Empresa no encontrada.");
            }
        }

        // Leer precio USD de PlanConfig
        decimal precioUsd;
        await using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            var parametro = anual ? "ImporteAnual" : "ImporteMensual";
            await using var cmd = new SqlCommand(
                "SELECT Valor FROM PlanConfig WHERE [Plan] = @Plan AND Parametro = @Parametro", conn);
            cmd.Parameters.AddWithValue("@Plan", plan);
            cmd.Parameters.AddWithValue("@Parametro", parametro);
            var result = await cmd.ExecuteScalarAsync();
            if (result == null || result == DBNull.Value)
                return (null, $"Precio no configurado para plan {plan} ({parametro}).");
            precioUsd = Convert.ToDecimal(result);
        }

        if (precioUsd <= 0)
            return (null, $"Precio invalido para plan {plan}.");

        // Obtener tipo de cambio vigente USD -> ARS
        decimal tipoCambio;
        await using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(
                @"SELECT TOP 1 tc.Valor
                  FROM TiposCambio tc
                  JOIN Monedas mo ON tc.MonedaOrigenId = mo.Id AND mo.Codigo = 'USD'
                  JOIN Monedas md ON tc.MonedaDestinoId = md.Id AND md.Codigo = 'ARS'
                  ORDER BY tc.VigenteDesde DESC", conn);
            var tcResult = await cmd.ExecuteScalarAsync();
            if (tcResult == null || tcResult == DBNull.Value)
                return (null, "Tipo de cambio USD/ARS no configurado. Cargar en Portal Licencias > Atributos > Monedas.");
            tipoCambio = Convert.ToDecimal(tcResult);
        }

        var precioArs = Math.Round(precioUsd * tipoCambio, 2);
        var periodo = anual ? "annual" : "monthly";
        var titulo = $"DigitalPlus - Plan {char.ToUpper(plan[0])}{plan[1..]} ({(anual ? "anual" : "mensual")})";

        _logger.LogInformation("MP checkout: plan {Plan} {Periodo}, USD {PrecioUsd} x TC {TipoCambio} = ARS {PrecioArs}",
            plan, periodo, precioUsd, tipoCambio, precioArs);

        try
        {
            var client = new PreferenceClient();
            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                    new()
                    {
                        Title = titulo,
                        Quantity = 1,
                        CurrencyId = "ARS",
                        UnitPrice = precioArs
                    }
                },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = successUrl,
                    Failure = successUrl.Replace("success=true", "canceled=true"),
                    Pending = successUrl
                },
                ExternalReference = $"emp_{empresaId}_plan_{plan}_{periodo}",
                NotificationUrl = WebhookUrl,
                AutoReturn = "approved",
                BinaryMode = true
            };

            if (!string.IsNullOrEmpty(email) && email.Contains('@'))
            {
                request.Payer = new PreferencePayerRequest { Email = email };
            }

            var preference = await client.CreateAsync(request, GetRequestOptions());
            var initPoint = preference?.InitPoint;

            if (string.IsNullOrEmpty(initPoint))
                return (null, "MercadoPago no retornó URL de checkout.");

            // Marcar empresa como pendiente de pago
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmdUpdate = new SqlCommand(
                @"UPDATE Empresas SET
                    MpSubscriptionId = @PrefId,
                    MpStatus = 'pending',
                    UpdatedAt = SYSUTCDATETIME()
                  WHERE Id = @EmpresaId", conn);
            cmdUpdate.Parameters.AddWithValue("@PrefId", preference!.Id);
            cmdUpdate.Parameters.AddWithValue("@EmpresaId", empresaId);
            await cmdUpdate.ExecuteNonQueryAsync();

            _logger.LogInformation(
                "MP checkout created: empresa {EmpresaId}, plan {Plan}, preference {PrefId}",
                empresaId, plan, preference.Id);

            await _licenseUpdate.LogEventAsync(empresaId, "mp_checkout_created", "MercadoPago",
                $"Checkout creado: plan={plan}, periodo={periodo}, ARS {precioArs} (USD {precioUsd} x TC {tipoCambio})");

            return (initPoint, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating MP checkout for empresa {EmpresaId}", empresaId);
            return (null, $"Error de MercadoPago: {ex.Message}");
        }
    }

    // ─── Checkout Pro (pago único - Enterprise manual con monto custom) ───

    /// <summary>
    /// Crea un Checkout Pro con monto personalizado (Enterprise manual).
    /// IntegraIA genera el link y lo envía al cliente.
    /// </summary>
    public async Task<(string? initPoint, string? error)> CreateCheckoutAsync(
        int empresaId, decimal monto, string descripcion, string successUrl)
    {
        string? email;
        await using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(
                "SELECT Email FROM Empresas WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", empresaId);
            email = await cmd.ExecuteScalarAsync() as string;
        }

        try
        {
            var client = new PreferenceClient();
            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                    new()
                    {
                        Title = descripcion,
                        Quantity = 1,
                        CurrencyId = "USD",
                        UnitPrice = monto
                    }
                },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = successUrl,
                    Failure = successUrl,
                    Pending = successUrl
                },
                ExternalReference = $"emp_{empresaId}_enterprise",
                NotificationUrl = WebhookUrl,
                AutoReturn = "approved",
                BinaryMode = true
            };

            if (!string.IsNullOrEmpty(email) && email.Contains('@'))
            {
                request.Payer = new PreferencePayerRequest { Email = email };
            }

            var preference = await client.CreateAsync(request, GetRequestOptions());

            if (preference == null || string.IsNullOrEmpty(preference.InitPoint))
                return (null, "MercadoPago no retornó URL de checkout.");

            _logger.LogInformation(
                "MP checkout created: empresa {EmpresaId}, monto {Monto} USD, preference {PrefId}",
                empresaId, monto, preference.Id);

            return (preference.InitPoint, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating MP checkout for empresa {EmpresaId}", empresaId);
            return (null, $"Error de MercadoPago: {ex.Message}");
        }
    }

    // ─── Webhook ───

    /// <summary>
    /// Verifica la firma HMAC-SHA256 del webhook de MercadoPago.
    /// </summary>
    public bool VerifyWebhookSignature(string? xSignature, string? xRequestId, string? dataId)
    {
        if (string.IsNullOrEmpty(xSignature) || string.IsNullOrEmpty(_webhookSecret))
            return false;

        string? ts = null, v1 = null;
        foreach (var part in xSignature.Split(','))
        {
            var kv = part.Split('=', 2);
            if (kv.Length == 2)
            {
                if (kv[0].Trim() == "ts") ts = kv[1].Trim();
                else if (kv[0].Trim() == "v1") v1 = kv[1].Trim();
            }
        }

        if (string.IsNullOrEmpty(ts) || string.IsNullOrEmpty(v1))
            return false;

        var manifest = $"id:{dataId};request-id:{xRequestId};ts:{ts};";
        var keyBytes = Encoding.UTF8.GetBytes(_webhookSecret);
        var dataBytes = Encoding.UTF8.GetBytes(manifest);
        var hash = HMACSHA256.HashData(keyBytes, dataBytes);
        var expectedHash = Convert.ToHexString(hash).ToLowerInvariant();

        return string.Equals(v1, expectedHash, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Procesa un webhook de MercadoPago (Checkout Pro solo envía "payment").
    /// </summary>
    public async Task ProcessWebhookAsync(string tipo, string dataId)
    {
        _logger.LogInformation("MP webhook: type={Type}, dataId={DataId}", tipo, dataId);

        if (tipo == "payment")
        {
            await HandlePaymentWebhookAsync(dataId);
        }
        else
        {
            _logger.LogInformation("MP webhook: tipo no manejado: {Type}", tipo);
        }
    }

    private async Task HandlePaymentWebhookAsync(string paymentIdStr)
    {
        if (!long.TryParse(paymentIdStr, out var paymentId))
        {
            _logger.LogWarning("MP payment webhook: ID invalido: {Id}", paymentIdStr);
            return;
        }

        var client = new PaymentClient();
        var payment = await client.GetAsync(paymentId, GetRequestOptions());

        if (payment == null)
        {
            _logger.LogWarning("MP payment {Id} not found", paymentId);
            return;
        }

        _logger.LogInformation("MP payment {Id}: status={Status}, ref={Ref}, amount={Amount} {Currency}",
            paymentId, payment.Status, payment.ExternalReference, payment.TransactionAmount, payment.CurrencyId);

        if (payment.Status != "approved")
            return;

        if (string.IsNullOrEmpty(payment.ExternalReference))
            return;

        if (!TryParseExternalReference(payment.ExternalReference, out var empresaId, out var plan, out var periodo))
            return;

        // Calcular vencimiento según periodo
        DateTime? planVencimiento = periodo switch
        {
            "monthly" => DateTime.UtcNow.AddDays(30),
            "annual" => DateTime.UtcNow.AddDays(365),
            _ => null // enterprise manual: sin vencimiento automático
        };

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        using var tx = conn.BeginTransaction();

        try
        {
            await using var cmdEmp = new SqlCommand(
                @"UPDATE Empresas SET
                    MpCustomerId = @CustomerId,
                    MpStatus = 'active',
                    PlanOrigen = 'mp',
                    PlanVencimiento = @PlanVencimiento,
                    UpdatedAt = SYSUTCDATETIME()
                  WHERE Id = @EmpresaId", conn, tx);
            cmdEmp.Parameters.AddWithValue("@CustomerId", payment.Payer?.Id?.ToString() ?? "");
            cmdEmp.Parameters.AddWithValue("@PlanVencimiento", (object?)planVencimiento ?? DBNull.Value);
            cmdEmp.Parameters.AddWithValue("@EmpresaId", empresaId);
            await cmdEmp.ExecuteNonQueryAsync();

            if (!string.IsNullOrEmpty(plan))
                await _licenseUpdate.ActualizarLicenciaAsync(conn, tx, empresaId, plan);

            tx.Commit();

            var vencStr = planVencimiento?.ToString("yyyy-MM-dd") ?? "sin vencimiento";
            _logger.LogInformation("MP payment approved: empresa {EmpresaId}, plan {Plan}, periodo {Periodo}, vence {Vencimiento}",
                empresaId, plan, periodo ?? "manual", vencStr);

            await _licenseUpdate.LogEventAsync(empresaId, "mp_payment_approved", "MercadoPago",
                $"Pago aprobado: plan={plan}, periodo={periodo ?? "manual"}, monto={payment.TransactionAmount} {payment.CurrencyId}, vence={vencStr}");

            // Notificar en dashboard MT
            await _licenseUpdate.NotificarCambioPlanMTAsync(empresaId, plan ?? "free", plan ?? "free", "MercadoPago");
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    // ─── Cancelación ───

    /// <summary>
    /// Cancela la suscripción MercadoPago de una empresa.
    /// Con Checkout Pro no hay suscripción en MP — solo se actualiza el estado local.
    /// La licencia expira naturalmente en PlanVencimiento.
    /// </summary>
    public async Task<(bool ok, string? error)> CancelSubscriptionAsync(string? companyId, int? empresaId)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        string query = !string.IsNullOrEmpty(companyId)
            ? "SELECT Id, MpStatus FROM Empresas WHERE CompanyId = @Key"
            : "SELECT Id, MpStatus FROM Empresas WHERE Id = @Key";

        await using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@Key", !string.IsNullOrEmpty(companyId) ? companyId : (object)empresaId!);

        int empId;
        string? mpStatus;
        await using (var reader = await cmd.ExecuteReaderAsync())
        {
            if (!await reader.ReadAsync())
                return (false, "Empresa no encontrada.");
            empId = reader.GetInt32(0);
            mpStatus = reader.IsDBNull(1) ? null : reader.GetString(1);
        }

        if (string.IsNullOrEmpty(mpStatus) || mpStatus == "cancelled")
            return (false, "La empresa no tiene un plan MercadoPago activo.");

        try
        {
            await using var cmdStatus = new SqlCommand(
                @"UPDATE Empresas SET
                    MpStatus = 'cancelled',
                    UpdatedAt = SYSUTCDATETIME()
                  WHERE Id = @EmpresaId", conn);
            cmdStatus.Parameters.AddWithValue("@EmpresaId", empId);
            await cmdStatus.ExecuteNonQueryAsync();

            _logger.LogInformation("MP plan cancelled locally for empresa {EmpresaId}", empId);
            await _licenseUpdate.LogEventAsync(empId, "mp_cancelled", "MercadoPago", "Plan MercadoPago cancelado por el usuario");
            return (true, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling MP plan for empresa {EmpresaId}", empId);
            return (false, $"Error: {ex.Message}");
        }
    }

    // ─── Helpers ───

    /// <summary>
    /// Parsea ExternalReference con formato:
    /// emp_{id}_plan_{plan}_{monthly|annual} o emp_{id}_enterprise
    /// </summary>
    private static bool TryParseExternalReference(string? externalRef, out int empresaId, out string? plan, out string? periodo)
    {
        empresaId = 0;
        plan = null;
        periodo = null;

        if (string.IsNullOrEmpty(externalRef) || !externalRef.StartsWith("emp_"))
            return false;

        var parts = externalRef.Split('_');
        if (parts.Length < 2 || !int.TryParse(parts[1], out empresaId))
            return false;

        if (parts.Length >= 4 && parts[2] == "plan")
        {
            plan = parts[3];
            // periodo es opcional (backward compat con refs sin periodo)
            if (parts.Length >= 5)
                periodo = parts[4]; // "monthly" o "annual"
        }
        else if (parts.Length >= 3 && parts[2] == "enterprise")
        {
            plan = "enterprise";
        }

        return empresaId > 0;
    }
}
