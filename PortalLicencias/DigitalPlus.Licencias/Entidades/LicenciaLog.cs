using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalPlus.Licencias.Entidades;

[Table("LicenciasLog")]
public class LicenciaLog
{
    public long Id { get; set; }
    public int? LicenciaId { get; set; }

    [MaxLength(50)]
    public string Action { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? App { get; set; }

    [MaxLength(500)]
    public string? Details { get; set; }

    [MaxLength(50)]
    public string? IP { get; set; }

    public DateTime Timestamp { get; set; }

    [ForeignKey(nameof(LicenciaId))]
    public Licencia? Licencia { get; set; }
}
