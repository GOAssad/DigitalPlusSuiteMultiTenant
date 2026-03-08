using System.ComponentModel.DataAnnotations;

namespace DigitalPlus.Entidades
{
    public class Sector
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo es obligatirio")]
        public string Nombre { get; set; }
       

    }
}