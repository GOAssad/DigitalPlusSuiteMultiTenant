using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlus.Entidades.Configuraciones
{
    public class VariableGlobalConfig : IEntityTypeConfiguration<VariableGlobal>
    {

        public void Configure(EntityTypeBuilder<VariableGlobal> builder)
        {
            builder.Property(p => p.Nombre).HasMaxLength(100).IsRequired();
            builder.HasIndex(p => p.sId).IsUnique();
        }

    }
}
