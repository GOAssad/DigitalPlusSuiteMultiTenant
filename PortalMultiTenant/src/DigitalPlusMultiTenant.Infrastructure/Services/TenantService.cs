using System.Security.Claims;
using DigitalPlusMultiTenant.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DigitalPlusMultiTenant.Infrastructure.Services;

public class TenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int EmpresaId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("EmpresaId");
            return claim != null && int.TryParse(claim.Value, out var id) ? id : 0;
        }
    }

    public string? UserId =>
        _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public string? UserName =>
        _httpContextAccessor.HttpContext?.User?.Identity?.Name;
}
