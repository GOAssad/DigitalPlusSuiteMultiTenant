namespace DigitalPlus.Entidades
{
    public class LegajoSucursal
    {
        public int LegajoId { get; set; }
        public int SucursalId { get; set; }
        public Legajo legajo { get; set; }
        public Sucursal Sucursal { get; set; }

    }
}
