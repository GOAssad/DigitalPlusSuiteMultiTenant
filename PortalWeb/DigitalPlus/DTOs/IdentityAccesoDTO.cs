using System.ComponentModel.DataAnnotations;

namespace DigitalPlus.DTOs
{
    public class RegistroDTO
    {
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
      
        public bool RememberMe { get; set; }
    }
}
