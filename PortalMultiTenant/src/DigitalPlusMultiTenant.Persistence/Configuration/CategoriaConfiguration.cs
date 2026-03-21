using DigitalPlusMultiTenant.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalPlusMultiTenant.Persistence.Configuration;

public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categoria");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
        builder.Property(e => e.CreatedBy).HasMaxLength(100);
        builder.Property(e => e.UpdatedBy).HasMaxLength(100);

        builder.HasIndex(e => new { e.EmpresaId, e.Nombre }).IsUnique();
        builder.HasIndex(e => new { e.EmpresaId, e.Codigo }).IsUnique().HasFilter("[Codigo] IS NOT NULL AND [Codigo] <> ''");

        builder.HasOne(e => e.Empresa).WithMany(e => e.Categorias)
            .HasForeignKey(e => e.EmpresaId).OnDelete(DeleteBehavior.Restrict);
    }
}
