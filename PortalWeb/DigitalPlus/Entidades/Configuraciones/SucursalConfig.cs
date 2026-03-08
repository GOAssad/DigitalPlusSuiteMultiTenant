using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalPlus.Entidades.Configuraciones
{
    public class SucursalConfig : IEntityTypeConfiguration<Sucursal>
    {
        public void Configure(EntityTypeBuilder<Sucursal> builder)
        {
            builder.Property(p => p.Nombre).HasMaxLength(100).IsRequired();
            builder.Property(p => p.CodigoSucursal).HasMaxLength(5).IsRequired();

            builder.HasIndex(p => p.Nombre).IsUnique();
            builder.HasIndex(p => p.CodigoSucursal).IsUnique();
        }
    }
}
