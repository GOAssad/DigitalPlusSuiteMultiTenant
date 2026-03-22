using System;
using System.Data;
using System.Data.SqlClient;

namespace Acceso.Clases.Datos.RRHH
{
    public class RRHHFichadas
    {


        protected int _nSucursalID;
        protected int _nLegajoID;
        protected string _sSucursalID;
        protected string _sLegajoID;
        protected DateTime _dRegistro;

        protected string _sOrigen;
        protected int _nTerminalId;

        public string sMensaje;

        private SqlParameter[] par = new SqlParameter[7];


        #region Propiedades

        public string sEntraSale;

        public string sSucursalID
        {
            set { _sSucursalID = value; }
            get { return _sSucursalID; }
        }

        public int nSucursalID
        {
            set { _nSucursalID = value; }
            get { return _nSucursalID; }
        }


        public string sLegajoID
        {

            set { _sLegajoID = value; }
            get { return _sLegajoID; }
        }

        public int nLegajoID
        {

            set { _nLegajoID = value; }
            get { return _nLegajoID; }
        }
        public DateTime dRegistro
        {

            set { _dRegistro = value; }
            get { return _dRegistro; }
        }

        public string sOrigen
        {
            set { _sOrigen = value; }
            get { return _sOrigen; }
        }

        public int nTerminalId
        {
            set { _nTerminalId = value; }
            get { return _nTerminalId; }
        }
        #endregion

        public bool Actualizar()
        {
            string cadena;
            cadena = "EscritorioFichadasSPSALIDA";
            try
            {

                llenarParametros();

                sEntraSale = Global.Datos.SQLServer.EjecutarSPInsertUpdateSalida(cadena, par);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Eliminar(SqlParameter[] par)
        {
            string cadena = "RRHHFichadas_SP_ELIMINAR";
            return Global.Datos.SQLServer.EjecutarSPInsertUpdateBool(cadena, par);
        }

        public bool Actualizar(SqlParameter[] par)
        {
            string cadena = "RRHHFichadas_SP_MANUAL";
            try
            {
                Global.Datos.SQLServer.EjecutarSPInsertUpdateBool(cadena, par);
                return true;
            }
            catch (Exception ex)
            {

                sMensaje = "Error insertando Registro \n\r " + ex.Message;
                return false;
            }


        }
        public DataTable TraerFichadas(DateTime fromDate, DateTime ToDate, string legdesde, string leghasta)
        {
            SqlParameter[] param = new SqlParameter[5];

            param[0] = new SqlParameter("@ld", SqlDbType.VarChar);
            param[0].Value = legdesde;

            param[1] = new SqlParameter("@lh", SqlDbType.VarChar);
            param[1].Value = leghasta;

            param[2] = new SqlParameter("@fd", SqlDbType.DateTime);
            param[2].Value = fromDate;

            param[3] = new SqlParameter("@fh", SqlDbType.DateTime);
            param[3].Value = ToDate;

            param[4] = new SqlParameter("@EmpresaId", SqlDbType.Int);
            param[4].Value = Global.Datos.TenantContext.EmpresaId;

            return Global.Datos.SQLServer.EjecutarSPSelect("RRHHFichadas_SP_MANUAL_SELECT", param);

        }

        public DataTable TraerFichadas(DateTime fromDate, DateTime ToDate, string legdesde)
        {
            if (legdesde == null)
                legdesde = string.Empty;

            SqlParameter[] param = new SqlParameter[4];

            param[0] = new SqlParameter("@ld", SqlDbType.VarChar);
            param[0].Value = legdesde;


            param[1] = new SqlParameter("@fd", SqlDbType.DateTime);
            param[1].Value = fromDate;

            param[2] = new SqlParameter("@fh", SqlDbType.DateTime);
            param[2].Value = ToDate;

            param[3] = new SqlParameter("@EmpresaId", SqlDbType.Int);
            param[3].Value = Global.Datos.TenantContext.EmpresaId;

            return Global.Datos.SQLServer.EjecutarSPSelect("RRHHFichadas_SP_MANUAL_SELECT_GRUPO", param);


        }

        public DataTable TraerLlegadasTarde(string legajo)
        {

            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@SoloTarde", SqlDbType.Bit);
            param[0].Value = true;


            param[1] = new SqlParameter("@legajo", SqlDbType.VarChar);
            param[1].Value = legajo;

            param[2] = new SqlParameter("@EmpresaId", SqlDbType.Int);
            param[2].Value = Global.Datos.TenantContext.EmpresaId;

            return Global.Datos.SQLServer.EjecutarSPSelect("RRHHFichadasEntradaEstatusLegajo_SP_SELECT", param);
        }
        private void llenarParametros()
        {

            par[0] = new SqlParameter();
            par[0].ParameterName = "@nSucursalID";
            par[0].Value = _nSucursalID;
            par[0].SqlDbType = SqlDbType.Int;

            par[1] = new SqlParameter();
            par[1].ParameterName = "@nLegajoID";
            par[1].Value = _nLegajoID;
            par[1].SqlDbType = SqlDbType.Int;

            par[2] = new SqlParameter();
            par[2].ParameterName = "@dRegistro";
            par[2].Value = _dRegistro;
            par[2].SqlDbType = SqlDbType.DateTime;

            par[3] = new SqlParameter();
            par[3].ParameterName = "@sAccion";
            par[3].Value = string.Empty;
            par[3].SqlDbType = SqlDbType.VarChar;
            par[3].Size = 50;
            par[3].Direction = ParameterDirection.Output;

            par[4] = new SqlParameter();
            par[4].ParameterName = "@EmpresaId";
            par[4].Value = Global.Datos.TenantContext.EmpresaId;
            par[4].SqlDbType = SqlDbType.Int;

            par[5] = new SqlParameter();
            par[5].ParameterName = "@Origen";
            par[5].Value = string.IsNullOrEmpty(_sOrigen) ? (object)DBNull.Value : _sOrigen;
            par[5].SqlDbType = SqlDbType.NVarChar;

            par[6] = new SqlParameter();
            par[6].ParameterName = "@TerminalId";
            par[6].Value = _nTerminalId > 0 ? (object)_nTerminalId : DBNull.Value;
            par[6].SqlDbType = SqlDbType.Int;

        }
    }
}
