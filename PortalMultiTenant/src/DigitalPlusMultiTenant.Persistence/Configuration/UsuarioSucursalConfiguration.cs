using DigitalPlusMultiTenant.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalPlusMultiTenant.Persistence.Configuration;

public class UsuarioSucursalConfiguration : IEntityTypeConfiguration<UsuarioSucursal>
{
    public void Configure(EntityTypeBuilder<UsuarioSucursal> builder)
    {
        builder.ToTable("UsuarioSucursal");
        builder.HasKey(e => new { e.UserId, e.SucursalId });

        builder.HasOne(e => e.User).WithMany(e => e.UsuarioSucursales)
            .HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.Sucursal).WithMany(e => e.UsuarioSucursales)
            .HasForeignKey(e => e.SucursalId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(e => e.NombreCompleto).HasMaxLength(200);
        builder.HasIndex(e => e.EmpresaId);
    }
}
