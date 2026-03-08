using Global.Datos;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Acceso.Clases.Datos.RRHH
{
    public class RRHHVacaciones
    {
        protected string _sLegajoID;
        protected DateTime _dDesde;
        protected DateTime _dHasta;

        public string sLegajoID
        {
            set { _sLegajoID = value; }
            get { return _sLegajoID; }
        }

        public DateTime dDesde
        {
            get { return _dDesde; }
            set { _dDesde = value; }
        }

        public DateTime dHasta
        {
            get { return _dHasta; }
            set { _dHasta = value; }
        }

        public DataTable TraerVacaciones()
        {
            string cadena = "RRHHVacaciones_SP_SELECT_LEGAJO";

            SqlParameter[] par = new SqlParameter[2];

            par[0] = new SqlParameter();
            par[0].ParameterName = "@sLegajoID";
            par[0].Value = _sLegajoID;
            par[0].SqlDbType = SqlDbType.VarChar;

            par[1] = new SqlParameter();
            par[1].ParameterName = "@EmpresaId";
            par[1].Value = TenantContext.EmpresaId;
            par[1].SqlDbType = SqlDbType.Int;

            return SQLServer.EjecutarSPSelect(cadena, par);
        }

        public void Insertar()
        {
            string cadena = "RRHHVacaciones_SP_INSERT_LEGAJO";

            SqlParameter[] par = new SqlParameter[4];

            par[0] = new SqlParameter();
            par[0].ParameterName = "@sLegajoID";
            par[0].Value = _sLegajoID;
            par[0].SqlDbType = SqlDbType.VarChar;

            par[1] = new SqlParameter();
            par[1].ParameterName = "@dDesde";
            par[1].Value = _dDesde;
            par[1].SqlDbType = SqlDbType.Date;

            par[2] = new SqlParameter();
            par[2].ParameterName = "@dHasta";
            par[2].Value = _dHasta;
            par[2].SqlDbType = SqlDbType.Date;

            par[3] = new SqlParameter();
            par[3].ParameterName = "@EmpresaId";
            par[3].Value = TenantContext.EmpresaId;
            par[3].SqlDbType = SqlDbType.Int;

            SQLServer.EjecutarSPInsertUpdateBool(cadena, par);
        }

        public void Eliminar()
        {
            string cadena = "RRHHVacaciones_SP_DELETE";

            SqlParameter[] par = new SqlParameter[4];

            par[0] = new SqlParameter();
            par[0].ParameterName = "@sLegajoID";
            par[0].Value = _sLegajoID;
            par[0].SqlDbType = SqlDbType.VarChar;

            par[1] = new SqlParameter();
            par[1].ParameterName = "@dDesde";
            par[1].Value = _dDesde;
            par[1].SqlDbType = SqlDbType.Date;

            par[2] = new SqlParameter();
            par[2].ParameterName = "@dHasta";
            par[2].Value = _dHasta;
            par[2].SqlDbType = SqlDbType.Date;

            par[3] = new SqlParameter();
            par[3].ParameterName = "@EmpresaId";
            par[3].Value = TenantContext.EmpresaId;
            par[3].SqlDbType = SqlDbType.Int;

            SQLServer.EjecutarSPInsertUpdateBool(cadena, par);
        }
    }
}
