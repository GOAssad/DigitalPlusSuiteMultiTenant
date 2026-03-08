using DigitalPlusMultiTenant.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalPlusMultiTenant.Persistence.Configuration;

public class IncidenciaConfiguration : IEntityTypeConfiguration<Incidencia>
{
    public void Configure(EntityTypeBuilder<Incidencia> builder)
    {
        builder.ToTable("Incidencia");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Color).HasMaxLength(15);
        builder.Property(e => e.Abreviatura).HasMaxLength(4);
        builder.Property(e => e.CreatedBy).HasMaxLength(100);
        builder.Property(e => e.UpdatedBy).HasMaxLength(100);

        builder.HasIndex(e => new { e.EmpresaId, e.Nombre }).IsUnique();

        builder.HasOne(e => e.Empresa).WithMany(e => e.Incidencias)
            .HasForeignKey(e => e.EmpresaId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class IncidenciaLegajoConfiguration : IEntityTypeConfiguration<IncidenciaLegajo>
{
    public void Configure(EntityTypeBuilder<IncidenciaLegajo> builder)
    {
        builder.ToTable("IncidenciaLegajo");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Detalle).HasMaxLength(500);
        builder.Property(e => e.CreatedBy).HasMaxLength(100);

        builder.HasOne(e => e.Incidencia).WithMany(e => e.IncidenciaLegajos)
            .HasForeignKey(e => e.IncidenciaId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Legajo).WithMany(e => e.IncidenciaLegajos)
            .HasForeignKey(e => e.LegajoId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class VacacionConfiguration : IEntityTypeConfiguration<Vacacion>
{
    public void Configure(EntityTypeBuilder<Vacacion> builder)
    {
        builder.ToTable("Vacacion");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Nota).HasMaxLength(500);
        builder.Property(e => e.CreatedBy).HasMaxLength(100);

        builder.HasOne(e => e.Legajo).WithMany(e => e.Vacaciones)
            .HasForeignKey(e => e.LegajoId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class EventoCalendarioConfiguration : IEntityTypeConfiguration<EventoCalendario>
{
    public void Configure(EntityTypeBuilder<EventoCalendario> builder)
    {
        builder.ToTable("EventoCalendario");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Nota).HasMaxLength(500);

        builder.HasOne(e => e.Legajo).WithMany(e => e.EventosCalendario)
            .HasForeignKey(e => e.LegajoId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class FeriadoConfiguration : IEntityTypeConfiguration<Feriado>
{
    public void Configure(EntityTypeBuilder<Feriado> builder)
    {
        builder.ToTable("Feriado");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Nombre).HasMaxLength(100).IsRequired();

        builder.HasIndex(e => new { e.EmpresaId, e.Fecha }).IsUnique();

        builder.HasOne(e => e.Empresa).WithMany(e => e.Feriados)
            .HasForeignKey(e => e.EmpresaId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class NoticiaConfiguration : IEntityTypeConfiguration<Noticia>
{
    public void Configure(EntityTypeBuilder<Noticia> builder)
    {
        builder.ToTable("Noticia");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Titulo).HasMaxLength(200).IsRequired();
        builder.Property(e => e.CreatedBy).HasMaxLength(100);

        builder.HasOne(e => e.Empresa).WithMany(e => e.Noticias)
            .HasForeignKey(e => e.EmpresaId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class VariableSistemaConfiguration : IEntityTypeConfiguration<VariableSistema>
{
    public void Configure(EntityTypeBuilder<VariableSistema> builder)
    {
        builder.ToTable("VariableSistema");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Clave).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Descripcion).HasMaxLength(500);
        builder.Property(e => e.TipoValor).HasMaxLength(50);
        builder.Property(e => e.Valor).HasMaxLength(500);

        builder.HasIndex(e => new { e.EmpresaId, e.Clave }).IsUnique();
    }
}
