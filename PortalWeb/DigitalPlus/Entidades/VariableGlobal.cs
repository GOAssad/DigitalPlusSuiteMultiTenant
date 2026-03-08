using Microsoft.Build.Evaluation;

namespace DigitalPlus.Entidades
{
    public class VariableGlobal
    {
        public int Id { get; set; }
        public string sId { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }
        public string TipoValor { get; set; }
        public string Valor { get; set; }
        public bool  Reiniciar { get; set; }
    }
}
