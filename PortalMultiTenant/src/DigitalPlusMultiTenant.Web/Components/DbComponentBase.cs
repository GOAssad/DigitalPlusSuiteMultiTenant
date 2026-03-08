using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using DigitalPlusMultiTenant.Persistence;

namespace DigitalPlusMultiTenant.Web.Components;

/// <summary>
/// Base class for Blazor components that need database access.
/// Each component gets its own DbContext instance, avoiding concurrency issues.
/// </summary>
public class DbComponentBase : ComponentBase, IAsyncDisposable
{
    [Inject] protected IDbContextFactory<ApplicationDbContext> DbFactory { get; set; } = null!;

    private ApplicationDbContext? _db;
    protected ApplicationDbContext Db => _db ??= DbFactory.CreateDbContext();

    public virtual async ValueTask DisposeAsync()
    {
        if (_db != null)
            await _db.DisposeAsync();
    }
}
