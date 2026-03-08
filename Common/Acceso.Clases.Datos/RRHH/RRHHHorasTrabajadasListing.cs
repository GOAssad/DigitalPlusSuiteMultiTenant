using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acceso.Clases.Datos.RRHH
{
    public class RRHHHorasTrabajadasListing
    {
        

        public string slegajoid { get; set; }
        public string sapellido { get; set; }
        public string snombre { get; set; }
        public string horariosector { get; set; }
        public string fecha { get; set; }
        public string MinHora { get; set; }
        public string MaxHora { get; set; }
        public string sSucursalID { get; set; }
        public string sSucursalGrupoID { get; set; }
        public string sDescSucursal { get; set; }
        public string DiaSemana { get; set; }
        public int NumeroDia { get; set; }
        public string HorarioEstab { get; set; }
        public int MinutosTrabajados { get; set; }
        public string Hs_minTrab { get; set; }
    }
}
