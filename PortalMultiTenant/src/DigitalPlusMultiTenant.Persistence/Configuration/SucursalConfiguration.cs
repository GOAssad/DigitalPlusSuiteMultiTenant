using DigitalPlusMultiTenant.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalPlusMultiTenant.Persistence.Configuration;

public class SucursalConfiguration : IEntityTypeConfiguration<Sucursal>
{
    public void Configure(EntityTypeBuilder<Sucursal> builder)
    {
        builder.ToTable("Sucursal");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Codigo).HasMaxLength(10).IsRequired();
        builder.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Direccion).HasMaxLength(200);
        builder.Property(e => e.Localidad).HasMaxLength(100);
        builder.Property(e => e.Provincia).HasMaxLength(100);
        builder.Property(e => e.Telefono).HasMaxLength(50);
        builder.Property(e => e.Email).HasMaxLength(150);
        builder.Property(e => e.CreatedBy).HasMaxLength(100);
        builder.Property(e => e.UpdatedBy).HasMaxLength(100);

        builder.HasIndex(e => new { e.EmpresaId, e.Codigo }).IsUnique();
        builder.HasIndex(e => new { e.EmpresaId, e.Nombre }).IsUnique();

        builder.HasOne(e => e.Empresa).WithMany(e => e.Sucursales)
            .HasForeignKey(e => e.EmpresaId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Geoconfig).WithOne(g => g.Sucursal)
            .HasForeignKey<SucursalGeoconfig>(g => g.SucursalId);
    }
}
