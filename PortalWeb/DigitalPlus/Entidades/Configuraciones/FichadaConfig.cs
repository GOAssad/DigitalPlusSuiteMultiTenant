using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalPlus.Entidades.Configuraciones
{
    public class FichadaConfig : IEntityTypeConfiguration<Fichada>
    {
        public void Configure(EntityTypeBuilder<Fichada> builder)
        {
            builder.Property(p => p.EntraSale).HasMaxLength(1).IsRequired();
            builder.HasIndex(x => new { x.Legajoid, x.Registro });
            

        }
    }
}
