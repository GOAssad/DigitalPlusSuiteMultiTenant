namespace DigitalPlusMultiTenant.Domain.Entities;

public class LegajoSucursal
{
    public int LegajoId { get; set; }
    public int SucursalId { get; set; }

    // Medios de fichada habilitados por sucursal
    public bool PermiteHuella { get; set; } = true;
    public bool PermitePin { get; set; } = true;
    public bool PermiteQr { get; set; } = true;
    public bool PermiteMovil { get; set; } = true;
    public bool PermiteKiosko { get; set; } = true;

    // Navigation
    public Legajo Legajo { get; set; } = null!;
    public Sucursal Sucursal { get; set; } = null!;
}
