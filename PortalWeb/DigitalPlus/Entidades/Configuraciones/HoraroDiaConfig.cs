using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DigitalPlus.Entidades.Configuraciones
{
    public class HoraroDiaConfig : IEntityTypeConfiguration<HorarioDia>
    {
        public void Configure(EntityTypeBuilder<HorarioDia> builder)
        {
          //builder.Property(prop => prop.Cerrado).HasConversion<bool>();
        }
    }
  
}
