namespace DigitalPlus.Entidades
{
    public class Vacacion
    {
        public int Id { get; set; }
        public int LegajoId { get; set; }
        public DateTime FechaDesde { get; set; } = DateTime.Now.Date;
        public DateTime FechaHasta { get; set; } = DateTime.Now.Date.AddDays(1);
        public string Nota{ get; set; }
        public Legajo Legajo { get; set; }
    }
}
