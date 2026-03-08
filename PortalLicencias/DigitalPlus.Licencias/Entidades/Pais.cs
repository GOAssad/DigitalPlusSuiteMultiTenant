using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalPlus.Licencias.Entidades;

[Table("Paises")]
public class Pais
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(3)]
    public string CodigoISO { get; set; } = string.Empty;

    public List<TipoIdentificacionFiscal> TiposIdentificacion { get; set; } = [];
}
