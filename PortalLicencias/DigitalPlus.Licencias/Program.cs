using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DigitalPlus.Licencias.Components;
using DigitalPlus.Licencias.Components.Account;
using DigitalPlus.Licencias.Data;
using DigitalPlus.Licencias.Repositorios;
using DigitalPlus.Licencias.Services;

var builder = WebApplication.CreateBuilder(args);

// Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Auth
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Identity
builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

// Servicios
builder.Services.AddTransient<RepositorioLicencias>();
builder.Services.AddSingleton<DatabaseProvisioningService>();
builder.Services.AddSingleton<MultiTenantProvisioningService>();
builder.Services.AddSingleton<IEmailService, EmailService>();

var app = builder.Build();

// Crear roles y usuario admin en primer inicio
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roles = ["Administrador"];
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    var adminEmail = "admin@digitalplus.com";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(admin, "Admin123");
        if (result.Succeeded)
            await userManager.AddToRoleAsync(admin, "Administrador");
    }
}

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

// API para instalador: obtener lista de paises
app.MapGet("/api/paises", async (RepositorioLicencias repo) =>
{
    var paises = await repo.GetPaisesAsync();
    return Results.Ok(paises.Select(p => new { p.Id, p.Nombre }));
});

// API para validar datos antes de registrar (usada por el instalador antes de instalar archivos)
app.MapPost("/api/validar-free", async (ActivarFreeRequest req,
    RepositorioLicencias repo, MultiTenantProvisioningService mtProvisioning) =>
{
    if (string.IsNullOrWhiteSpace(req.Nombre))
        return Results.BadRequest(new { error = "Nombre de empresa requerido" });
    if (string.IsNullOrWhiteSpace(req.Email))
        return Results.BadRequest(new { error = "Email requerido" });

    var companyId = req.Nombre.Trim().ToLowerInvariant()
        .Replace(" ", "-").Replace(".", "").Replace(",", "");

    // Verificar si ya existe la empresa (y esta completa)
    var existente = await repo.GetEmpresaPorCompanyIdAsync(companyId);
    if (existente != null)
    {
        var mtId = await mtProvisioning.BuscarEmpresaIdPorCodigoAsync(existente.CompanyId);
        if (mtId != null && mtId > 0)
            return Results.Ok(new { ok = true, existe = true }); // Reintento, OK
        // Incompleta: se limpiara en activar-free
    }

    // Verificar email duplicado
    var emailExiste = await mtProvisioning.ExisteEmailAsync(req.Email.Trim());
    if (emailExiste)
        return Results.BadRequest(new { error = "Ese email ya se encuentra registrado. Ingrese otro." });

    return Results.Ok(new { ok = true, existe = false });
});

// API para instalador liviano: activar codigo y obtener connection string
app.MapPost("/api/activar", async (ActivarRequest req,
    RepositorioLicencias repo, DatabaseProvisioningService provisioning,
    MultiTenantProvisioningService mtProvisioning, IConfiguration config) =>
{
    if (string.IsNullOrWhiteSpace(req.Codigo))
        return Results.BadRequest(new { error = "Codigo requerido" });

    var empresa = await repo.BuscarEmpresaPorCodigoActivacionAsync(req.Codigo.Trim());
    if (empresa == null)
        return Results.NotFound(new { error = "Codigo invalido o empresa inactiva" });

    // Connection string a DigitalPlusMultiTenant (BD compartida multi-tenant, NO por empresa)
    var multiTenantDbName = config["MultiTenant:DatabaseName"] ?? "DigitalPlusMultiTenant";
    var tenantConnectionString = provisioning.BuildClientConnectionString(multiTenantDbName);

    // Connection string a DigitalPlusAdmin (para info de empresa: logo, redes sociales)
    var adminConnectionString = config.GetConnectionString("DefaultConnection") ?? "";

    // EmpresaId real en DigitalPlusMultiTenant (diferente al Id en DigitalPlusAdmin)
    var tenantEmpresaId = await mtProvisioning.BuscarEmpresaIdPorCodigoAsync(empresa.CompanyId);

    return Results.Ok(new
    {
        connectionString = tenantConnectionString,
        adminConnectionString,
        empresaId = tenantEmpresaId ?? 0,
        adminEmpresaId = empresa.Id,
        companyId = empresa.CompanyId,
        nombreEmpresa = empresa.Nombre,
        databaseName = empresa.DatabaseName,
        urlPortal = empresa.UrlPortal ?? ""
    });
});

// API para desktop apps: verificar si la empresa esta activa
app.MapPost("/api/verificar-estado", async (VerificarEstadoRequest req, RepositorioLicencias repo) =>
{
    if (string.IsNullOrWhiteSpace(req.CompanyId))
        return Results.BadRequest(new { error = "CompanyId requerido" });

    var empresa = await repo.GetEmpresaPorCompanyIdAsync(req.CompanyId.Trim());
    if (empresa == null)
        return Results.NotFound(new { error = "Empresa no encontrada", activa = false });

    return Results.Ok(new
    {
        activa = empresa.Estado == "activa",
        estado = empresa.Estado ?? "activa",
        nombre = empresa.Nombre
    });
});

// API para registro Free: crear empresa sin codigo de activacion
app.MapPost("/api/activar-free", async (ActivarFreeRequest req,
    RepositorioLicencias repo, DatabaseProvisioningService provisioning,
    MultiTenantProvisioningService mtProvisioning, IConfiguration config,
    IEmailService emailService) =>
{
    // Validar campos requeridos
    if (string.IsNullOrWhiteSpace(req.Nombre))
        return Results.BadRequest(new { error = "Nombre de empresa requerido" });
    if (string.IsNullOrWhiteSpace(req.Email))
        return Results.BadRequest(new { error = "Email requerido" });

    // Generar CompanyId desde el nombre (slug)
    var companyId = req.Nombre.Trim()
        .ToLowerInvariant()
        .Replace(" ", "-")
        .Replace(".", "")
        .Replace(",", "");

    // Si ya existe en Admin Y en MT, devolver datos (reintento legítimo)
    var existente = await repo.GetEmpresaPorCompanyIdAsync(companyId);
    if (existente != null)
    {
        var mtIdExist = await mtProvisioning.BuscarEmpresaIdPorCodigoAsync(existente.CompanyId);
        if (mtIdExist != null && mtIdExist > 0)
        {
            // Empresa completa, devolver datos
            var dbNameExist = config["MultiTenant:DatabaseName"] ?? "DigitalPlusMultiTenant";
            return Results.Ok(new
            {
                connectionString = provisioning.BuildClientConnectionString(dbNameExist),
                adminConnectionString = config.GetConnectionString("DefaultConnection") ?? "",
                empresaId = mtIdExist.Value,
                adminEmpresaId = existente.Id,
                companyId = existente.CompanyId,
                nombreEmpresa = existente.Nombre,
                databaseName = dbNameExist,
                urlPortal = existente.UrlPortal ?? "",
                plan = "free",
                email = existente.Email ?? req.Email.Trim(),
                password = "Admin123"
            });
        }
        else
        {
            // Existe en Admin pero no en MT (creacion anterior incompleta)
            // Rollback: eliminar de Admin para empezar de cero
            try
            {
                var adminConnStr = config.GetConnectionString("DefaultConnection")!;
                await mtProvisioning.EliminarEmpresaSoloAdminAsync(existente.Id, existente.CompanyId, adminConnStr);
            }
            catch { /* si falla el rollback, seguir intentando crear */ }
        }
    }

    // Verificar que el email no este registrado en otra empresa
    var emailExiste = await mtProvisioning.ExisteEmailAsync(req.Email.Trim());
    if (emailExiste)
        return Results.BadRequest(new { error = "Ese email ya se encuentra registrado. Ingrese otro." });

    // Obtener limites del plan Free de PlanConfig
    var planValores = await repo.GetPlanValoresAsync("free");
    int maxLegajos = planValores.GetValueOrDefault("MaxLegajos", 5);
    int maxSucursales = planValores.GetValueOrDefault("MaxSucursales", 1);
    int maxFichadasMes = planValores.GetValueOrDefault("MaxFichadasRolling30d", 200);
    int duracionDias = planValores.GetValueOrDefault("DuracionDias", 0);

    var dbName = config["MultiTenant:DatabaseName"] ?? "DigitalPlusMultiTenant";
    DigitalPlus.Licencias.Entidades.Empresa? empresa = null;

    try
    {
        // Paso 1: Crear empresa en DigitalPlusAdmin
        empresa = await repo.CrearEmpresaAsync(req.Nombre.Trim(), companyId, dbName, null, req.Email.Trim(), null);
        empresa.MobileHabilitado = true;
        if (req.PaisId.HasValue && req.PaisId.Value > 0)
            empresa.PaisId = req.PaisId.Value;
        await repo.ActualizarEmpresaAsync(empresa);

        // Paso 2: Crear licencia Free
        await repo.CrearLicenciaParaEmpresaConLimitesAsync(companyId, "free", maxLegajos, maxSucursales, maxFichadasMes, duracionDias);

        // Paso 3: Crear en Portal Multi-Tenant (empresa + admin user + datos default)
        var (empresaId, adminEmail, _) = await mtProvisioning.CreateEmpresaAndAdminAsync(
            companyId, req.Nombre.Trim(), null, req.Email.Trim());

        // Todo OK - preparar respuesta
        var tenantConnectionString = provisioning.BuildClientConnectionString(dbName);
        var adminConnectionString = config.GetConnectionString("DefaultConnection") ?? "";

        // Enviar email de bienvenida en background
        var emailTo = req.Email.Trim();
        var emailNombre = req.Nombre.Trim();
        var emailPortalUrl = empresa.UrlPortal ?? "https://digitalplusportalmt.azurewebsites.net";
        _ = Task.Run(async () =>
        {
            try
            {
                var htmlBody = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <div style='background: #1a1a2e; padding: 20px; text-align: center; border-radius: 8px 8px 0 0;'>
                        <h1 style='color: #d4a843; margin: 0;'>Digital One</h1>
                        <p style='color: #ccc; margin: 5px 0 0;'>Bienvenido a DigitalPlus</p>
                    </div>
                    <div style='padding: 25px; background: #f8f9fa; border: 1px solid #dee2e6;'>
                        <h2 style='color: #333;'>Hola!</h2>
                        <p>Su empresa <strong>{emailNombre}</strong> fue registrada exitosamente con el plan <strong>Free</strong>.</p>
                        <div style='background: white; border: 1px solid #dee2e6; border-radius: 6px; padding: 15px; margin: 20px 0;'>
                            <h3 style='margin-top: 0; color: #1a1a2e;'>Credenciales de acceso</h3>
                            <p><strong>Portal Web:</strong> <a href='{emailPortalUrl}'>{emailPortalUrl}</a></p>
                            <p><strong>Usuario:</strong> {adminEmail}</p>
                            <p><strong>Contrasena:</strong> Admin123</p>
                            <p style='color: #dc3545; font-size: 0.9em;'><strong>Importante:</strong> Debera cambiar la contrasena en el primer inicio de sesion.</p>
                        </div>
                        <div style='background: white; border: 1px solid #dee2e6; border-radius: 6px; padding: 15px; margin: 20px 0;'>
                            <h3 style='margin-top: 0; color: #1a1a2e;'>Aplicacion de escritorio</h3>
                            <p>Las mismas credenciales funcionan para la aplicacion <strong>Administrador</strong> de escritorio.</p>
                        </div>
                        <div style='background: #fff3cd; border: 1px solid #ffc107; border-radius: 6px; padding: 15px; margin: 20px 0;'>
                            <h3 style='margin-top: 0; color: #856404;'>Plan Free</h3>
                            <ul style='margin: 0; padding-left: 20px;'>
                                <li>Hasta 5 legajos</li>
                                <li>1 sucursal</li>
                                <li>200 fichadas por mes</li>
                                <li>Sin vencimiento</li>
                            </ul>
                        </div>
                    </div>
                    <div style='background: #1a1a2e; padding: 15px; text-align: center; border-radius: 0 0 8px 8px;'>
                        <p style='color: #888; margin: 0; font-size: 0.8em;'>Digital One by IntegraIA Tech</p>
                    </div>
                </div>";
                await emailService.SendAsync(emailTo, "Bienvenido a Digital One - Credenciales de acceso", htmlBody);
            }
            catch { /* fire and forget */ }
        });

        return Results.Ok(new
        {
            connectionString = tenantConnectionString,
            adminConnectionString,
            empresaId,
            adminEmpresaId = empresa.Id,
            companyId,
            nombreEmpresa = empresa.Nombre,
            databaseName = dbName,
            urlPortal = empresa.UrlPortal ?? "",
            plan = "free",
            email = adminEmail,
            password = "Admin123"
        });
    }
    catch (Exception ex)
    {
        // Rollback manual: si se creo en Admin, borrar
        if (empresa != null)
        {
            try
            {
                var adminConnStr = config.GetConnectionString("DefaultConnection")!;
                await mtProvisioning.EliminarEmpresaSoloAdminAsync(empresa.Id, companyId, adminConnStr);
            }
            catch { /* best effort rollback */ }
        }
        return Results.Problem($"Error creando empresa: {ex.Message}");
    }
});

app.Run();

record ActivarRequest(string Codigo);
record VerificarEstadoRequest(string CompanyId);
record ActivarFreeRequest(string Nombre, string Email, int? PaisId);
