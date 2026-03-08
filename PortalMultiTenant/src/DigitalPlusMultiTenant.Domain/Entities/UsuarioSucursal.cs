namespace DigitalPlusMultiTenant.Domain.Entities;

public class UsuarioSucursal
{
    public string UserId { get; set; } = null!;
    public int SucursalId { get; set; }

    // Navigation
    public ApplicationUser User { get; set; } = null!;
    public Sucursal Sucursal { get; set; } = null!;
}
