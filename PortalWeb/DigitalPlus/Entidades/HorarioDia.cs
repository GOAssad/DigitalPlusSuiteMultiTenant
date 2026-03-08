using DigitalPlus.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalPlus.Entidades
{
    public class HorarioDia
    {
        public int Id { get; set; }
        public int HorarioId { get; set; }
        public int DiaId { get; set; }
        public int HoraDesde { get; set; }
        public int HoraHasta { get; set; }
        public int MinutoDesde { get; set; }
        public int MinutoHasta { get; set; }
        public int Cerrado { get; set; }
        [NotMapped]
        public Dia Dia { get; set; }
        

    }
}
