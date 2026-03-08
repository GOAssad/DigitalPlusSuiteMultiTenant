using DigitalPlusMultiTenant.Domain.Entities;

namespace DigitalPlusMultiTenant.Domain.Common;

public class Empresa : AuditableEntity
{
    public string Codigo { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string? NombreFantasia { get; set; }
    public string? Cuit { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<Sucursal> Sucursales { get; set; } = [];
    public ICollection<Sector> Sectores { get; set; } = [];
    public ICollection<Categoria> Categorias { get; set; } = [];
    public ICollection<Horario> Horarios { get; set; } = [];
    public ICollection<Terminal> Terminales { get; set; } = [];
    public ICollection<Legajo> Legajos { get; set; } = [];
    public ICollection<Fichada> Fichadas { get; set; } = [];
    public ICollection<Incidencia> Incidencias { get; set; } = [];
    public ICollection<Feriado> Feriados { get; set; } = [];
    public ICollection<Noticia> Noticias { get; set; } = [];
}
