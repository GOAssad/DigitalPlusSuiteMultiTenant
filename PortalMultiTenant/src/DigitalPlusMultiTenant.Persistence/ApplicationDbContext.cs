using System.Text.Json;
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
    public DbSet<SolicitudSoporte> SolicitudesSoporte => Set<SolicitudSoporte>();

    // Terminal Movil (v2)
    public DbSet<TerminalMovil> TerminalesMoviles => Set<TerminalMovil>();
    public DbSet<SucursalGeoconfig> SucursalGeoconfigs => Set<SucursalGeoconfig>();
    public DbSet<CodigoActivacionMovil> CodigosActivacionMovil => Set<CodigoActivacionMovil>();

    // Seguridad
    public DbSet<UsuarioSucursal> UsuarioSucursales => Set<UsuarioSucursal>();

    // Auditoria
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

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
        builder.Entity<TerminalMovil>().HasQueryFilter(e => e.EmpresaId == CurrentEmpresaId);
        builder.Entity<SucursalGeoconfig>().HasQueryFilter(e => e.EmpresaId == CurrentEmpresaId);
        builder.Entity<CodigoActivacionMovil>().HasQueryFilter(e => e.EmpresaId == CurrentEmpresaId);
        builder.Entity<SolicitudSoporte>().HasQueryFilter(e => e.EmpresaId == CurrentEmpresaId);

        // QrToken: indice unico filtrado (solo no-nulos)
        builder.Entity<Legajo>()
            .HasIndex(l => l.QrToken)
            .IsUnique()
            .HasFilter("[QrToken] IS NOT NULL");

        builder.Entity<Legajo>()
            .Property(l => l.QrToken)
            .HasMaxLength(64);

        // TerminalMovil: FK opcional a Sucursal (para modo kiosko)
        builder.Entity<TerminalMovil>()
            .HasOne(t => t.Sucursal)
            .WithMany()
            .HasForeignKey(t => t.SucursalId)
            .OnDelete(DeleteBehavior.SetNull);

        // Precision para coordenadas GPS
        builder.Entity<SucursalGeoconfig>().Property(e => e.Latitud).HasColumnType("decimal(10,7)");
        builder.Entity<SucursalGeoconfig>().Property(e => e.Longitud).HasColumnType("decimal(10,7)");
        // AuditLog: sin query filter (se filtra manualmente por EmpresaId en las queries)
        builder.Entity<AuditLog>(e =>
        {
            e.ToTable("AuditLog");
            e.HasKey(a => a.Id);
            e.Property(a => a.Id).UseIdentityColumn();
            e.Property(a => a.Action).HasMaxLength(50);
            e.Property(a => a.EntityType).HasMaxLength(100);
            e.Property(a => a.EntityId).HasMaxLength(50);
            e.Property(a => a.Description).HasMaxLength(500);
            e.Property(a => a.UserId).HasMaxLength(450);
            e.Property(a => a.UserName).HasMaxLength(256);
            e.Property(a => a.IpAddress).HasMaxLength(45);
            e.Property(a => a.Source).HasMaxLength(30);
            e.Property(a => a.Timestamp).HasColumnType("datetime2(3)");
        });

        // No filtrar ApplicationUser: Identity necesita acceso sin restriccion para login
    }

    // Propiedades para inyeccion de IP/Source desde el servicio de auditoria
    internal string? AuditIpAddress { get; set; }
    internal string? AuditSource { get; set; }

    private static readonly JsonSerializerOptions _jsonOpts = new()
    {
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    // Tipos que NO se auditan (evitar recursion y ruido)
    private static readonly HashSet<Type> _excludedTypes = new()
    {
        typeof(AuditLog),
        typeof(Noticia),        // Noticias son informativas, no datos criticos
        typeof(VariableSistema) // Variables de sistema cambian frecuentemente
    };

    // Propiedades que se excluyen del diff (sensibles o ruidosas)
    private static readonly HashSet<string> _excludedProperties = new()
    {
        "CreatedAt", "CreatedBy", "UpdatedAt", "UpdatedBy",
        "PinHash", "PinSalt", // Nunca loguear hashes
        "HuellaData", "Foto"  // Datos binarios grandes
    };

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var empresaId = _tenantService.EmpresaId;
        var now = DateTime.UtcNow;
        var userName = _tenantService.UserName;
        var userId = _tenantService.UserId;

        var auditEntries = new List<AuditEntry>();

        foreach (var entry in ChangeTracker.Entries())
        {
            // Auto-set EmpresaId en entidades tenant (solo si no fue seteado manualmente)
            if (entry.Entity is ITenantEntity tenantEntity && entry.State == EntityState.Added
                && tenantEntity.EmpresaId == 0 && empresaId > 0)
            {
                tenantEntity.EmpresaId = empresaId;
            }

            // Auditoria automatica de campos
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

            // Capturar cambios para AuditLog
            if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;
            if (_excludedTypes.Contains(entry.Entity.GetType()))
                continue;
            if (entry.Entity is not ITenantEntity && entry.Entity is not BaseEntity)
                continue; // Solo auditar entidades de dominio

            var auditEntry = new AuditEntry
            {
                EntityType = entry.Entity.GetType().Name,
                Action = entry.State switch
                {
                    EntityState.Added => "Create",
                    EntityState.Modified => "Update",
                    EntityState.Deleted => "Delete",
                    _ => entry.State.ToString()
                },
                EmpresaId = entry.Entity is ITenantEntity te ? te.EmpresaId : empresaId
            };

            foreach (var prop in entry.Properties)
            {
                if (prop.Metadata.IsPrimaryKey())
                {
                    auditEntry.EntityId = prop.CurrentValue?.ToString();
                    continue;
                }

                if (_excludedProperties.Contains(prop.Metadata.Name))
                    continue;

                switch (entry.State)
                {
                    case EntityState.Added:
                        if (prop.CurrentValue != null)
                            auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                        break;

                    case EntityState.Deleted:
                        if (prop.OriginalValue != null)
                            auditEntry.OldValues[prop.Metadata.Name] = prop.OriginalValue;
                        break;

                    case EntityState.Modified:
                        if (prop.IsModified && !Equals(prop.OriginalValue, prop.CurrentValue))
                        {
                            auditEntry.OldValues[prop.Metadata.Name] = prop.OriginalValue;
                            auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                        }
                        break;
                }
            }

            // Solo registrar si hay cambios reales (Update sin cambios = skip)
            if (entry.State == EntityState.Modified && auditEntry.OldValues.Count == 0)
                continue;

            auditEntry.Description = BuildDescription(auditEntry, entry);
            auditEntries.Add(auditEntry);
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        // Insertar audit logs despues del save (para tener los IDs de entidades nuevas)
        if (auditEntries.Count > 0 && empresaId > 0)
        {
            foreach (var ae in auditEntries)
            {
                AuditLogs.Add(new AuditLog
                {
                    EmpresaId = ae.EmpresaId > 0 ? ae.EmpresaId : empresaId,
                    Timestamp = now,
                    UserId = userId,
                    UserName = userName,
                    Action = ae.Action,
                    EntityType = ae.EntityType,
                    EntityId = ae.EntityId,
                    Description = ae.Description,
                    OldValues = ae.OldValues.Count > 0 ? JsonSerializer.Serialize(ae.OldValues, _jsonOpts) : null,
                    NewValues = ae.NewValues.Count > 0 ? JsonSerializer.Serialize(ae.NewValues, _jsonOpts) : null,
                    IpAddress = AuditIpAddress,
                    Source = AuditSource ?? "Portal"
                });
            }
            await base.SaveChangesAsync(cancellationToken);
        }

        return result;
    }

    private static string BuildDescription(AuditEntry ae, Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry)
    {
        var entityName = ae.EntityType;
        var id = ae.EntityId ?? "?";

        // Intentar obtener un nombre legible
        var nombre = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "Nombre")?.CurrentValue?.ToString()
                  ?? entry.Properties.FirstOrDefault(p => p.Metadata.Name == "Apellido")?.CurrentValue?.ToString();
        if (!string.IsNullOrEmpty(nombre))
        {
            var prenombre = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "Nombre" && p.Metadata.DeclaringType.ClrType.GetProperty("Apellido") != null)
                ?.CurrentValue?.ToString();
            var apellido = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "Apellido")?.CurrentValue?.ToString();
            if (!string.IsNullOrEmpty(apellido) && !string.IsNullOrEmpty(prenombre))
                nombre = $"{apellido}, {prenombre}";
        }

        var suffix = !string.IsNullOrEmpty(nombre) ? $" - {nombre}" : "";

        return ae.Action switch
        {
            "Create" => $"Creó {entityName} #{id}{suffix}",
            "Update" => $"Modificó {entityName} #{id}{suffix} ({ae.OldValues.Count} campo(s))",
            "Delete" => $"Eliminó {entityName} #{id}{suffix}",
            _ => $"{ae.Action} {entityName} #{id}"
        };
    }

    private class AuditEntry
    {
        public string EntityType { get; set; } = "";
        public string? EntityId { get; set; }
        public string Action { get; set; } = "";
        public string? Description { get; set; }
        public int EmpresaId { get; set; }
        public Dictionary<string, object?> OldValues { get; set; } = new();
        public Dictionary<string, object?> NewValues { get; set; } = new();
    }
}
