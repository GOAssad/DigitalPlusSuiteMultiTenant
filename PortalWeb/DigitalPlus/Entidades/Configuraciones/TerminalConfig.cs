using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalPlus.Entidades.Configuraciones
{
    public class TerminalConfig : IEntityTypeConfiguration<Terminal>
    {
        public void Configure(EntityTypeBuilder<Terminal> builder)
        {
            builder.Property(p => p.Nombre).HasMaxLength(100).IsRequired();
            builder.HasIndex(p => p.Nombre).IsUnique();

        }
    }
}
