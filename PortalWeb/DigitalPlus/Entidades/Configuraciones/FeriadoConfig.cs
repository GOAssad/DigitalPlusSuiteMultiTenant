using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalPlus.Entidades.Configuraciones
{
    public class FeriadoConfig: IEntityTypeConfiguration<Feriado>
    {
        public void Configure(EntityTypeBuilder<Feriado> builder)
        {
            builder.Property(p => p.Nombre).HasMaxLength(100).IsRequired();
            
        }
    }
}
