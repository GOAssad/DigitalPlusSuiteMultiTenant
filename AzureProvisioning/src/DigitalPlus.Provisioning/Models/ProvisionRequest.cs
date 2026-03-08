using System.Text.Json.Serialization;

namespace DigitalPlus.Provisioning.Models;

public class ProvisionRequest
{
    [JsonPropertyName("activationCode")]
    public string ActivationCode { get; set; } = string.Empty;

    [JsonPropertyName("companyName")]
    public string CompanyName { get; set; } = string.Empty;

    [JsonPropertyName("installType")]
    public string InstallType { get; set; } = string.Empty;

    [JsonPropertyName("machineId")]
    public string MachineId { get; set; } = string.Empty;
}
