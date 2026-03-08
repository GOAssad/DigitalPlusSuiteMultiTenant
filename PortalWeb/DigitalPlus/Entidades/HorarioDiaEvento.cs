using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalPlus.Entidades
{
    public class HorarioDiaEvento
    {
        public int Id { get; set; }
        [Required]
        public DateTime FechaDesde { get; set; } = new DateTime(1900, 1, 1);
        [Required]
        public DateTime FechaHasta { get; set; } = new DateTime(1900, 1, 1);
        [Required]
        public int LegajoId { get; set; }
        public string Nota { get; set; }

        //propiedad de Navegacion
        public Legajo Legajo { get; set; }

    }
}
