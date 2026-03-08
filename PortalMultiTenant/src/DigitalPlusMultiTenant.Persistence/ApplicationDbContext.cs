using DigitalPlusMultiTenant.Application.Interfaces;
using DigitalPlusMultiTenant.Domain.Common;
using DigitalPlusMultiTenant.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlusMultiTenant.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly ITenantService _tenantService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ITenantService tenantService)
        : base(options)
    {
        _tenantService = tenantService;
    }

    // Entidad raiz
    public DbSet<Empresa> Empresas => Set<Empresa>();

    // Estructura organizativa
    public DbSet<Sucursal> Sucursales => Set<Sucursal>();
    public DbSet<Sector> Sectores => Set<Sector>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Horario> Horarios => Set<Horario>();
    public DbSet<HorarioDetalle> HorarioDetalles => Set<HorarioDetalle>();
    public DbSet<Terminal> Terminales => Set<Terminal>();

    // Personas
    public DbSet<Legajo> Legajos => Set<Legajo>();
    public DbSet<LegajoSucursal> LegajoSucursales => Set<LegajoSucursal>();
    public DbSet<LegajoHuella> LegajoHuellas => Set<LegajoHuella>();
    public DbSet<LegajoPin> LegajoPines => Set<LegajoPin>();
    public DbSet<LegajoDomicilio> LegajoDomicilios => Set<LegajoDomicilio>();

    // Operacion
    public DbSet<Fichada> Fichadas => Set<Fichada>();
    public DbSet<Incidencia> Incidencias => Set<Incidencia>();
    public DbSet<IncidenciaLegajo> IncidenciaLegajos => Set<IncidenciaLegajo>();
    public DbSet<Vacacion> Vacaciones => Set<Vacacion>();
    public DbSet<EventoCalendario> EventosCalendario => Set<EventoCalendario>();
    public DbSet<Feriado> Feriados => Set<Feriado>();
    public DbSet<Noticia> Noticias => Set<Noticia>();
    public DbSet<VariableSistema> VariablesSistema => Set<VariableSistema>();

    // Seguridad
    public DbSet<UsuarioSucursal> UsuarioSucursales => Set<UsuarioSucursal>();

    // Propiedad para query filters - EF Core la evalua en cada query, no en OnModelCreating
    private int CurrentEmpresaId => _tenantService.EmpresaId;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Aplicar todas las configuraciones del assembly
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Global query filters para tenant isolation
        // Usan CurrentEmpresaId (propiedad del DbContext) para evaluacion diferida
        builder.Entity<Sucursal>().HasQueryFilter(e => e.EmpresaId == CurrentEmpresaId);
        builder.Entity<Sector>().HasQueryFilter(e => e.EmpresaId == CurrentEmpresaId);
        builder.Entity<Categoria>().HasQueryFilter(e => e.EmpresaId == CurrentEmpresaId);
        builder.Entity<Horario>().HasQueryFilter(e => e.EmpresaId == CurrentEmpresaId);
        builder.Entity<Terminal>().HasQueryFilter(e => e.EmpresaId == CurrentEmpresaId);
        builder.Entity<Legajo>().HasQueryFilter(e => e.EmpresaId == CurrentEmpresaId);
        builder.Entity<Fichada>().HasQueryFilter(e => e.EmpresaId == CurrentEmpresaId);
        builder.Entity<Incidencia>().HasQueryFilter(e => e.EmpresaId == CurrentEmpresaId);
        builder.Entity<IncidenciaLegajo>().HasQueryFilter(e => e.EmpresaId == CurrentEmpresaId);
        builder.Entity<Vacacion>().HasQueryFilter(e => e.EmpresaId == CurrentEmpresaId);
        builder.Entity<EventoCalendario>().HasQueryFilter(e => e.EmpresaId == CurrentEmpresaId);
        builder.Entity<Feriado>().HasQueryFilter(e => e.EmpresaId == CurrentEmpresaId);
        builder.Entity<Noticia>().HasQueryFilter(e => e.EmpresaId == CurrentEmpresaId);
        builder.Entity<VariableSistema>().HasQueryFilter(e => e.EmpresaId == CurrentEmpresaId);
        // No filtrar ApplicationUser: Identity necesita acceso sin restriccion para login
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var empresaId = _tenantService.EmpresaId;
        var now = DateTime.UtcNow;
        var userName = _tenantService.UserName;

        foreach (var entry in ChangeTracker.Entries())
        {
            // Auto-set EmpresaId en entidades tenant (solo si no fue seteado manualmente)
            if (entry.Entity is ITenantEntity tenantEntity && entry.State == EntityState.Added
                && tenantEntity.EmpresaId == 0 && empresaId > 0)
            {
                tenantEntity.EmpresaId = empresaId;
            }

            // Auditoria automatica
            if (entry.Entity is AuditableEntity auditable)
            {
                if (entry.State == EntityState.Added)
                {
                    auditable.CreatedAt = now;
                    auditable.CreatedBy = userName;
                }
                else if (entry.State == EntityState.Modified)
                {
                    auditable.UpdatedAt = now;
                    auditable.UpdatedBy = userName;
                }
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
