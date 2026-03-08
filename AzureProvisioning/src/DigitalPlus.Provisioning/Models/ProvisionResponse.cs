using System.Text.Json.Serialization;

namespace DigitalPlus.Provisioning.Models;

public class ProvisionResponse
{
    [JsonPropertyName("companySanitized")]
    public string CompanySanitized { get; set; } = string.Empty;

    [JsonPropertyName("dbName")]
    public string DbName { get; set; } = string.Empty;

    [JsonPropertyName("mode")]
    public string Mode { get; set; } = string.Empty;

    [JsonPropertyName("server")]
    public string? Server { get; set; }

    [JsonPropertyName("connectionString")]
    public string? ConnectionString { get; set; }

    [JsonPropertyName("policy")]
    public ProvisionPolicy Policy { get; set; } = new();
}

public class ProvisionPolicy
{
    [JsonPropertyName("cloudMustFailIfDbExists")]
    public bool CloudMustFailIfDbExists { get; set; } = true;

    [JsonPropertyName("localMustFailIfDbExists")]
    public bool LocalMustFailIfDbExists { get; set; } = true;
}
