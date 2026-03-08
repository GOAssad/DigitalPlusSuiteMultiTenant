namespace DigitalPlus.DTOs
{
    public class RespuestaPaginada<T>
    {
        public int TotalPaginas { get; set; }
        public List<T> Registros { get; set; }
    }
}

