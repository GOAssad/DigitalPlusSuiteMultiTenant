using System.ComponentModel.DataAnnotations;

namespace DigitalPlus.Entidades
{
    public class Terminal
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo es obligatirio")]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        [Required]
        public int SucursalId { get; set; }
        public Sucursal Sucursal { get; set; }

    }
}
