using System.Text.Json.Serialization;

namespace DigitalPlus.Provisioning.Models;

// ============================================================
// REQUEST: POST /api/license/activate
// ============================================================
public class LicenseActivateRequest
{
    [JsonPropertyName("activationCode")]
    public string? ActivationCode { get; set; }    // null/empty = trial

    [JsonPropertyName("companyName")]
    public string CompanyName { get; set; } = string.Empty;

    [JsonPropertyName("machineId")]
    public string MachineId { get; set; } = string.Empty;

    [JsonPropertyName("installType")]
    public string InstallType { get; set; } = string.Empty;  // "cloud" or "local"

    [JsonPropertyName("empresaId")]
    public int? EmpresaId { get; set; }    // Preferido sobre companyName para lookup
}

// ============================================================
// REQUEST: POST /api/license/heartbeat
// ============================================================
public class LicenseHeartbeatRequest
{
    [JsonPropertyName("companyId")]
    public string CompanyId { get; set; } = string.Empty;

    [JsonPropertyName("machineId")]
    public string MachineId { get; set; } = string.Empty;

    [JsonPropertyName("app")]
    public string App { get; set; } = string.Empty;  // "Fichador" or "Administrador"

    [JsonPropertyName("activeLegajos")]
    public int ActiveLegajos { get; set; }

    [JsonPropertyName("empresaId")]
    public int? EmpresaId { get; set; }    // Preferido sobre companyId para lookup
}

// ============================================================
// RESPONSE: ticket + signature
// ============================================================
public class LicenseResponse
{
    [JsonPropertyName("ticket")]
    public string Ticket { get; set; } = string.Empty;

    [JsonPropertyName("signature")]
    public string Signature { get; set; } = string.Empty;
}

// ============================================================
// LICENSE TICKET: JSON que se firma con RSA
// ============================================================
public class LicenseTicket
{
    [JsonPropertyName("v")]
    public int Version { get; set; } = 1;

    [JsonPropertyName("empresaId")]
    public int? EmpresaId { get; set; }

    [JsonPropertyName("companyId")]
    public string CompanyId { get; set; } = string.Empty;

    [JsonPropertyName("machineId")]
    public string MachineId { get; set; } = string.Empty;

    [JsonPropertyName("licenseType")]
    public string LicenseType { get; set; } = string.Empty;  // "trial", "active", "suspended"

    [JsonPropertyName("plan")]
    public string Plan { get; set; } = "free";

    [JsonPropertyName("maxLegajos")]
    public int MaxLegajos { get; set; } = 5;

    [JsonPropertyName("trialEndsAt")]
    public DateTime? TrialEndsAt { get; set; }

    [JsonPropertyName("expiresAt")]
    public DateTime? ExpiresAt { get; set; }

    [JsonPropertyName("suspendedAt")]
    public DateTime? SuspendedAt { get; set; }

    [JsonPropertyName("graceEndsAt")]
    public DateTime? GraceEndsAt { get; set; }

    [JsonPropertyName("issuedAt")]
    public DateTime IssuedAt { get; set; }

    [JsonPropertyName("nextCheckRequiredAt")]
    public DateTime NextCheckRequiredAt { get; set; }

    [JsonPropertyName("serverTimeUtc")]
    public DateTime ServerTimeUtc { get; set; }
}
