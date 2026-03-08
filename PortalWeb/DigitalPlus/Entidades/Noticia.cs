namespace DigitalPlus.Entidades
{
    public class Noticia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }
        public DateTime FechaDesde { get; set; } = DateTime.Today;
        public DateTime FechaHasta { get; set; } = DateTime.Today.AddMonths(1);
        public bool Privado { get; set; } = false;

    }
}
