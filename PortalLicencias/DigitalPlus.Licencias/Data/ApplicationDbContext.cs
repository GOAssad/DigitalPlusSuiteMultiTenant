using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DigitalPlus.Licencias.Entidades;

namespace DigitalPlus.Licencias.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Licencia> Licencias { get; set; }
    public DbSet<LicenciaLog> LicenciasLog { get; set; }
    public DbSet<LicenseCode> LicenseCodes { get; set; }
    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<Pais> Paises { get; set; }
    public DbSet<TipoIdentificacionFiscal> TiposIdentificacionFiscal { get; set; }
    public DbSet<PlanConfig> PlanConfigs { get; set; }
    public DbSet<Moneda> Monedas { get; set; }
    public DbSet<TipoCambio> TiposCambio { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Licencia>(e =>
        {
            e.HasIndex(l => new { l.CompanyId, l.MachineId }).IsUnique();
            e.ToTable(t => t.ExcludeFromMigrations());
        });

        builder.Entity<LicenciaLog>(e =>
        {
            e.ToTable(t => t.ExcludeFromMigrations());
        });

        builder.Entity<LicenseCode>(e =>
        {
            e.HasIndex(c => c.CodeHash).IsUnique();
            e.ToTable(t => t.ExcludeFromMigrations());
        });

        builder.Entity<Empresa>(e =>
        {
            e.HasIndex(emp => emp.CompanyId).IsUnique();
            e.HasIndex(emp => emp.DatabaseName).IsUnique();
            e.ToTable(t => t.ExcludeFromMigrations());
        });

        builder.Entity<Pais>(e =>
        {
            e.HasIndex(p => p.CodigoISO).IsUnique();
            e.ToTable(t => t.ExcludeFromMigrations());
        });

        builder.Entity<TipoIdentificacionFiscal>(e =>
        {
            e.ToTable(t => t.ExcludeFromMigrations());
        });

        builder.Entity<PlanConfig>(e =>
        {
            e.ToTable("PlanConfig", t => t.ExcludeFromMigrations());
        });

        builder.Entity<Moneda>(e =>
        {
            e.HasIndex(m => m.Codigo).IsUnique();
            e.ToTable(t => t.ExcludeFromMigrations());
        });

        builder.Entity<TipoCambio>(e =>
        {
            e.ToTable(t => t.ExcludeFromMigrations());
        });
    }
}
