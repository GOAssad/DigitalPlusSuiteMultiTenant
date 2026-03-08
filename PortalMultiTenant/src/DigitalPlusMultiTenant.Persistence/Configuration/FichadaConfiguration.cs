using DigitalPlusMultiTenant.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalPlusMultiTenant.Persistence.Configuration;

public class FichadaConfiguration : IEntityTypeConfiguration<Fichada>
{
    public void Configure(EntityTypeBuilder<Fichada> builder)
    {
        builder.ToTable("Fichada");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Tipo).HasMaxLength(1).IsRequired();
        builder.Property(e => e.Origen).HasMaxLength(20)
            .HasConversion<string>();

        builder.HasIndex(e => new { e.EmpresaId, e.FechaHora });
        builder.HasIndex(e => new { e.EmpresaId, e.LegajoId, e.FechaHora });
        builder.HasIndex(e => e.SucursalId);

        builder.HasOne(e => e.Empresa).WithMany(e => e.Fichadas)
            .HasForeignKey(e => e.EmpresaId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Legajo).WithMany(e => e.Fichadas)
            .HasForeignKey(e => e.LegajoId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Sucursal).WithMany(e => e.Fichadas)
            .HasForeignKey(e => e.SucursalId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Terminal).WithMany()
            .HasForeignKey(e => e.TerminalId).OnDelete(DeleteBehavior.SetNull);
    }
}
