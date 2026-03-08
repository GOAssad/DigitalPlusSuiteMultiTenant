using DigitalPlus.Entidades;
using DigitalPlus.Entidades.DigitalOnePlusEscritorio;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DigitalPlus.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //------------------------------------------------------------------------------------------------------
            //---- Tomar la configuracion de los EntityTypeConfiguration que se encuentran en Entity/Configuraciones
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //------------------------------------------------------------------------------------------------------
        }
        public DbSet<Sector> Sectores { get; set; }
        public DbSet<Legajo> Legajos { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<HorarioDia> HorariosDias { get; set; }
        public DbSet<HorarioDiaEvento> HorariosDiasEventos { get; set; }
        public DbSet<Dia> Dias { get; set; }
        public DbSet<Terminal> Terminales { get; set; }
        public DbSet<Incidencia> Incidencias { get; set; }
        public DbSet<Feriado> Feriados { get; set; }
        public DbSet<Fichada> Fichadas { get; set; }
        public DbSet<LegajoSucursal> LegajosSucursales { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Dedo> Dedos { get; set; }
        public DbSet<LegajoHuella> LegajosHuellas { get; set; }
        public DbSet<IncidenciaLegajo> IncidenciasLegajos { get; set; }
        public DbSet<UsuarioSucursal> UsuariosSucursales { get; set; }  
        public DbSet<GRALUsuario> GRALUsuarios { get; set; }
        public DbSet<Vacacion> Vacaciones { get; set; }
        public DbSet<Noticia> Noticias { get; set; }
        public DbSet<VariableGlobal> VariablesGlobales { get; set; }
    }
}

