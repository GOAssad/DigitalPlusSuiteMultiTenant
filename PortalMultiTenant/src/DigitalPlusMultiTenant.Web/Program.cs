using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using DigitalPlusMultiTenant.Web.Components;
using DigitalPlusMultiTenant.Web.Components.Account;
using DigitalPlusMultiTenant.Domain.Entities;
using DigitalPlusMultiTenant.Application.Interfaces;
using DigitalPlusMultiTenant.Infrastructure.Services;
using DigitalPlusMultiTenant.Persistence;
using DigitalPlusMultiTenant.Persistence.Seed;

namespace DigitalPlusMultiTenant.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

        // Tenant service
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ITenantService, TenantService>();

        // Database
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
        // Factory for Blazor components - each component gets its own DbContext
        builder.Services.AddScoped<IDbContextFactory<ApplicationDbContext>>(sp =>
        {
            var options = sp.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
            var tenantService = sp.GetRequiredService<ITenantService>();
            return new ScopedDbContextFactory(options, tenantService);
        });
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
            .AddDefaultTokenProviders()
            .AddClaimsPrincipalFactory<CustomClaimsPrincipalFactory>();

        builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
        builder.Services.AddMemoryCache();

        var app = builder.Build();

        // Seed database
        await DatabaseSeeder.SeedAsync(app.Services);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
        app.UseHttpsRedirection();

        app.UseAntiforgery();

        // Forzar cambio de contraseña temporal
        app.Use(async (context, next) =>
        {
            if (context.User.Identity?.IsAuthenticated == true
                && context.User.HasClaim("MustChangePassword", "true")
                && !context.Request.Path.StartsWithSegments("/Account/ForceChangePassword")
                && !context.Request.Path.StartsWithSegments("/Account/Logout")
                && !context.Request.Path.StartsWithSegments("/_blazor")
                && !context.Request.Path.StartsWithSegments("/_framework"))
            {
                context.Response.Redirect("/Account/ForceChangePassword");
                return;
            }
            await next();
        });

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        // Add additional endpoints required by the Identity /Account Razor components.
        app.MapAdditionalIdentityEndpoints();

        // Logo de empresa desde DigitalPlusAdmin (cacheado 1 hora)
        app.MapGet("/api/empresa-logo", async (HttpContext ctx, IConfiguration config, IMemoryCache cache) =>
        {
            var empresaNombre = ctx.User.FindFirst("EmpresaNombre")?.Value;
            if (string.IsNullOrEmpty(empresaNombre))
                return Results.NotFound();

            var cacheKey = $"empresa-logo-{empresaNombre}";
            if (cache.TryGetValue(cacheKey, out (byte[] data, string contentType) cached))
                return Results.File(cached.data, cached.contentType);

            var adminCs = config.GetConnectionString("AdminConnection");
            if (string.IsNullOrEmpty(adminCs))
                return Results.NotFound();

            try
            {
                using var conn = new SqlConnection(adminCs);
                await conn.OpenAsync();
                using var cmd = new SqlCommand(
                    "SELECT Logo, LogoContentType FROM Empresas WHERE Nombre = @Nombre", conn);
                cmd.Parameters.AddWithValue("@Nombre", empresaNombre);
                cmd.CommandTimeout = 10;
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync() && !reader.IsDBNull(0))
                {
                    var logo = (byte[])reader[0];
                    var contentType = reader.IsDBNull(1) ? "image/png" : reader.GetString(1);
                    cache.Set(cacheKey, (logo, contentType), TimeSpan.FromHours(1));
                    return Results.File(logo, contentType);
                }
            }
            catch { }

            return Results.NotFound();
        }).RequireAuthorization();

        await app.RunAsync();
    }
}
