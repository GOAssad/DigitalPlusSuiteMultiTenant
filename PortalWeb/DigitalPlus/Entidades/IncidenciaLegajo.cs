namespace DigitalPlus.Entidades
{
    public class IncidenciaLegajo
    {
        public int Id { get; set; }
        public int IncidenciaId { get; set; }
        public int LegajoId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Today;
        public string Detalle { get; set; }
        public Incidencia Incidencia { get; set; }
        public Legajo Legajo { get; set; }
    }
}
