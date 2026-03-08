using DigitalPlus.Entidades;

namespace DigitalPlus.DTOs
{
    public class HorarioDTO : Horario
    {
        public List<HorarioDiaDTO> HorariosDias { get; set; } = new List<HorarioDiaDTO>();
    }
}
