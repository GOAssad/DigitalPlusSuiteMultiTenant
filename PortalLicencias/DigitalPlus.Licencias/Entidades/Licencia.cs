using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalPlus.Licencias.Entidades;

[Table("Licencias")]
public class Licencia
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string CompanyId { get; set; } = string.Empty;

    [MaxLength(100)]
    public string MachineId { get; set; } = string.Empty;

    [MaxLength(20)]
    public string LicenseType { get; set; } = "trial"; // trial, active, suspended

    [MaxLength(50)]
    public string Plan { get; set; } = "free";

    public int MaxLegajos { get; set; } = 5;
    public int MaxSucursales { get; set; } = 1;
    public int MaxFichadasMes { get; set; } = 200;

    [MaxLength(100)]
    public string? ActivationCode { get; set; }

    public DateTime? TrialEndsAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? SuspendedAt { get; set; }
    public DateTime? GraceEndsAt { get; set; }
    public DateTime? LastHeartbeat { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Propiedades calculadas para UI
    [NotMapped]
    public string EstadoDisplay => LicenseType switch
    {
        "trial" => TrialEndsAt.HasValue && TrialEndsAt.Value < DateTime.UtcNow ? "Trial Vencido" : "Trial",
        "active" => ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow ? "Vencida" : "Activa",
        "suspended" => "Suspendida",
        _ => LicenseType
    };

    [NotMapped]
    public int? DiasRestantes
    {
        get
        {
            var fecha = LicenseType == "trial" ? TrialEndsAt : ExpiresAt;
            if (!fecha.HasValue) return null;
            return (int)(fecha.Value - DateTime.UtcNow).TotalDays;
        }
    }
}
