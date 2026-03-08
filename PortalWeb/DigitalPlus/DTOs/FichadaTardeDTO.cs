namespace DigitalPlus.DTOs
{
    public class FichadaTardeDTO
    {
        public DateTime Fecha { get; set; }
        public int Id { get; set; }
        public int LegajoId { get; set; }
        public string Nombre { get; set; }
        public string Sucursal { get; set; }
        public string EntraSale { get; set; }
        public DateTime Registro { get; set; }
        public DateTime HorarioCalendario { get; set; }
        public int MinEntTar { get; set; }
        public int MinSalTem { get; set; }

    }
}
