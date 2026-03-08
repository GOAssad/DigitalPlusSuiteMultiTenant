using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalPlus.Licencias.Entidades;

[Table("LicenseCodes")]
public class LicenseCode
{
    public int Id { get; set; }

    [MaxLength(64)]
    public string CodeHash { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Plan { get; set; } = "basic";

    public int MaxLegajos { get; set; } = 25;
    public int DurationDays { get; set; } = 365;
    public DateTime ExpiresAt { get; set; }
    public DateTime? UsedAt { get; set; }

    [MaxLength(100)]
    public string? UsedByCompany { get; set; }

    [MaxLength(100)]
    public string? UsedByMachine { get; set; }

    public DateTime CreatedAt { get; set; }

    [MaxLength(100)]
    public string? CreatedBy { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    // Calculadas
    [NotMapped]
    public string EstadoDisplay => UsedAt.HasValue ? "Usado" :
        ExpiresAt < DateTime.UtcNow ? "Expirado" : "Disponible";

    [NotMapped]
    public bool IsAvailable => !UsedAt.HasValue && ExpiresAt >= DateTime.UtcNow;
}
