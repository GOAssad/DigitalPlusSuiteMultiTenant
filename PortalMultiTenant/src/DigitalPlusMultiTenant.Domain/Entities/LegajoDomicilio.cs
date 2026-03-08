using DigitalPlusMultiTenant.Domain.Common;

namespace DigitalPlusMultiTenant.Domain.Entities;

public class LegajoDomicilio : BaseEntity
{
    public int LegajoId { get; set; }
    public string? Calle { get; set; }
    public string? Altura { get; set; }
    public string? Piso { get; set; }
    public string? Barrio { get; set; }
    public string? Localidad { get; set; }
    public string? Provincia { get; set; }
    public string? CodigoPostal { get; set; }

    // Navigation
    public Legajo Legajo { get; set; } = null!;
}
