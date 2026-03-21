using System.Security.Claims;
using DigitalPlusMultiTenant.Application.Interfaces;
using DigitalPlusMultiTenant.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DigitalPlusMultiTenant.Infrastructure.Services;

public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
{
    private readonly IApplicationDbContext _db;

    public CustomClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> options,
        IApplicationDbContext db)
        : base(userManager, roleManager, options)
    {
        _db = db;
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        identity.AddClaim(new Claim("EmpresaId", user.EmpresaId.ToString()));

        identity.AddClaim(new Claim("EmailConfirmed", user.EmailConfirmed ? "true" : "false"));

        if (user.MustChangePassword)
            identity.AddClaim(new Claim("MustChangePassword", "true"));

        var empresa = await _db.Empresas.AsNoTracking()
            .Where(e => e.Id == user.EmpresaId)
            .Select(e => new { e.Nombre, e.MobileHabilitado, e.KioskoHabilitado })
            .FirstOrDefaultAsync();
        if (empresa != null)
        {
            if (!string.IsNullOrEmpty(empresa.Nombre))
                identity.AddClaim(new Claim("EmpresaNombre", empresa.Nombre));
            if (empresa.MobileHabilitado)
                identity.AddClaim(new Claim("MobileHabilitado", "true"));
            if (empresa.KioskoHabilitado)
                identity.AddClaim(new Claim("KioskoHabilitado", "true"));
        }

        return identity;
    }
}
