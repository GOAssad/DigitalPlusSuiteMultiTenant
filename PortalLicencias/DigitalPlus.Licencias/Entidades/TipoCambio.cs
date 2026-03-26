using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalPlus.Licencias.Entidades;

[Table("TiposCambio")]
public class TipoCambio
{
    public int Id { get; set; }

    public int MonedaOrigenId { get; set; }

    public int MonedaDestinoId { get; set; }

    [Column(TypeName = "decimal(18,4)")]
    public decimal Valor { get; set; }

    public DateTime VigenteDesde { get; set; }

    [MaxLength(100)]
    public string? CreadoPor { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation
    [ForeignKey(nameof(MonedaOrigenId))]
    public Moneda MonedaOrigen { get; set; } = null!;

    [ForeignKey(nameof(MonedaDestinoId))]
    public Moneda MonedaDestino { get; set; } = null!;
}
