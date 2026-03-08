namespace DigitalPlus.DTOs
{
    public class FichadaConsolidadoDTO
    {
        public DateTime Fecha { get; set; }

        public int id { get; set; }
        public int LegajoId { get; set; }
        public string Nombre { get; set; }
        public string Sucursal { get; set; }
        public string Entrada { get; set; }
        public string EntrCalen { get; set; }
        public int Entrdif { get; set; }
        public string Salida { get; set; }
        public string SalCalen { get; set; }
        public int SalDif { get; set; }

    }
}
