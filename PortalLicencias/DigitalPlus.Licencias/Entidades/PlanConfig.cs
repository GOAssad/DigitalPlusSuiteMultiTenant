using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalPlus.Licencias.Entidades;

[Table("PlanConfig")]
public class PlanConfig
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Plan { get; set; } = null!;

    [Required, MaxLength(100)]
    public string Parametro { get; set; } = null!;

    public int Valor { get; set; }

    [MaxLength(200)]
    public string? Descripcion { get; set; }

    [MaxLength(50)]
    public string? Categoria { get; set; }

    [MaxLength(50)]
    public string? TipoVisualizacion { get; set; }

    [MaxLength(100)]
    public string? LabelAmigable { get; set; }

    [MaxLength(50)]
    public string? Icono { get; set; }

    public int OrdenVisualizacion { get; set; } = 50;

    public bool VisibleEnComparacion { get; set; } = true;
}
