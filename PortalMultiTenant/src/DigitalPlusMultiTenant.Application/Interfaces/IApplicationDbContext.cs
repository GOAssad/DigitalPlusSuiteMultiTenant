using DigitalPlusMultiTenant.Domain.Common;
using DigitalPlusMultiTenant.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlusMultiTenant.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Empresa> Empresas { get; }
    DbSet<Sucursal> Sucursales { get; }
    DbSet<Sector> Sectores { get; }
    DbSet<Categoria> Categorias { get; }
    DbSet<Horario> Horarios { get; }
    DbSet<HorarioDetalle> HorarioDetalles { get; }
    DbSet<Terminal> Terminales { get; }
    DbSet<Legajo> Legajos { get; }
    DbSet<LegajoSucursal> LegajoSucursales { get; }
    DbSet<LegajoHuella> LegajoHuellas { get; }
    DbSet<LegajoPin> LegajoPines { get; }
    DbSet<LegajoDomicilio> LegajoDomicilios { get; }
    DbSet<Fichada> Fichadas { get; }
    DbSet<Incidencia> Incidencias { get; }
    DbSet<IncidenciaLegajo> IncidenciaLegajos { get; }
    DbSet<Vacacion> Vacaciones { get; }
    DbSet<EventoCalendario> EventosCalendario { get; }
    DbSet<Feriado> Feriados { get; }
    DbSet<Noticia> Noticias { get; }
    DbSet<VariableSistema> VariablesSistema { get; }
    DbSet<UsuarioSucursal> UsuarioSucursales { get; }

    // Terminal Movil (v2)
    DbSet<TerminalMovil> TerminalesMoviles { get; }
    DbSet<SucursalGeoconfig> SucursalGeoconfigs { get; }
    DbSet<CodigoActivacionMovil> CodigosActivacionMovil { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
