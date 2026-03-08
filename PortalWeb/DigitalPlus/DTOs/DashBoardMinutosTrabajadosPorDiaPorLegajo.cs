namespace DigitalPlus.DTOs
{
    public class DashBoardMinutosTrabajadosPorDiaPorLegajo
    {
        public string LegajoId { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string NombreSucursal { get; set; }
        public int Minutos { get; set; }
    }
}
