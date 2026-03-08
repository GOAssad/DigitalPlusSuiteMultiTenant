using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalPlus.Licencias.Entidades;

[Table("TiposIdentificacionFiscal")]
public class TipoIdentificacionFiscal
{
    public int Id { get; set; }

    public int PaisId { get; set; }

    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Formato { get; set; }

    [MaxLength(100)]
    public string? Ejemplo { get; set; }

    [ForeignKey(nameof(PaisId))]
    public Pais? Pais { get; set; }
}
