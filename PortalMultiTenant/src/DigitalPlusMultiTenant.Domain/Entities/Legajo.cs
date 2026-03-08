using DigitalPlusMultiTenant.Domain.Common;

namespace DigitalPlusMultiTenant.Domain.Entities;

public class Legajo : TenantEntity
{
    public string NumeroLegajo { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public int SectorId { get; set; }
    public int CategoriaId { get; set; }
    public int? HorarioId { get; set; }
    public bool IsActive { get; set; } = true;
    public bool HasCalendarioPersonalizado { get; set; }
    public byte[]? Foto { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public DateOnly? FechaIngreso { get; set; }
    public DateOnly? FechaEgreso { get; set; }

    // Navigation
    public Sector Sector { get; set; } = null!;
    public Categoria Categoria { get; set; } = null!;
    public Horario? Horario { get; set; }
    public LegajoPin? Pin { get; set; }
    public LegajoDomicilio? Domicilio { get; set; }
    public ICollection<LegajoHuella> Huellas { get; set; } = [];
    public ICollection<LegajoSucursal> LegajoSucursales { get; set; } = [];
    public ICollection<Fichada> Fichadas { get; set; } = [];
    public ICollection<IncidenciaLegajo> IncidenciaLegajos { get; set; } = [];
    public ICollection<Vacacion> Vacaciones { get; set; } = [];
    public ICollection<EventoCalendario> EventosCalendario { get; set; } = [];
}
