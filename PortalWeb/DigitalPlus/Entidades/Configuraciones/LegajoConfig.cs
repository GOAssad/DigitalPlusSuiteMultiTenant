using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalPlus.Entidades.Configuraciones
{
    public class LegajoConfig : IEntityTypeConfiguration<Legajo>
    {
        public void Configure(EntityTypeBuilder<Legajo> builder)
        {
            builder.Property(p => p.Nombre).HasMaxLength(200).IsRequired();
            builder.Property(p => p.LegajoId).HasMaxLength(25).IsRequired();

        

            //            builder.HasIndex(p => p.Nombre).IsUnique();
            builder.HasIndex(p => p.LegajoId).IsUnique();
        }
    }
}
