using System.ComponentModel.DataAnnotations;

namespace DigitalPlus.Entidades
{
    public class Horario
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Este campo es Requerido")]
        public string Nombre { get; set; }
        public List<HorarioDia> HorariosDias { get; set; } = new List<HorarioDia>();

    }
}
