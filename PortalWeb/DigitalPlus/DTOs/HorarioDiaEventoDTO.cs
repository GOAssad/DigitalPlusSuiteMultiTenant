using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalPlus.DTOs
{
    public class HorarioDiaEventoDTO
    {

        public int Id { get; set; }


        public int LegajoId { get; set; }
        public string Nota { get; set; }
        public DateTime FechaEvento{ get; set; } = new DateTime(1900, 1, 1);
        public DateTime FromDate { get; set; } = new DateTime(1900, 1, 1);
        public DateTime ToDate { get; set; } = new DateTime(1900, 1, 1);
        public string FechaValor{ get; set; }
        public string DiaNombre{ get; set; }
        public string Mensaje { get; set; }

        public string HoraTexto
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FromDate.ToShortTimeString()) || string.IsNullOrWhiteSpace(ToDate.ToShortTimeString()))
                {
                    return string.Empty;
                }
                else if(FromDate.ToShortTimeString() == "00:00" || ToDate.ToShortTimeString() == "00:00")
                {
                    return string.Empty;
                }
                else if (FromDate.ToShortTimeString().Contains("12:00") && ToDate.ToShortTimeString().Contains("12:00"))
                {
                    return string.Empty;
                }
                else
                {
                    return FromDate.ToShortTimeString() + " - " + ToDate.ToShortTimeString();
                }
            }
        }
    }
}
