using DigitalPlusMultiTenant.Domain.Common;
using DigitalPlusMultiTenant.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalPlusMultiTenant.Persistence.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await SeedRolesAsync(roleManager);
        await SeedSuperAdminAsync(context, userManager);
        var empresa = await SeedEmpresaKosiukoAsync(context);
        await SeedAdminEmpresaAsync(userManager, empresa.Id);
        await SeedVariablesSistemaAsync(context, empresa.Id);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = ["SuperAdmin", "AdminEmpresa", "Operador", "Consulta"];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private static async Task<Empresa> SeedEmpresaKosiukoAsync(ApplicationDbContext context)
    {
        var empresa = await context.Empresas
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(e => e.Codigo == "KSK");

        if (empresa != null)
            return empresa;

        empresa = new Empresa
        {
            Codigo = "KSK",
            Nombre = "Kosiuko S.A.",
            NombreFantasia = "Kosiuko",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "seed"
        };

        context.Empresas.Add(empresa);
        await context.SaveChangesAsync();

        return empresa;
    }

    /// <summary>
    /// SuperAdmin de IntegraIA: acceso cross-tenant a todas las empresas.
    /// Credenciales documentadas en DOC03-Manual_Portal_Licencias_IntegraIA.md
    /// </summary>
    private static async Task SeedSuperAdminAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        const string superAdminEmail = "admin@integraia.tech";

        // Asegurar que existe empresa IntegraIA (EmpresaId=1, tenant de gestion)
        var empresaIA = await context.Empresas
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(e => e.Codigo == "IA");

        if (empresaIA == null)
        {
            empresaIA = new Empresa
            {
                Codigo = "IA",
                Nombre = "IntegraIA Technology",
                NombreFantasia = "IntegraIA",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "seed"
            };
            context.Empresas.Add(empresaIA);
            await context.SaveChangesAsync();
        }

        var existingUser = await userManager.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Email == superAdminEmail);

        if (existingUser != null)
            return;

        var superAdmin = new ApplicationUser
        {
            UserName = superAdminEmail,
            Email = superAdminEmail,
            EmailConfirmed = true,
            NombreCompleto = "SuperAdmin IntegraIA",
            EmpresaId = empresaIA.Id,
            IsActive = true,
            AccesoAdminDesktop = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(superAdmin, "Admin123");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
        }
    }

    /// <summary>
    /// Admin de empresa Kosiuko (cliente de ejemplo). Rol AdminEmpresa, no SuperAdmin.
    /// </summary>
    private static async Task SeedAdminEmpresaAsync(UserManager<ApplicationUser> userManager, int empresaId)
    {
        const string adminEmail = "admin@kosiuko.com";

        var existingUser = await userManager.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Email == adminEmail);

        if (existingUser != null)
            return;

        var admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            NombreCompleto = "Administrador Kosiuko",
            EmpresaId = empresaId,
            IsActive = true,
            AccesoAdminDesktop = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(admin, "Admin123");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "AdminEmpresa");
        }
    }

    private static async Task SeedVariablesSistemaAsync(ApplicationDbContext context, int empresaId)
    {
        if (await context.VariablesSistema.IgnoreQueryFilters().AnyAsync(v => v.EmpresaId == empresaId))
            return;

        var variables = new List<VariableSistema>
        {
            new() { EmpresaId = empresaId, Clave = "FichadaModoPIN", Descripcion = "Habilita fichada por PIN", TipoValor = "bool", Valor = "true" },
            new() { EmpresaId = empresaId, Clave = "FichadaModoDemo", Descripcion = "Habilita modo demo (seleccion de legajo)", TipoValor = "bool", Valor = "false" },
            new() { EmpresaId = empresaId, Clave = "PinExpiraDias", Descripcion = "Dias de validez del PIN", TipoValor = "int", Valor = "90" },
            new() { EmpresaId = empresaId, Clave = "ToleranciaMinutos", Descripcion = "Minutos de tolerancia para llegada tarde", TipoValor = "int", Valor = "10" },
        };

        context.VariablesSistema.AddRange(variables);
        await context.SaveChangesAsync();
    }
}
