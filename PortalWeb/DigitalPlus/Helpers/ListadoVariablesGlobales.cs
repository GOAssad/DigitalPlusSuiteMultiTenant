using DigitalPlus.Entidades;
using DigitalPlus.Repositorios;

namespace DigitalPlus.Helpers
{
    public class ListadoVariablesGlobales
    {
        public List<VariableGlobal> Variables { get; set; } = new List<VariableGlobal>();
        public ListadoVariablesGlobales(RepositorioVariablesGlobales repositorio)
        {
            if (Variables.Count == 0)
            {
                Variables = repositorio.GetLista();
            }
        }
        
    }
}
