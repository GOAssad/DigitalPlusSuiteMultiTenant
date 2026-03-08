using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalPlus.Entidades.Configuraciones
{
    public class LegajoSucursalConfig : IEntityTypeConfiguration<LegajoSucursal>
    {
        public void Configure(EntityTypeBuilder<LegajoSucursal> builder)
        {
            builder.HasKey(x => new { x.LegajoId, x.SucursalId });
        }
    }
}
