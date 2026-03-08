namespace DigitalPlus.DTOs
{
    public class DYGPINVEReservasArticulosDTO
    {
        public int nReservaArticuloID { get; set; }
        public string ITEMNMBR { get; set; }
        public string sUsuarioID { get; set; }
        public string sDescripcion { get; set; }
        public DateTime dFechaReserva { get; set; }
        public DateTime dFechaVencimiento { get; set; }
        public decimal nCantidad { get; set; }
    }
}
