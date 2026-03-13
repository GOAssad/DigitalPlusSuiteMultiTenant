using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DigitalPlusMultiTenant.Domain.Entities;
using DigitalPlusMultiTenant.Domain.Enums;
using DigitalPlusMultiTenant.Infrastructure.Services;
using DigitalPlusMultiTenant.Persistence;

namespace DigitalPlusMultiTenant.Web.Controllers;

[ApiController]
[Route("api/mobile")]
public class MobileController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly UbicacionService _ubicacionService;
    private readonly IConfiguration _config;

    public MobileController(
        ApplicationDbContext db,
        UbicacionService ubicacionService,
        IConfiguration config)
    {
        _db = db;
        _ubicacionService = ubicacionService;
        _config = config;
    }

    // ============================================================
    // POST /api/mobile/login
    // ============================================================
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Legajo) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { ok = false, mensaje = "Legajo y contraseña son requeridos." });

        // Buscar legajo sin query filter (necesitamos EmpresaId)
        var legajo = await _db.Legajos
            .IgnoreQueryFilters()
            .Include(l => l.Pin)
            .Include(l => l.Empresa)
            .FirstOrDefaultAsync(l => l.NumeroLegajo == request.Legajo && l.IsActive);

        if (legajo == null)
            return Unauthorized(new { ok = false, mensaje = "Legajo no encontrado o inactivo." });

        if (!legajo.Empresa.IsActive)
            return Unauthorized(new { ok = false, mensaje = "La empresa se encuentra suspendida." });

        // Verificar PIN como password
        if (legajo.Pin == null)
            return Unauthorized(new { ok = false, mensaje = "El empleado no tiene PIN configurado. Solicite al administrador." });

        string hashIngresado = ComputeHash(request.Password, legajo.Pin.PinSalt);
        if (!string.Equals(hashIngresado, legajo.Pin.PinHash, StringComparison.Ordinal))
            return Unauthorized(new { ok = false, mensaje = "Contraseña incorrecta." });

        // Verificar si el device ya está registrado
        var deviceId = Request.Headers["X-Device-Id"].FirstOrDefault();
        bool dispositivoRegistrado = false;
        if (!string.IsNullOrEmpty(deviceId))
        {
            dispositivoRegistrado = await _db.TerminalesMoviles
                .IgnoreQueryFilters()
                .AnyAsync(t => t.DeviceId == deviceId && t.LegajoId == legajo.Id
                           && t.EmpresaId == legajo.EmpresaId && t.Activo);
        }

        // Generar JWT
        var token = GenerarToken(legajo);

        return Ok(new
        {
            ok = true,
            token,
            legajoId = legajo.Id,
            nombreEmpleado = $"{legajo.Apellido}, {legajo.Nombre}",
            empresaId = legajo.EmpresaId,
            nombreEmpresa = legajo.Empresa.Nombre,
            dispositivoRegistrado
        });
    }

    // ============================================================
    // POST /api/mobile/registrar-dispositivo
    // ============================================================
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("registrar-dispositivo")]
    public async Task<IActionResult> RegistrarDispositivo([FromBody] RegistrarDispositivoRequest request)
    {
        var (empresaId, legajoId) = ExtraerClaims();
        if (legajoId == 0)
            return Unauthorized(new { ok = false, mensaje = "Token inválido." });

        // Buscar código de activación válido
        var codigo = await _db.CodigosActivacionMovil
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Codigo == request.Codigo
                                   && c.EmpresaId == empresaId
                                   && !c.Usado
                                   && c.FechaExpira > DateTime.UtcNow);

        if (codigo == null)
            return BadRequest(new { ok = false, mensaje = "Código inválido, expirado o ya utilizado." });

        if (codigo.LegajoId != legajoId)
            return BadRequest(new { ok = false, mensaje = "El código no corresponde a este empleado." });

        // Desactivar dispositivos anteriores del mismo legajo
        var anteriores = await _db.TerminalesMoviles
            .IgnoreQueryFilters()
            .Where(t => t.LegajoId == legajoId && t.EmpresaId == empresaId && t.Activo)
            .ToListAsync();
        foreach (var ant in anteriores)
            ant.Activo = false;

        // Registrar nuevo dispositivo
        var terminal = new TerminalMovil
        {
            EmpresaId = empresaId,
            LegajoId = legajoId,
            DeviceId = request.DeviceId,
            PublicKey = request.PublicKey,
            Nombre = request.NombreDispositivo,
            Plataforma = request.Plataforma,
            FechaRegistro = DateTime.UtcNow,
            Activo = true
        };
        _db.TerminalesMoviles.Add(terminal);

        // Marcar código como usado
        codigo.Usado = true;
        codigo.UsadoEn = DateTime.UtcNow;
        codigo.DeviceId = request.DeviceId;

        await _db.SaveChangesAsync();

        return Ok(new { ok = true, mensaje = "Dispositivo registrado correctamente." });
    }

    // ============================================================
    // POST /api/mobile/fichada
    // ============================================================
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("fichada")]
    public async Task<IActionResult> Fichada([FromBody] FichadaRequest request)
    {
        var (empresaId, legajoId) = ExtraerClaims();
        if (legajoId == 0)
            return Unauthorized(new { ok = false, mensaje = "Token inválido." });

        // 1. Buscar terminal activa
        var deviceId = Request.Headers["X-Device-Id"].FirstOrDefault();
        if (string.IsNullOrEmpty(deviceId))
            return BadRequest(new { ok = false, codigo = "DISPOSITIVO_REQUERIDO", mensaje = "Header X-Device-Id es requerido." });

        var terminal = await _db.TerminalesMoviles
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(t => t.DeviceId == deviceId && t.LegajoId == legajoId
                                   && t.EmpresaId == empresaId && t.Activo);

        if (terminal == null)
            return StatusCode(403, new { ok = false, codigo = "DISPOSITIVO_NO_REGISTRADO", mensaje = "Dispositivo no registrado o desactivado." });

        // 2. Verificar firma RSA del body (X-Signature header)
        var signatureHeader = Request.Headers["X-Signature"].FirstOrDefault();
        if (!string.IsNullOrEmpty(signatureHeader))
        {
            // Si se envía firma, validarla
            if (!VerificarFirma(request, terminal.PublicKey, signatureHeader))
                return StatusCode(403, new { ok = false, codigo = "FIRMA_INVALIDA", mensaje = "La firma del request es inválida." });
        }

        // 3. Verificar timestamp
        var diff = Math.Abs((DateTime.UtcNow - request.Timestamp).TotalMinutes);
        if (diff > 5)
            return BadRequest(new { ok = false, codigo = "TIMESTAMP_INVALIDO", mensaje = "El timestamp tiene más de 5 minutos de diferencia con el servidor." });

        // 4. Resolver sucursal por ubicación
        var geoConfigs = await _db.SucursalGeoconfigs
            .IgnoreQueryFilters()
            .Where(g => g.EmpresaId == empresaId && g.Activo)
            .Join(_db.Sucursales.IgnoreQueryFilters(),
                  g => g.SucursalId, s => s.Id,
                  (g, s) => new UbicacionService.GeoConfigInfo(
                      s.Id, s.Nombre, g.WifiBSSID, g.Latitud, g.Longitud, g.RadioMetros, g.MetodoValidacion))
            .ToListAsync();

        var ubicacion = _ubicacionService.ResolverSucursal(geoConfigs, request.WifiBSSID, request.Latitud, request.Longitud);
        if (!ubicacion.Ok)
            return StatusCode(403, new { ok = false, codigo = "UBICACION_INVALIDA", mensaje = ubicacion.Error });

        // 5. Determinar tipo (Entrada/Salida)
        string tipo = request.TipoFichada;
        if (string.Equals(tipo, "Auto", StringComparison.OrdinalIgnoreCase))
        {
            var hoy = DateTime.UtcNow.Date;
            var ultimaFichada = await _db.Fichadas
                .IgnoreQueryFilters()
                .Where(f => f.LegajoId == legajoId && f.EmpresaId == empresaId && f.FechaHora >= hoy)
                .OrderByDescending(f => f.FechaHora)
                .FirstOrDefaultAsync();

            tipo = ultimaFichada?.Tipo == "E" ? "S" : "E";
        }
        else
        {
            tipo = string.Equals(tipo, "Salida", StringComparison.OrdinalIgnoreCase) ? "S" : "E";
        }

        // 6. Insertar fichada
        var fichada = new Fichada
        {
            EmpresaId = empresaId,
            LegajoId = legajoId,
            SucursalId = ubicacion.SucursalId!.Value,
            FechaHora = request.Timestamp,
            Tipo = tipo,
            Origen = OrigenFichada.Movil,
            CreatedAt = DateTime.UtcNow
        };
        _db.Fichadas.Add(fichada);

        // 7. Actualizar último uso del terminal
        terminal.UltimoUso = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok(new
        {
            ok = true,
            fichadaId = fichada.Id,
            tipo = tipo == "E" ? "Entrada" : "Salida",
            sucursalId = ubicacion.SucursalId,
            sucursalNombre = ubicacion.SucursalNombre,
            fechaHora = fichada.FechaHora,
            mensaje = $"{(tipo == "E" ? "Entrada" : "Salida")} registrada correctamente."
        });
    }

    // ============================================================
    // GET /api/mobile/estado
    // ============================================================
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("estado")]
    public async Task<IActionResult> Estado()
    {
        var (empresaId, legajoId) = ExtraerClaims();
        if (legajoId == 0)
            return Unauthorized(new { ok = false, mensaje = "Token inválido." });

        var legajo = await _db.Legajos
            .IgnoreQueryFilters()
            .Include(l => l.Empresa)
            .FirstOrDefaultAsync(l => l.Id == legajoId && l.EmpresaId == empresaId);

        if (legajo == null)
            return NotFound(new { ok = false, mensaje = "Legajo no encontrado." });

        var deviceId = Request.Headers["X-Device-Id"].FirstOrDefault();
        bool dispositivoActivo = false;
        if (!string.IsNullOrEmpty(deviceId))
        {
            dispositivoActivo = await _db.TerminalesMoviles
                .IgnoreQueryFilters()
                .AnyAsync(t => t.DeviceId == deviceId && t.LegajoId == legajoId
                           && t.EmpresaId == empresaId && t.Activo);
        }

        var hoy = DateTime.UtcNow.Date;
        var fichadasHoy = await _db.Fichadas
            .IgnoreQueryFilters()
            .Where(f => f.LegajoId == legajoId && f.EmpresaId == empresaId && f.FechaHora >= hoy)
            .OrderBy(f => f.FechaHora)
            .Select(f => new { tipo = f.Tipo == "E" ? "Entrada" : "Salida", fechaHora = f.FechaHora })
            .ToListAsync();

        var ultima = fichadasHoy.LastOrDefault();

        return Ok(new
        {
            ok = true,
            legajoId,
            nombre = $"{legajo.Apellido}, {legajo.Nombre}",
            empresaNombre = legajo.Empresa.Nombre,
            dispositivoActivo,
            ultimaFichada = ultima,
            fichadasHoy
        });
    }

    // ============================================================
    // Helpers
    // ============================================================

    private string GenerarToken(Legajo legajo)
    {
        var jwtConfig = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("legajoId", legajo.Id.ToString()),
            new Claim("empresaId", legajo.EmpresaId.ToString()),
            new Claim("nombre", $"{legajo.Apellido}, {legajo.Nombre}"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expireHours = int.TryParse(jwtConfig["ExpireHours"], out var h) ? h : 8;
        var token = new JwtSecurityToken(
            issuer: jwtConfig["Issuer"],
            audience: jwtConfig["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expireHours),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private (int empresaId, int legajoId) ExtraerClaims()
    {
        var empresaId = int.TryParse(User.FindFirst("empresaId")?.Value, out var e) ? e : 0;
        var legajoId = int.TryParse(User.FindFirst("legajoId")?.Value, out var l) ? l : 0;
        return (empresaId, legajoId);
    }

    private static string ComputeHash(string pin, string salt)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(salt + pin);
        byte[] hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }

    private static bool VerificarFirma(FichadaRequest request, string publicKeyPem, string signatureBase64)
    {
        try
        {
            var json = System.Text.Json.JsonSerializer.Serialize(request);
            var data = Encoding.UTF8.GetBytes(json);
            var signature = Convert.FromBase64String(signatureBase64);

            using var rsa = RSA.Create();
            rsa.ImportFromPem(publicKeyPem.ToCharArray());
            return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
        catch
        {
            return false;
        }
    }

    // ============================================================
    // Request DTOs
    // ============================================================

    public record LoginRequest(string Legajo, string Password);

    public record RegistrarDispositivoRequest(
        string Codigo,
        string DeviceId,
        string PublicKey,
        string? NombreDispositivo,
        string? Plataforma);

    public record FichadaRequest(
        DateTime Timestamp,
        string? WifiBSSID,
        string? WifiSSID,
        decimal? Latitud,
        decimal? Longitud,
        string TipoFichada);
}
