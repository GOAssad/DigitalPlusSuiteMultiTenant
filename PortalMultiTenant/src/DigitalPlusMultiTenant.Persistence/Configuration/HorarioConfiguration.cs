using DigitalPlusMultiTenant.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalPlusMultiTenant.Persistence.Configuration;

public class HorarioConfiguration : IEntityTypeConfiguration<Horario>
{
    public void Configure(EntityTypeBuilder<Horario> builder)
    {
        builder.ToTable("Horario");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
        builder.Property(e => e.CreatedBy).HasMaxLength(100);
        builder.Property(e => e.UpdatedBy).HasMaxLength(100);

        builder.HasIndex(e => new { e.EmpresaId, e.Nombre }).IsUnique();
        builder.HasIndex(e => new { e.EmpresaId, e.Codigo }).IsUnique().HasFilter("[Codigo] IS NOT NULL AND [Codigo] <> ''");

        builder.HasOne(e => e.Empresa).WithMany(e => e.Horarios)
            .HasForeignKey(e => e.EmpresaId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class HorarioDetalleConfiguration : IEntityTypeConfiguration<HorarioDetalle>
{
    public void Configure(EntityTypeBuilder<HorarioDetalle> builder)
    {
        builder.ToTable("HorarioDetalle");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.DiaSemana).IsRequired();
        builder.Property(e => e.HoraDesde).IsRequired();
        builder.Property(e => e.HoraHasta).IsRequired();

        builder.HasOne(e => e.Horario).WithMany(e => e.Detalles)
            .HasForeignKey(e => e.HorarioId).OnDelete(DeleteBehavior.Cascade);
    }
}
