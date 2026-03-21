using DigitalPlusMultiTenant.Domain.Common;

namespace DigitalPlusMultiTenant.Domain.Entities;

public class TerminalMovil : BaseEntity, ITenantEntity
{
    public int EmpresaId { get; set; }
    public int? LegajoId { get; set; }
    public string DeviceId { get; set; } = null!;
    public string? PublicKey { get; set; }
    public string? Nombre { get; set; }
    public string? Plataforma { get; set; }
    public DateTime FechaRegistro { get; set; }
    public DateTime? UltimoUso { get; set; }
    public bool Activo { get; set; } = true;
    public bool ModoKiosko { get; set; }
    public int? SucursalId { get; set; }

    // Navigation
    public Empresa Empresa { get; set; } = null!;
    public Legajo? Legajo { get; set; }
    public Sucursal? Sucursal { get; set; }
}
