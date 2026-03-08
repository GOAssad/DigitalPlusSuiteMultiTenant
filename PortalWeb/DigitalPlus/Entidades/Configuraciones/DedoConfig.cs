using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalPlus.Entidades.Configuraciones
{
    public class DedoConfig : IEntityTypeConfiguration<Dedo>
    {
        public void Configure(EntityTypeBuilder<Dedo> builder)
        {
            builder.Property(p => p.Nombre).HasMaxLength(100).IsRequired();
            builder.HasIndex(p => p.Nombre).IsUnique();

            //para evitar el identity
            builder.Property(l => l.Id).ValueGeneratedNever();
            builder.HasKey(l => l.Id);
        }
    }
}
