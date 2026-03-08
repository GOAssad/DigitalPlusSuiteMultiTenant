namespace DigitalPlus.DTOs.Vacaciones
{
    public class ListadoVacacionesPorRangodeFechasDTO
    {
        //select b.Id LegajoId, b.Nombre, c.id SectorId, c.Nombre NombreSector, a.FechaDesde, a.FechaHasta
        public int LegajoId { get; set; }
        public string Nombre { get; set; }
        public int SectorId { get; set; }
        public string NombreSector { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string Nota { get; set; }

    }
}
