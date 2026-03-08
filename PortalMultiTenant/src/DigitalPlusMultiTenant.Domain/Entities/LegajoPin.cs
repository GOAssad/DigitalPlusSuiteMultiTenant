using DigitalPlusMultiTenant.Domain.Common;

namespace DigitalPlusMultiTenant.Domain.Entities;

public class LegajoPin : BaseEntity
{
    public int LegajoId { get; set; }
    public string PinHash { get; set; } = null!;
    public string PinSalt { get; set; } = null!;
    public bool PinMustChange { get; set; }
    public DateTime? PinChangedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Legajo Legajo { get; set; } = null!;
}
