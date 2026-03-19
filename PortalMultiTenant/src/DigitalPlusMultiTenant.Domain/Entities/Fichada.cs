using DigitalPlusMultiTenant.Domain.Common;
using DigitalPlusMultiTenant.Domain.Enums;

namespace DigitalPlusMultiTenant.Domain.Entities;

public class Fichada : BaseEntity, ITenantEntity
{
    public int EmpresaId { get; set; }
    public int LegajoId { get; set; }
    public int SucursalId { get; set; }
    public int? TerminalId { get; set; }
    public DateTime FechaHora { get; set; }
    public string Tipo { get; set; } = null!; // "E" o "S"
    public string? Origen { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModificadoPor { get; set; }
    public DateTime? ModificadoAt { get; set; }

    // Navigation
    public Empresa Empresa { get; set; } = null!;
    public Legajo Legajo { get; set; } = null!;
    public Sucursal Sucursal { get; set; } = null!;
    public Terminal? Terminal { get; set; }
}
