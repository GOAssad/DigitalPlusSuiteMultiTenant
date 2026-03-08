namespace DigitalPlusMultiTenant.Domain.Entities;

public class LegajoSucursal
{
    public int LegajoId { get; set; }
    public int SucursalId { get; set; }

    // Navigation
    public Legajo Legajo { get; set; } = null!;
    public Sucursal Sucursal { get; set; } = null!;
}
