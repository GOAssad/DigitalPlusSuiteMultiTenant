using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acceso.Clases.Datos.RRHH
{
    public class RRHHAusenciasLegajoReport
    {
        public string reportDate { get; private set; }
        public List<RRHHAusenciasLegajoListing> fichadalisting { get; set; }

        //Metodos
        public void CreateAusenciasReport(string legajo, DateTime fdesde, DateTime fhasta, bool incidencia)
        {
            reportDate = DateTime.Now.ToShortDateString();


            var rrhhfichadasdao = new RRHHFichadasDao();
            var result = rrhhfichadasdao.getAusenciasxLegajo(legajo, fdesde, fhasta, incidencia);

            fichadalisting = new List<RRHHAusenciasLegajoListing>();
            foreach (System.Data.DataRow rows in result.Rows)
            {

                var ausenciassmodel = new RRHHAusenciasLegajoListing()
                {
                    fecha = rows["fecha"].ToString(),
                    nrodia = (int)rows["nrodia"],
                    Dia = rows["Dia"].ToString(),
                    Estatus = rows["Estatus"].ToString()
                    
                };

                fichadalisting.Add(ausenciassmodel);
            }
        }
    }
}
