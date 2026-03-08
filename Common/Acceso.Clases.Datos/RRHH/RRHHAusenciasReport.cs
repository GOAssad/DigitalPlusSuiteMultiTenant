using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acceso.Clases.Datos.RRHH
{
    public class RRHHAusenciasReport
    {
        public string reportDate { get; private set; }
        public List<RRHHFichadasListing> fichadalisting { get; set; }

        //Metodos
        public void CreateAusenciasReport(DateTime date, string grupo)
        {
            reportDate = DateTime.Now.ToShortDateString();
            var rrhhfichadasdao = new RRHHFichadasDao();
            var result = rrhhfichadasdao.getAusencias(date, grupo);

            fichadalisting = new List<RRHHFichadasListing>();
            foreach (System.Data.DataRow rows in result.Rows)
            {

                var ausenciassmodel = new RRHHFichadasListing()
                {
                    slegajoid = rows["sLegajoID"].ToString(),
                    sapellido = rows["sApellido"].ToString(),
                    snombre = rows["sNombre"].ToString(),
                    sSucursalID = rows["sSucursalID"].ToString(),
                    sDescSucursal = rows["sDescSucursal"].ToString()
                };

                fichadalisting.Add(ausenciassmodel);
            }
        }
    }
}
