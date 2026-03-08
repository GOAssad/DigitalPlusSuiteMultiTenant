using System.ComponentModel.DataAnnotations;

namespace DigitalPlus.DTOs
{
    public class IdentityRegistroDTO
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Las contraseñas no coinciden")]
        [Display(Name ="Confirmar Password")]
        public string ConfirmPassword { get; set; }
    }
}
