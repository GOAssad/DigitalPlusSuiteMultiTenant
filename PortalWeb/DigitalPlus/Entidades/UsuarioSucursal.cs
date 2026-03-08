using Microsoft.AspNetCore.Identity;

namespace DigitalPlus.Entidades
{
    public class UsuarioSucursal
    {
        public string UsuarioId { get; set; }
        public int SucursalId { get; set; }
        public IdentityUser Usuario { get; set; }
        public Sucursal Sucursal { get; set; }
    }
}
