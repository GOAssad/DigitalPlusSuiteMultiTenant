using DigitalPlus.Entidades;

namespace DigitalPlus.DTOs
{
    public class LegajoActualiacionDTO
    {
        public Legajo Legajo { get; set; }
        public List<Sucursal> SucursalesSeleccionadas { get; set; }
        public List<Sucursal> SucursalesNoSeleccionadas { get; set; }
        public List<Sucursal> Sucursales { get; set; }

        public List<Dedo> DedosSeleccionados { get; set; }
        public List<Dedo> DedosNoSeleccionados { get; set; }
        public List<Dedo> Dedos { get; set; }

    }
}
