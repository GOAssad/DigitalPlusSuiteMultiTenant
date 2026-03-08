using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acceso.Clases.Datos.RRHH
{
    public class RRHHHorasTrabajadasDao: ConnectionSql
    {
        public DataTable getHorasTrabajadas(DateTime fromDate, DateTime toDate, string legajodesde, string legajohasta, string sGrupo)
        {
            toDate = toDate.AddDays(1);
            SqlParameter[] par = new SqlParameter[5];

            string auxSector = string.Empty;
            if (sGrupo != string.Empty)
                auxSector = sGrupo.Trim();
            else
                auxSector = null;


            par[0] = new SqlParameter("@fromDate", SqlDbType.Date);
            par[0].Value = fromDate;
            par[1] = new SqlParameter("@toDate", SqlDbType.Date);
            par[1].Value = toDate;
            par[2] = new SqlParameter("@legdesde", SqlDbType.VarChar);
            par[2].Value = legajodesde;
            par[3] = new SqlParameter("@leghasta", SqlDbType.VarChar);
            par[3].Value = legajohasta;
            par[4] = new SqlParameter("@grupo", SqlDbType.VarChar);
            par[4].Value = auxSector;

            DataTable dt = Global.Datos.SQLServer.EjecutarSPSelect("RRHHFichadasHorasTrabajadas_SP_LISTADO", par, true);


            return dt;
        }
    }
}
