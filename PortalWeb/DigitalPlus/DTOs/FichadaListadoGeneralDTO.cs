using DigitalPlus.Entidades;

namespace DigitalPlus.DTOs
{
    public class FichadaListadoGeneralDTO : Fichada
    {
        public string NombreSucursal { get; set; }
        public string NombreLegajo { get; set; }
        public string CodigoSucursal { get; set; }

        public DateTime HorarioCalendario { get; set; }
        public int MinEntTar { get; set; }
        public int MinSaleTem { get; set; }
    }
}
