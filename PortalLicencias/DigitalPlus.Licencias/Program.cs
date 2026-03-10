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

// API para instalador liviano: activar codigo y obtener connection string
app.MapPost("/api/activar", async (ActivarRequest req,
    RepositorioLicencias repo, DatabaseProvisioningService provisioning) =>
{
    if (string.IsNullOrWhiteSpace(req.Codigo))
        return Results.BadRequest(new { error = "Codigo requerido" });

    var empresa = await repo.BuscarEmpresaPorCodigoActivacionAsync(req.Codigo.Trim());
    if (empresa == null)
        return Results.NotFound(new { error = "Codigo invalido o empresa inactiva" });

    var connectionString = provisioning.BuildClientConnectionString(empresa.DatabaseName);

    return Results.Ok(new
    {
        connectionString,
        empresaId = empresa.Id,
        companyId = empresa.CompanyId,
        nombreEmpresa = empresa.Nombre,
        databaseName = empresa.DatabaseName
    });
});

app.Run();

record ActivarRequest(string Codigo);
