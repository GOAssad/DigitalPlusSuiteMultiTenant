using System.Security.Claims;
using DigitalPlusMultiTenant.Application.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;

namespace DigitalPlusMultiTenant.Infrastructure.Services;

public class TenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AuthenticationStateProvider? _authStateProvider;

    public TenantService(
        IHttpContextAccessor httpContextAccessor,
        AuthenticationStateProvider? authStateProvider = null)
    {
        _httpContextAccessor = httpContextAccessor;
        _authStateProvider = authStateProvider;
    }

    private ClaimsPrincipal? GetUser()
    {
        // SSR: HttpContext disponible
        var httpUser = _httpContextAccessor.HttpContext?.User;
        if (httpUser?.Identity?.IsAuthenticated == true)
            return httpUser;

        // Interactive Server (SignalR): usar AuthenticationStateProvider
        if (_authStateProvider != null)
        {
            try
            {
                var authState = _authStateProvider.GetAuthenticationStateAsync()
                    .ConfigureAwait(false).GetAwaiter().GetResult();
                if (authState.User?.Identity?.IsAuthenticated == true)
                    return authState.User;
            }
            catch
            {
                // Fuera de scope de Razor component (model creation, seed, etc.)
            }
        }

        return null;
    }

    public int EmpresaId
    {
        get
        {
            var claim = GetUser()?.FindFirst("EmpresaId");
            return claim != null && int.TryParse(claim.Value, out var id) ? id : 0;
        }
    }

    public string? EmpresaNombre =>
        GetUser()?.FindFirst("EmpresaNombre")?.Value;

    public bool MobileHabilitado =>
        GetUser()?.FindFirst("MobileHabilitado")?.Value == "true";

    public bool KioskoHabilitado =>
        GetUser()?.FindFirst("KioskoHabilitado")?.Value == "true";

    public string? UserId =>
        GetUser()?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public string? UserName =>
        GetUser()?.Identity?.Name;
}
