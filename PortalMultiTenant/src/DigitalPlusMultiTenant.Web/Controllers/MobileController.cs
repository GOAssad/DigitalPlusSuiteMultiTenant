using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DigitalPlusMultiTenant.Application.Interfaces;
using DigitalPlusMultiTenant.Domain.Entities;
using DigitalPlusMultiTenant.Domain.Enums;
using DigitalPlusMultiTenant.Infrastructure.Services;
using DigitalPlusMultiTenant.Persistence;
using DigitalPlusMultiTenant.Web.Helpers;

namespace DigitalPlusMultiTenant.Web.Controllers;

[ApiController]
[Route("api/mobile")]
public class MobileController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly UbicacionService _ubicacionService;
    private readonly IConfiguration _config;
    private readonly ILicenciaService _licenciaService;

    public MobileController(
        ApplicationDbContext db,
        UbicacionService ubicacionService,
        IConfiguration config,
        ILicenciaService licenciaService)
    {
        _db = db;
        _ubicacionService = ubicacionService;
        _config = config;
        _licenciaService = licenciaService;
    }

    // ============================================================
    // POST /api/mobile/login
    // ============================================================
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Legajo) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { ok = false, mensaje = "Legajo y contraseña son requeridos." });

            // Determinar EmpresaId desde el dispositivo registrado (si existe)
            int? empresaIdFromDevice = null;
            var deviceId = Request.Headers["X-Device-Id"].FirstOrDefault();
            if (!string.IsNullOrEmpty(deviceId))
            {
                var terminalRegistrada = await _db.TerminalesMoviles
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(t => t.DeviceId == deviceId && t.Activo);
                if (terminalRegistrada != null)
                    empresaIdFromDevice = terminalRegistrada.EmpresaId;
            }

            // Buscar legajo filtrando por empresa cuando es posible
            Legajo? legajo = null;
            if (empresaIdFromDevice.HasValue)
            {
                // Dispositivo registrado: buscar solo en su empresa
                legajo = await _db.Legajos
                    .IgnoreQueryFilters()
                    .Include(l => l.Pin)
                    .Include(l => l.Empresa)
                    .FirstOrDefaultAsync(l => l.NumeroLegajo == request.Legajo
                                           && l.EmpresaId == empresaIdFromDevice.Value && l.IsActive);
            }
            else
            {
                // Dispositivo no registrado: intentar resolver empresa desde código de activación
                if (!string.IsNullOrWhiteSpace(request.CodigoActivacion))
                {
                    var codigoAct = await _db.CodigosActivacionMovil
                        .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(c => c.Codigo == request.CodigoActivacion
                                               && !c.Usado && c.FechaExpira > DateTime.UtcNow);
                    if (codigoAct != null)
                    {
                        legajo = await _db.Legajos
                            .IgnoreQueryFilters()
                            .Include(l => l.Pin)
                            .Include(l => l.Empresa)
                            .FirstOrDefaultAsync(l => l.NumeroLegajo == request.Legajo
                                                   && l.EmpresaId == codigoAct.EmpresaId && l.IsActive);
                    }
                }

                if (legajo == null)
                {
                    var legajos = await _db.Legajos
                        .IgnoreQueryFilters()
                        .Include(l => l.Pin)
                        .Include(l => l.Empresa)
                        .Where(l => l.NumeroLegajo == request.Legajo && l.IsActive)
                        .ToListAsync();

                    if (legajos.Count > 1)
                        return BadRequest(new { ok = false, mensaje = "Número de legajo existe en múltiples empresas. Registre el dispositivo primero." });

                    legajo = legajos.FirstOrDefault();
                }
            }

            if (legajo == null)
                return Unauthorized(new { ok = false, mensaje = "Legajo no encontrado o inactivo." });

            if (!legajo.Empresa.IsActive)
                return Unauthorized(new { ok = false, mensaje = "La empresa se encuentra suspendida." });

            if (!legajo.Empresa.MobileHabilitado)
                return Unauthorized(new { ok = false, mensaje = "La empresa no tiene habilitado el módulo móvil." });

            if (!legajo.MobileHabilitado)
                return Unauthorized(new { ok = false, mensaje = "Este legajo no tiene habilitado el acceso móvil." });

            // Verificar PIN como password
            if (legajo.Pin == null)
                return Unauthorized(new { ok = false, mensaje = "El empleado no tiene PIN configurado. Solicite al administrador." });

            string hashIngresado = ComputeHash(request.Password, legajo.Pin.PinSalt);
            if (!string.Equals(hashIngresado, legajo.Pin.PinHash, StringComparison.Ordinal))
                return Unauthorized(new { ok = false, mensaje = "Contraseña incorrecta." });

            // Verificar si el device ya está registrado
            bool dispositivoRegistrado = false;
            if (!string.IsNullOrEmpty(deviceId))
            {
                // Primero buscar por DeviceId exacto
                dispositivoRegistrado = await _db.TerminalesMoviles
                    .IgnoreQueryFilters()
                    .AnyAsync(t => t.DeviceId == deviceId && t.LegajoId == legajo.Id
                               && t.EmpresaId == legajo.EmpresaId && t.Activo);

                // Si no matchea pero el legajo tiene terminal activa, actualizar DeviceId
                // (el usuario cambió browser/borró cache/reinstalo PWA)
                if (!dispositivoRegistrado)
                {
                    var terminalExistente = await _db.TerminalesMoviles
                        .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(t => t.LegajoId == legajo.Id
                                           && t.EmpresaId == legajo.EmpresaId && t.Activo);
                    if (terminalExistente != null)
                    {
                        terminalExistente.DeviceId = deviceId;
                        terminalExistente.UltimoUso = DateTime.UtcNow;
                        await _db.SaveChangesAsync();
                        dispositivoRegistrado = true;
                    }
                }
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
        catch (Exception ex)
        {
            return StatusCode(500, new { ok = false, mensaje = $"Error interno: {ex.Message}" });
        }
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

        // Validar limite de terminales moviles del plan
        var terminalesActivas = await _db.TerminalesMoviles
            .IgnoreQueryFilters()
            .CountAsync(t => t.EmpresaId == empresaId && t.Activo);
        var (tmPermitido, tmMensaje) = await _licenciaService.PuedeRegistrarTerminalMovilAsync(empresaId, terminalesActivas);
        if (!tmPermitido)
            return StatusCode(403, new { ok = false, codigo = "LIMITE_TERMINALES", mensaje = tmMensaje });

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

        // 4. Obtener sucursales asignadas al legajo
        var sucursalesAsignadas = await _db.LegajoSucursales
            .IgnoreQueryFilters()
            .Where(ls => ls.LegajoId == legajoId)
            .Select(ls => ls.SucursalId)
            .ToListAsync();

        if (!sucursalesAsignadas.Any())
            return StatusCode(403, new { ok = false, codigo = "SIN_SUCURSAL", mensaje = "No tiene sucursales asignadas. Contacte a su administrador." });

        // 5. Resolver sucursal por ubicación (solo las asignadas al legajo)
        var geoConfigs = await _db.SucursalGeoconfigs
            .IgnoreQueryFilters()
            .Where(g => g.EmpresaId == empresaId && g.Activo && sucursalesAsignadas.Contains(g.SucursalId))
            .Join(_db.Sucursales.IgnoreQueryFilters(),
                  g => g.SucursalId, s => s.Id,
                  (g, s) => new UbicacionService.GeoConfigInfo(
                      s.Id, s.Nombre, g.WifiBSSID, g.Latitud, g.Longitud, g.RadioMetros, g.MetodoValidacion))
            .ToListAsync();

        if (!geoConfigs.Any())
            return StatusCode(403, new { ok = false, codigo = "SIN_GEOCONFIG", mensaje = "Las sucursales asignadas no tienen configuración GPS. Contacte a su administrador." });

        var ubicacion = _ubicacionService.ResolverSucursal(geoConfigs, request.WifiBSSID, request.Latitud, request.Longitud);
        if (!ubicacion.Ok)
            return StatusCode(403, new { ok = false, codigo = "UBICACION_INVALIDA", mensaje = ubicacion.Error });

        // 6. Determinar tipo (Entrada/Salida) usando hora Argentina
        var ahoraLocal = Clock.Now;
        string tipo = request.TipoFichada;
        if (string.Equals(tipo, "Auto", StringComparison.OrdinalIgnoreCase))
        {
            var hoy = Clock.Today;
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

        // 7. Validar limite de fichadas del plan
        var hace30d = Clock.Now.AddDays(-30);
        var fichadasUlt30d = await _db.Fichadas
            .IgnoreQueryFilters()
            .CountAsync(f => f.EmpresaId == empresaId && f.FechaHora >= hace30d);
        var (ficPermitido, ficMensaje) = await _licenciaService.PuedeRegistrarFichadaAsync(empresaId, fichadasUlt30d);
        if (!ficPermitido)
            return StatusCode(403, new { ok = false, codigo = "LIMITE_FICHADAS", mensaje = ficMensaje });

        // 8. Insertar fichada (FechaHora en hora local Argentina, consistente con desktop)
        var fichada = new Fichada
        {
            EmpresaId = empresaId,
            LegajoId = legajoId,
            SucursalId = ubicacion.SucursalId!.Value,
            FechaHora = ahoraLocal,
            Tipo = tipo,
            Origen = nameof(OrigenFichada.Movil),
            CreatedAt = DateTime.UtcNow
        };
        _db.Fichadas.Add(fichada);

        // 9. Actualizar último uso del terminal
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

        var hoy = Clock.Today;
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
    // POST /api/mobile/fichar-qr
    // Fichada por QR desde kiosko (web o desktop)
    // ============================================================
    [AllowAnonymous]
    [HttpPost("fichar-qr")]
    public async Task<IActionResult> FicharQR([FromBody] FicharQrRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.QrToken))
                return BadRequest(new { ok = false, mensaje = "QR token es requerido." });

            var deviceId = request.DeviceId ?? Request.Headers["X-Device-Id"].FirstOrDefault();
            if (string.IsNullOrEmpty(deviceId))
                return BadRequest(new { ok = false, codigo = "DISPOSITIVO_REQUERIDO", mensaje = "DeviceId es requerido." });

            // 1. Buscar terminal kiosko activa
            var terminal = await _db.TerminalesMoviles
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(t => t.DeviceId == deviceId && t.Activo && t.ModoKiosko);

            if (terminal == null)
                return StatusCode(403, new { ok = false, codigo = "KIOSKO_NO_ENCONTRADO", mensaje = "Dispositivo no registrado como kiosko o desactivado." });

            if (!terminal.SucursalId.HasValue)
                return StatusCode(403, new { ok = false, codigo = "KIOSKO_SIN_SUCURSAL", mensaje = "El kiosko no tiene sucursal asignada. Contacte al administrador." });

            // 2. Buscar legajo por QrToken (cross-tenant, luego validar empresa)
            var legajo = await _db.Legajos
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(l => l.QrToken == request.QrToken && l.IsActive);

            if (legajo == null)
                return NotFound(new { ok = false, codigo = "QR_INVALIDO", mensaje = "QR no reconocido o legajo inactivo." });

            // 3. Validar misma empresa (barrera multi-tenant)
            if (legajo.EmpresaId != terminal.EmpresaId)
                return StatusCode(403, new { ok = false, codigo = "QR_INVALIDO", mensaje = "QR no reconocido o legajo inactivo." });

            // 4. Validar legajo asignado a la sucursal del kiosko
            var enSucursal = await _db.LegajoSucursales
                .IgnoreQueryFilters()
                .AnyAsync(ls => ls.LegajoId == legajo.Id && ls.SucursalId == terminal.SucursalId.Value);

            if (!enSucursal)
                return StatusCode(403, new { ok = false, codigo = "SUCURSAL_NO_ASIGNADA",
                    mensaje = "No está habilitado para fichar en esta sucursal." });

            // 5. Cooldown: evitar doble escaneo (30 segundos, compara contra CreatedAt que es UTC)
            var ahora = Clock.Now;
            var cooldownUtc = DateTime.UtcNow.AddSeconds(-30);
            var fichadaReciente = await _db.Fichadas
                .IgnoreQueryFilters()
                .AnyAsync(f => f.LegajoId == legajo.Id && f.EmpresaId == legajo.EmpresaId
                            && f.CreatedAt > cooldownUtc);
            if (fichadaReciente)
                return BadRequest(new { ok = false, codigo = "COOLDOWN", mensaje = "Ya se registró una fichada hace menos de 30 segundos." });

            // 6. Determinar tipo (Entrada/Salida) automático
            var hoy = ahora.Date;
            var ultimaFichada = await _db.Fichadas
                .IgnoreQueryFilters()
                .Where(f => f.LegajoId == legajo.Id && f.EmpresaId == legajo.EmpresaId && f.FechaHora >= hoy)
                .OrderByDescending(f => f.FechaHora)
                .FirstOrDefaultAsync();

            var tipo = ultimaFichada?.Tipo == "E" ? "S" : "E";

            // 7. Validar limite de fichadas del plan
            var hace30d = DateTime.UtcNow.AddDays(-30);
            var fichadasUlt30d = await _db.Fichadas
                .IgnoreQueryFilters()
                .CountAsync(f => f.EmpresaId == legajo.EmpresaId && f.FechaHora >= hace30d);
            var (ficPermitido, ficMensaje) = await _licenciaService.PuedeRegistrarFichadaAsync(legajo.EmpresaId, fichadasUlt30d);
            if (!ficPermitido)
                return StatusCode(403, new { ok = false, codigo = "LIMITE_FICHADAS", mensaje = ficMensaje });

            // 8. Insertar fichada
            var fichada = new Fichada
            {
                EmpresaId = legajo.EmpresaId,
                LegajoId = legajo.Id,
                SucursalId = terminal.SucursalId.Value,
                FechaHora = ahora,
                Tipo = tipo,
                Origen = nameof(OrigenFichada.QR),
                CreatedAt = DateTime.UtcNow
            };
            _db.Fichadas.Add(fichada);

            // 9. Actualizar último uso del terminal
            terminal.UltimoUso = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            // 10. Obtener nombre sucursal para respuesta
            var sucursalNombre = await _db.Sucursales
                .IgnoreQueryFilters()
                .Where(s => s.Id == terminal.SucursalId.Value)
                .Select(s => s.Nombre)
                .FirstOrDefaultAsync();

            return Ok(new
            {
                ok = true,
                fichadaId = fichada.Id,
                legajoId = legajo.Id,
                nombre = $"{legajo.Apellido}, {legajo.Nombre}",
                foto = legajo.Foto != null ? Convert.ToBase64String(legajo.Foto) : null,
                tipo = tipo == "E" ? "Entrada" : "Salida",
                sucursalNombre,
                fechaHora = fichada.FechaHora,
                mensaje = $"{(tipo == "E" ? "Entrada" : "Salida")} registrada correctamente."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { ok = false, mensaje = $"Error interno: {ex.Message}" });
        }
    }

    // ============================================================
    // GET /api/mobile/mi-qr
    // Devuelve el QR token del empleado logueado (para mostrar en PWA)
    // ============================================================
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("mi-qr")]
    public async Task<IActionResult> MiQR()
    {
        var (empresaId, legajoId) = ExtraerClaims();
        if (legajoId == 0)
            return Unauthorized(new { ok = false, mensaje = "Token inválido." });

        var legajo = await _db.Legajos
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(l => l.Id == legajoId && l.EmpresaId == empresaId);

        if (legajo == null)
            return NotFound(new { ok = false, mensaje = "Legajo no encontrado." });

        // Auto-generar QrToken si no tiene
        if (string.IsNullOrEmpty(legajo.QrToken))
        {
            legajo.QrToken = Guid.NewGuid().ToString("N");
            await _db.SaveChangesAsync();
        }

        return Ok(new
        {
            ok = true,
            qrToken = legajo.QrToken,
            nombre = $"{legajo.Apellido}, {legajo.Nombre}",
            legajoId = legajo.Id
        });
    }

    // ============================================================
    // GET /api/mobile/kiosko-info
    // Info del kiosko para la UI (nombre empresa, sucursal, etc)
    // ============================================================
    [AllowAnonymous]
    [HttpGet("kiosko-info")]
    public async Task<IActionResult> KioskoInfo()
    {
        var deviceId = Request.Headers["X-Device-Id"].FirstOrDefault();
        if (string.IsNullOrEmpty(deviceId))
            return BadRequest(new { ok = false, mensaje = "Header X-Device-Id es requerido." });

        var terminal = await _db.TerminalesMoviles
            .IgnoreQueryFilters()
            .Include(t => t.Empresa)
            .Include(t => t.Sucursal)
            .FirstOrDefaultAsync(t => t.DeviceId == deviceId && t.Activo && t.ModoKiosko);

        if (terminal == null)
            return NotFound(new { ok = false, mensaje = "Kiosko no encontrado o desactivado." });

        return Ok(new
        {
            ok = true,
            empresaNombre = terminal.Empresa.Nombre,
            sucursalNombre = terminal.Sucursal?.Nombre,
            sucursalId = terminal.SucursalId,
            terminalNombre = terminal.Nombre
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

    public record LoginRequest(string Legajo, string Password, string? CodigoActivacion = null);

    public record RegistrarDispositivoRequest(
        string Codigo,
        string DeviceId,
        string PublicKey,
        string? NombreDispositivo,
        string? Plataforma);

    public record FicharQrRequest(string QrToken, string? DeviceId);

    public record FichadaRequest(
        DateTime Timestamp,
        string? WifiBSSID,
        string? WifiSSID,
        decimal? Latitud,
        decimal? Longitud,
        string TipoFichada);
}
