using System.ComponentModel.DataAnnotations;

namespace DigitalPlus.Entidades
{
    public class Feriado
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Por favor ingrese un Nombre que identifique el Feriado")]
        public string Nombre { get; set; }
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; } = DateTime.Today;

    }
}
