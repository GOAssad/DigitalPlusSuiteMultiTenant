using DigitalPlus.Entidades;

namespace DigitalPlus.DTOs
{
    public class LegajoVisualizarDTO
    {
        public Legajo Legajo { get; set; }
        public List<Sucursal> Sucursales { get; set; }
        public List<Dedo> Dedos { get; set; }
    }
}
