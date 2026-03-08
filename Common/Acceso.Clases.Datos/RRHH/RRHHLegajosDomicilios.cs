using System;
using System.Data;
using System.Data.SqlClient;


namespace Acceso.Clases.Datos.RRHH
{
    public class RRHHLegajosDomicilios
    {
        private SqlParameter[] par = new SqlParameter[8];

        public string sMensaje;

        #region Propiedades 

        public bool Existe { get; set; }
        private int _nPaisID;
        public int nPaisID
        {
            get { return _nPaisID; }
            set { _nPaisID = value; }
        }
        private string _sLegajoID;
        public string sLegajoID
        {
            get { return _sLegajoID; }
            set { _sLegajoID = value; }
        }
        private string _sCalle;
        public string sCalle
        {
            get { return _sCalle; }
            set { _sCalle = value; }
        }
        private string _sAltura;
        public string sAltura
        {
            get { return _sAltura; }
            set { _sAltura = value; }
        }
        private string _sBarrio;
        public string sBarrio
        {
            get { return _sBarrio; }
            set { _sBarrio = value; }
        }
        private string _sPiso;
        public string sPiso
        {
            get { return _sPiso; }
            set { _sPiso = value; }
        }
        private string _sProvincia;
        public string sProvincia
        {
            get { return _sProvincia; }
            set { _sProvincia = value; }
        }
        private string _sLocalidad;
        public string sLocalidad
        {
            get { return _sLocalidad; }
            set { _sLocalidad = value; }
        }
        #endregion

        public RRHHLegajosDomicilios() { }
   
        public void Inicializar()
        {
            // RRHHLegajosDomicilios no existe en DigitalPlus — devolver campos vacíos
            _nPaisID = 0;
            _sCalle = string.Empty;
            _sAltura = string.Empty;
            _sBarrio = string.Empty;
            _sPiso = string.Empty;
            _sProvincia = string.Empty;
            _sLocalidad = string.Empty;
            Existe = false;
        }

        public bool Actualizar()
        {
            // RRHHLegajosDomicilios no existe en DigitalPlus — ignorar silenciosamente
            return true;
        }
        public bool eliminar()
        {
            try
            {
                string instruccion;

                instruccion = "delete RRHHLEGAJOSDOMICILIOS WHERE sLegajoID = '" + sLegajoID + "'";
                Global.Datos.SQLServer.EjecutarSPsinRespuesta(instruccion, false);

            }
            catch (Exception e)
            {

                sMensaje = e.Message;
                return false;

            }

            return true;
        }
        private void llenarParametros()
        {
            par[0] = new SqlParameter();
            par[0].ParameterName = "@sLegajoID";
            par[0].Value = (_sLegajoID == null) ? "":_sLegajoID;
            par[0].SqlDbType = SqlDbType.VarChar;

            par[1] = new SqlParameter();
            par[1].ParameterName = "@sCalle";
            par[1].Value = (_sCalle == null) ? "" : _sCalle; 
            par[1].SqlDbType = SqlDbType.VarChar;

            par[2] = new SqlParameter();
            par[2].ParameterName = "@sAltura";
            par[2].Value = (_sAltura == null) ? "" : _sAltura;
            par[2].SqlDbType = SqlDbType.VarChar;

            par[3] = new SqlParameter();
            par[3].ParameterName = "@sBarrio";
            par[3].Value = (_sBarrio == null) ? "" : _sBarrio;
            par[3].SqlDbType = SqlDbType.VarChar;

            par[4] = new SqlParameter();
            par[4].ParameterName = "@sPiso";
            par[4].Value = (_sPiso == null) ? "" : _sPiso;
            par[4].SqlDbType = SqlDbType.VarChar;

            par[5] = new SqlParameter();
            par[5].ParameterName = "@sProvincia";
            par[5].Value = (_sProvincia == null) ? "" : _sProvincia; 
            par[5].SqlDbType = SqlDbType.VarChar;

            par[6] = new SqlParameter();
            par[6].ParameterName = "@sLocalidad";
            par[6].Value = (_sLocalidad == null) ? "" : _sLocalidad; ;
            par[6].SqlDbType = SqlDbType.VarChar;

            par[7] = new SqlParameter();
            par[7].ParameterName = "@nPaisID";
            par[7].Value = _nPaisID;
            par[7].SqlDbType = SqlDbType.Int;
        }
    }
}
