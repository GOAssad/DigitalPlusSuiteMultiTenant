using System.ComponentModel.DataAnnotations;

namespace DigitalPlus.Entidades
{
    public class Incidencia
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo es obligatirio")]
        public string Nombre { get; set; }
        public string Color { get; set; }

        [MaxLength(4)]
        public string Abreviatura { get; set; }
    }
}
