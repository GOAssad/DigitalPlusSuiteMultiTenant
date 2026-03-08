using DigitalPlusMultiTenant.Domain.Common;
using DigitalPlusMultiTenant.Domain.Enums;

namespace DigitalPlusMultiTenant.Domain.Entities;

public class HorarioDetalle : BaseEntity
{
    public int HorarioId { get; set; }
    public DiaSemana DiaSemana { get; set; }
    public TimeOnly HoraDesde { get; set; }
    public TimeOnly HoraHasta { get; set; }
    public bool IsCerrado { get; set; }

    // Navigation
    public Horario Horario { get; set; } = null!;
}
