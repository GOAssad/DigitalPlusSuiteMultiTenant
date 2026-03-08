namespace DigitalPlus.DTOs
{
    public class FichadaAusenciasDTO
    {
        public int Id { get; set; }
        public string LegajoId { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }

        public string Sector { get; set; }
        public string CodigoSucursal { get; set; }
        public string NombreSucursal { get; set; }

    }
}
