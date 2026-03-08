using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalPlus.Entidades.Configuraciones
{
    public class UsuarioSucursalConfig : IEntityTypeConfiguration<UsuarioSucursal>
    {
        public void Configure(EntityTypeBuilder<UsuarioSucursal> builder)
        {
            builder.HasKey(x => new { x.UsuarioId, x.SucursalId });
        }
    }
}
