namespace DigitalPlus.DTOs
{
    public class FichadaControlDTO
    {
        public DateTime Fecha { get; set; }
        public int id { get; set; }
        public int LegajoId { get; set; }
        public string Nombre { get; set; }
        public string Sucursal { get; set; }
        public string EntraSale { get; set; }
        public DateTime Registro { get; set; }
        public DateTime HorarioCalendario { get; set; }
        public string Auditoria { get; set; }
        public int IncidenciaId { get; set; }
        public string Tipo { get; set; }
    }
}
