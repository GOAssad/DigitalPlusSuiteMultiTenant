using System;
using System.Data;
using System.Data.SqlClient;

namespace Acceso.Clases.Datos.RRHH
{
    public class RRHHFichadasDao: ConnectionSql
    {

        public DataTable getFichadas(DateTime fromDate, DateTime toDate, string legajodesde, string legajohasta, string sGrupo)
        {
            toDate = toDate.AddDays(1);
            SqlParameter[] par = new SqlParameter[6];

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
            par[5] = new SqlParameter("@EmpresaId", SqlDbType.Int);
            par[5].Value = Global.Datos.TenantContext.EmpresaId;

            DataTable dt = Global.Datos.SQLServer.EjecutarSPSelect("RRHHFichadas_SP_LISTADO", par, true);

            return dt;
        }

        public DataTable getAusencias(DateTime fromDate, string grupo)
        {

            SqlParameter[] par = new SqlParameter[3];
            par[0] = new SqlParameter("@Fecha", SqlDbType.DateTime);
            par[0].Value = fromDate;

            par[1] = new SqlParameter("@grupo", SqlDbType.VarChar);
            par[1].Value = grupo;

            par[2] = new SqlParameter("@EmpresaId", SqlDbType.Int);
            par[2].Value = Global.Datos.TenantContext.EmpresaId;

            return Global.Datos.SQLServer.EjecutarSPSelect("RRHHFichadasAusencias_SP_SELECT", par);
        }

        public DataTable getAusenciasxLegajo(string legajo, DateTime fdesde, DateTime fhasta, bool incidencia)
        {
            SqlParameter[] par = new SqlParameter[5];

            par[0] = new SqlParameter("@legajo", SqlDbType.VarChar);
            par[0].Value = legajo;

            par[1] = new SqlParameter("@fdesde", SqlDbType.DateTime);
            par[1].Value = fdesde;

            par[2] = new SqlParameter("@fhasta", SqlDbType.DateTime);
            par[2].Value = fhasta;

            par[3] = new SqlParameter("@incidencia", SqlDbType.Bit);
            par[3].Value = incidencia;

            par[4] = new SqlParameter("@EmpresaId", SqlDbType.Int);
            par[4].Value = Global.Datos.TenantContext.EmpresaId;

            return Global.Datos.SQLServer.EjecutarSPSelect("RRHHFichadasAusencias_x_Legajo_SP_SELECT", par);
        }

        public DataTable getFichadasEntradaEstatus(DateTime fdesde, DateTime fhasta, bool solotarde, string grupo)
        {
            SqlParameter[] par = new SqlParameter[5];

            par[0] = new SqlParameter("@fechadesde", SqlDbType.DateTime);
            par[0].Value = fdesde;

            par[1] = new SqlParameter("@fechahasta", SqlDbType.DateTime);
            par[1].Value = fhasta;

            par[2] = new SqlParameter("@soloTarde", SqlDbType.Bit);
            par[2].Value = solotarde;

            par[3] = new SqlParameter("@grupo", SqlDbType.VarChar);
            par[3].Value = grupo;

            par[4] = new SqlParameter("@EmpresaId", SqlDbType.Int);
            par[4].Value = Global.Datos.TenantContext.EmpresaId;

            return Global.Datos.SQLServer.EjecutarSPSelect("RRHHFichadasEntradaEstatus_SP_SELECT", par);
        }

        /// <summary>
        /// Query directa multi-tenant (tablas Legajo, Horario, Fichada, Sucursal)
        /// </summary>
        public DataTable obtenerFichadas(DateTime fromDate, DateTime toDate, string legajodesde, string legajohasta, string sGrupo)
        {
            toDate = toDate.AddDays(1);
            int empresaId = Global.Datos.TenantContext.EmpresaId;

            using (var connection = getConnection())
            {
                string auxSector = string.Empty;
                if (sGrupo != string.Empty)
                    auxSector = " AND s.Id = " + sGrupo.Trim();

                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"SELECT l.NumeroLegajo AS sLegajoID, l.Apellido AS sApellido, l.Nombre AS sNombre,
                                            h.Nombre AS HorarioSector, f.FechaHora AS dRegistro,
                                            CONVERT(varchar(10), f.FechaHora, 103) Fecha,
                                            CONVERT(char(5), f.FechaHora, 108) Hora, f.Tipo AS sEntraSale,
                                            s.Id AS sSucursalID, s.Id AS sSucursalGrupoID, s.Nombre AS sDescSucursal
                                            FROM Legajo l
                                                INNER JOIN Horario h ON l.HorarioId = h.Id
                                                INNER JOIN Fichada f ON f.LegajoId = l.Id
                                                INNER JOIN Sucursal s ON f.SucursalId = s.Id
                                            WHERE
                                                f.FechaHora BETWEEN @fromDate AND @toDate AND
                                                l.NumeroLegajo BETWEEN @legdesde AND @leghasta AND
                                                l.EmpresaId = @EmpresaId" + auxSector;

                    command.Parameters.Add("@fromDate", SqlDbType.Date).Value = fromDate;
                    command.Parameters.Add("@toDate", SqlDbType.Date).Value = toDate;
                    command.Parameters.Add("@legdesde", SqlDbType.VarChar).Value = legajodesde;
                    command.Parameters.Add("@leghasta", SqlDbType.VarChar).Value = legajohasta;
                    command.Parameters.Add("@EmpresaId", SqlDbType.Int).Value = empresaId;

                    command.CommandType = CommandType.Text;

                    var reader = command.ExecuteReader();
                    var table = new DataTable();
                    table.Load(reader);
                    reader.Dispose();

                    return table;
                }
            }
        }
    }
}
