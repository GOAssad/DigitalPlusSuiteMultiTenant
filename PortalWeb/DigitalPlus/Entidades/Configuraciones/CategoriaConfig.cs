using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalPlus.Entidades.Configuraciones
{
    public class CategoriaConfig : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.Property(p => p.Nombre).HasMaxLength(100).IsRequired();
            builder.HasIndex(p => p.Nombre).IsUnique();
        }
    }
}