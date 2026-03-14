using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using DigitalPlusMultiTenant.Persistence;

namespace DigitalPlusMultiTenant.Web.Components;

/// <summary>
/// Base class for Blazor components that need database access.
/// Each component gets its own DbContext instance, avoiding concurrency issues.
/// Handles disposal gracefully to prevent "connection closed" errors on navigation.
/// </summary>
public class DbComponentBase : ComponentBase, IAsyncDisposable
{
    [Inject] protected IDbContextFactory<ApplicationDbContext> DbFactory { get; set; } = null!;

    private ApplicationDbContext? _db;
    private bool _disposed;

    protected ApplicationDbContext Db
    {
        get
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(DbComponentBase));
            return _db ??= DbFactory.CreateDbContext();
        }
    }

    protected bool IsDisposed => _disposed;

    public virtual async ValueTask DisposeAsync()
    {
        _disposed = true;
        if (_db != null)
            await _db.DisposeAsync();
    }
}
