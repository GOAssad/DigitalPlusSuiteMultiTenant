using DigitalPlus.Entidades;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalPlus.DTOs
{
    public class FeriadoDTO : Feriado
    {
        [NotMapped]
        public DateTime FechaHasta { get; set; } = DateTime.Today;
    }
}
