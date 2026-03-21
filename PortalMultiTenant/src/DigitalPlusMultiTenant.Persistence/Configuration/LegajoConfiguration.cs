using DigitalPlusMultiTenant.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalPlusMultiTenant.Persistence.Configuration;

public class LegajoConfiguration : IEntityTypeConfiguration<Legajo>
{
    public void Configure(EntityTypeBuilder<Legajo> builder)
    {
        builder.ToTable("Legajo");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.NumeroLegajo).HasMaxLength(25).IsRequired();
        builder.Property(e => e.Apellido).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Email).HasMaxLength(200);
        builder.Property(e => e.Telefono).HasMaxLength(50);
        builder.Property(e => e.CreatedBy).HasMaxLength(100);
        builder.Property(e => e.UpdatedBy).HasMaxLength(100);

        builder.HasIndex(e => new { e.EmpresaId, e.NumeroLegajo }).IsUnique();
        builder.HasIndex(e => new { e.EmpresaId, e.IsActive });
        builder.HasIndex(e => new { e.EmpresaId, e.SectorId });
        builder.HasIndex(e => new { e.EmpresaId, e.CategoriaId });

        builder.HasOne(e => e.Empresa).WithMany(e => e.Legajos)
            .HasForeignKey(e => e.EmpresaId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Sector).WithMany(e => e.Legajos)
            .HasForeignKey(e => e.SectorId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Categoria).WithMany(e => e.Legajos)
            .HasForeignKey(e => e.CategoriaId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Horario).WithMany(e => e.Legajos)
            .HasForeignKey(e => e.HorarioId).OnDelete(DeleteBehavior.SetNull);
    }
}

public class LegajoSucursalConfiguration : IEntityTypeConfiguration<LegajoSucursal>
{
    public void Configure(EntityTypeBuilder<LegajoSucursal> builder)
    {
        builder.ToTable("LegajoSucursal");
        builder.HasKey(e => new { e.LegajoId, e.SucursalId });

        builder.Property(e => e.PermiteHuella).HasDefaultValue(true);
        builder.Property(e => e.PermitePin).HasDefaultValue(true);
        builder.Property(e => e.PermiteQr).HasDefaultValue(true);
        builder.Property(e => e.PermiteMovil).HasDefaultValue(true);
        builder.Property(e => e.PermiteKiosko).HasDefaultValue(true);

        builder.HasOne(e => e.Legajo).WithMany(e => e.LegajoSucursales)
            .HasForeignKey(e => e.LegajoId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.Sucursal).WithMany(e => e.LegajoSucursales)
            .HasForeignKey(e => e.SucursalId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class LegajoHuellaConfiguration : IEntityTypeConfiguration<LegajoHuella>
{
    public void Configure(EntityTypeBuilder<LegajoHuella> builder)
    {
        builder.ToTable("LegajoHuella");
        builder.HasKey(e => new { e.LegajoId, e.DedoId });
        builder.Property(e => e.Huella).IsRequired();

        builder.HasOne(e => e.Legajo).WithMany(e => e.Huellas)
            .HasForeignKey(e => e.LegajoId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class LegajoPinConfiguration : IEntityTypeConfiguration<LegajoPin>
{
    public void Configure(EntityTypeBuilder<LegajoPin> builder)
    {
        builder.ToTable("LegajoPin");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.PinHash).HasMaxLength(100).IsRequired();
        builder.Property(e => e.PinSalt).HasMaxLength(100).IsRequired();

        builder.HasIndex(e => e.LegajoId).IsUnique();

        builder.HasOne(e => e.Legajo).WithOne(e => e.Pin)
            .HasForeignKey<LegajoPin>(e => e.LegajoId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class LegajoDomicilioConfiguration : IEntityTypeConfiguration<LegajoDomicilio>
{
    public void Configure(EntityTypeBuilder<LegajoDomicilio> builder)
    {
        builder.ToTable("LegajoDomicilio");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Calle).HasMaxLength(200);
        builder.Property(e => e.Altura).HasMaxLength(20);
        builder.Property(e => e.Piso).HasMaxLength(20);
        builder.Property(e => e.Barrio).HasMaxLength(100);
        builder.Property(e => e.Localidad).HasMaxLength(100);
        builder.Property(e => e.Provincia).HasMaxLength(100);
        builder.Property(e => e.CodigoPostal).HasMaxLength(10);

        builder.HasIndex(e => e.LegajoId).IsUnique();

        builder.HasOne(e => e.Legajo).WithOne(e => e.Domicilio)
            .HasForeignKey<LegajoDomicilio>(e => e.LegajoId).OnDelete(DeleteBehavior.Cascade);
    }
}
