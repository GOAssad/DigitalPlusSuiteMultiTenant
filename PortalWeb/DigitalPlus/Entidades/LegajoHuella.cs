namespace DigitalPlus.Entidades
{
    public class LegajoHuella
    {
        public int LegajoId { get; set; }
        public string sLegajoId { get; set; }
        public int DedoId { get; set; }
        public byte[] huella{ get; set; }
        public Legajo legajo { get; set; }
        public Dedo Dedo { get; set; }
        public int nFingerMask { get; set; }
        public string sLegajoNombre { get; set; }

    }
}
