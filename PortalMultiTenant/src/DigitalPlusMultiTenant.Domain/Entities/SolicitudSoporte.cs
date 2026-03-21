using DigitalPlusMultiTenant.Domain.Common;

namespace DigitalPlusMultiTenant.Domain.Entities;

public class SolicitudSoporte : BaseEntity, ITenantEntity
{
    public int EmpresaId { get; set; }
    public string Tipo { get; set; } = null!; // "Limpieza" or "Eliminacion"
    public string Motivo { get; set; } = null!;
    public string SolicitadoPor { get; set; } = null!; // email del usuario
    public DateTime FechaSolicitud { get; set; }
    public string Estado { get; set; } = "Pendiente"; // Pendiente, EnProceso, Completada, Rechazada
    public DateTime? FechaResolucion { get; set; }
    public string? Comentario { get; set; }
    public bool VistoPorUsuario { get; set; }

    // Navigation
    public Empresa Empresa { get; set; } = null!;
}
