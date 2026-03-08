namespace DigitalPlus.DTOs
{
    public class DashBoardMinutosMensualesCalendarioPorLegajo
    {
        public int Id { get; set; }
        public string NombreDia { get; set; }
        public int  CantDiasEnMes { get; set; }
        public int MinutosPorCadaDia { get; set; }
        public int HoraDesde { get; set; }
        public int MinutoDesde { get; set; }
        public int HoraHasta { get; set; }
        public int MinutoHasta { get; set; }

    }
}
