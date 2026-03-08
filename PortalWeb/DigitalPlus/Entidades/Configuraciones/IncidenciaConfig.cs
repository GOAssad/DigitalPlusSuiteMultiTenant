using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalPlus.Entidades.Configuraciones
{
    public class IncidenciaConfig : IEntityTypeConfiguration<Incidencia>
    {
        public void Configure(EntityTypeBuilder<Incidencia> builder)
        {
            builder.Property(p => p.Nombre).HasMaxLength(100).IsRequired();
            builder.HasIndex(p => p.Nombre).IsUnique();

            builder.Property(p => p.Color).HasMaxLength(15);
            builder.Property(p => p.Abreviatura).HasMaxLength(4);
        }
    }
}


