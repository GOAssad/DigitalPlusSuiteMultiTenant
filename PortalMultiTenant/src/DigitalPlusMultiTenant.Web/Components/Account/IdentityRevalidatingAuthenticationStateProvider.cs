using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using DigitalPlusMultiTenant.Domain.Entities;

namespace DigitalPlusMultiTenant.Web.Components.Account;

// Server-side AuthenticationStateProvider that revalidates the security stamp
// and verifies the empresa is still active every minute.
internal sealed class IdentityRevalidatingAuthenticationStateProvider(
        ILoggerFactory loggerFactory,
        IServiceScopeFactory scopeFactory,
        IOptions<IdentityOptions> options,
        IConfiguration configuration)
    : RevalidatingServerAuthenticationStateProvider(loggerFactory)
{
    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(1);

    protected override async Task<bool> ValidateAuthenticationStateAsync(
        AuthenticationState authenticationState, CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (!await ValidateSecurityStampAsync(userManager, authenticationState.User))
            return false;

        if (!await ValidateEmpresaActivaAsync(userManager, authenticationState.User))
            return false;

        return true;
    }

    private async Task<bool> ValidateSecurityStampAsync(UserManager<ApplicationUser> userManager, ClaimsPrincipal principal)
    {
        var user = await userManager.GetUserAsync(principal);
        if (user is null)
        {
            return false;
        }
        else if (!userManager.SupportsUserSecurityStamp)
        {
            return true;
        }
        else
        {
            var principalStamp = principal.FindFirstValue(options.Value.ClaimsIdentity.SecurityStampClaimType);
            var userStamp = await userManager.GetSecurityStampAsync(user);
            return principalStamp == userStamp;
        }
    }

    private async Task<bool> ValidateEmpresaActivaAsync(UserManager<ApplicationUser> userManager, ClaimsPrincipal principal)
    {
        try
        {
            var user = await userManager.GetUserAsync(principal);
            if (user is null) return false;

            var connStr = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connStr)) return true;

            await using var conn = new SqlConnection(connStr);
            await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT IsActive FROM Empresa WHERE Id = @Id";
            cmd.Parameters.AddWithValue("@Id", user.EmpresaId);
            cmd.CommandTimeout = 5;

            var result = await cmd.ExecuteScalarAsync();
            return result is true;
        }
        catch
        {
            return true; // Fail-open: si no se puede verificar, permitir acceso
        }
    }
}
