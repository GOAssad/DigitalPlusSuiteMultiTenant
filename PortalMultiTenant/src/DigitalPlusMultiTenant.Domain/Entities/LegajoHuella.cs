namespace DigitalPlusMultiTenant.Domain.Entities;

public class LegajoHuella
{
    public int LegajoId { get; set; }
    public int DedoId { get; set; }
    public byte[] Huella { get; set; } = null!;
    public int FingerMask { get; set; }

    // Navigation
    public Legajo Legajo { get; set; } = null!;
}
