using DigitalPlusMultiTenant.Application.Interfaces;
using DigitalPlusMultiTenant.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlusMultiTenant.Web;

/// <summary>
/// Creates a new ApplicationDbContext per call, reusing the scoped ITenantService.
/// Solves Blazor Server DbContext concurrency when multiple components share the same scope.
/// </summary>
internal class ScopedDbContextFactory : IDbContextFactory<ApplicationDbContext>
{
    private readonly DbContextOptions<ApplicationDbContext> _options;
    private readonly ITenantService _tenantService;

    public ScopedDbContextFactory(
        DbContextOptions<ApplicationDbContext> options,
        ITenantService tenantService)
    {
        _options = options;
        _tenantService = tenantService;
    }

    public ApplicationDbContext CreateDbContext()
        => new ApplicationDbContext(_options, _tenantService);
}
