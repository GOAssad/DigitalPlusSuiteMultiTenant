using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalPlus.Licencias.Entidades;

[Table("Monedas")]
public class Moneda
{
    public int Id { get; set; }

    [Required, MaxLength(10)]
    public string Codigo { get; set; } = "";

    [Required, MaxLength(100)]
    public string Nombre { get; set; } = "";

    [Required, MaxLength(10)]
    public string Simbolo { get; set; } = "$";

    public bool EsBase { get; set; }

    public bool Activa { get; set; } = true;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
