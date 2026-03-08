using DigitalPlusMultiTenant.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace DigitalPlusMultiTenant.Domain.Entities;

public class ApplicationUser : IdentityUser, ITenantEntity
{
    public int EmpresaId { get; set; }
    public string? NombreCompleto { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Empresa Empresa { get; set; } = null!;
    public ICollection<UsuarioSucursal> UsuarioSucursales { get; set; } = [];
}
