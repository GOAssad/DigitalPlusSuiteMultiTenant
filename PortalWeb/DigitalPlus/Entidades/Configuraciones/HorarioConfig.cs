using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalPlus.Entidades.Configuraciones
{
    public class HorarioConfig : IEntityTypeConfiguration<Horario>
    {
        public void Configure(EntityTypeBuilder<Horario> builder)
        {
            builder.Property(p => p.Nombre).HasMaxLength(100).IsRequired();
            builder.HasIndex(p => p.Nombre).IsUnique();
            //builder.HasMany(typeof(HorarioDia)).WithOne();
        }
    }
}
