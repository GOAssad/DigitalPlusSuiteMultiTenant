using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalPlus.Entidades.Configuraciones
{
    public class LegajoHuellaConfig : IEntityTypeConfiguration<LegajoHuella>
    {
        public void Configure(EntityTypeBuilder<LegajoHuella> builder)
        {
            builder.HasKey(x => new { x.LegajoId, x.DedoId });
            builder.Property(p => p.sLegajoId).HasMaxLength(5).IsRequired();
            builder.Property(p => p.sLegajoNombre).HasMaxLength(100).IsRequired();
        }
    }
}
