using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace Acceso.Clases.Datos.Clientes
{
    public class VENTClientes
    {
        string tabla = "VENTClientes";
        public DataTable DTCreditos = new DataTable();
        public DataTable DTCuotas = new DataTable();

        public decimal TotalCreditos;
        public decimal TotalCuotas;
        public decimal TotalSaldo;
        public int TotalCantCreditos;

        protected string _sClienteID;
        protected string _sRazonSocial;
        protected string _sCalle;
        protected string _sAltura;
        protected string _sLocalidad;
        protected string _sProvincia;
        protected string _sCodigoPostal;
        protected int _nPaisID;
        protected string _sTelefonoCelular;
        protected string _sTelefonoFijo;

        protected bool _lActivo;

        private SqlParameter[] par = new SqlParameter[11];
        DataTable dt;

        public bool Existe;

        public string sMensaje;

        #region Propiedades



        public string sClienteID
        {

            set { _sClienteID = value; }
            get { return _sClienteID; }
        }

        public string sRazonSocial
        {

            set { _sRazonSocial = value; }
            get { return _sRazonSocial; }
        }


        public string sCalle
        {
            set { _sCalle = value; }
            get { return _sCalle; }
        }

        public string sAltura
        {
            set { _sAltura = value; }
            get { return _sAltura; }
        }

        public string sLocalidad
        {
            set { _sLocalidad = value; }
            get { return _sLocalidad; }
        }

        public string sProvincia
        {
            set { _sProvincia = value; }
            get { return _sProvincia; }
        }

        public string sCodigoPostal
        {
            set { _sCodigoPostal = value; }
            get { return _sCodigoPostal; }
        }

        public int nPaisID
        {
            set { _nPaisID = value; }
            get { return _nPaisID; }
        }

        public string sTelefonoCelular
        {
            set { _sTelefonoCelular = value; }
            get { return _sTelefonoCelular; }
        }

        public string sTelefonoFijo
        {
            set { _sTelefonoFijo = value; }
            get { return _sTelefonoFijo; }
        }

        public bool lActivo
        {
            set { _lActivo = value; }
            get { return _lActivo; }
        }
        #endregion
        public bool Eliminar()
        {
            string cadena = "VENTClientes_Delete";
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter();
            param[0].ParameterName = "@sClienteID";
            param[0].Value = _sClienteID;
            param[0].SqlDbType = SqlDbType.VarChar;

            try
            {
                Global.Datos.SQLServer.EjecutarSPInsertUpdate(cadena, param);
                return true;
            }
            catch (Exception ex)
            {
                sMensaje = ex.Message;
                return false;
            }
        }

        public void Inicializar()
        {
            sMensaje = string.Empty;
            string cadena;
            cadena = "Select * from  " + tabla + " where sClienteID = '" + _sClienteID + "'";

            try
            {

                dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(cadena);

                _sRazonSocial = dt.Rows[0]["sRazonSocial"].ToString();
                _sCalle = dt.Rows[0]["sCalle"].ToString();
                _sAltura = dt.Rows[0]["sAltura"].ToString();
                _sLocalidad = dt.Rows[0]["sLocalidad"].ToString();
                _sProvincia = dt.Rows[0]["sProvincia"].ToString();
                _sCodigoPostal = dt.Rows[0]["sCodigoPostal"].ToString();
                _nPaisID = (int)dt.Rows[0]["nPaisID"];
                _sTelefonoCelular = dt.Rows[0]["sTelefonoCelular"].ToString();
                _sTelefonoFijo = dt.Rows[0]["sTelefonoFijo"].ToString();
                _lActivo = (bool)dt.Rows[0]["lActivo"];

                Existe = true;

                //Buscar Creditos del cliente
                BuscarCreditos();

            }
            catch (Exception ex)
            {

                _sRazonSocial = String.Empty;

                _sCalle = string.Empty;
                _sAltura = string.Empty;
                _sLocalidad = string.Empty;
                _sProvincia = string.Empty;
                _sCodigoPostal = string.Empty;
                _nPaisID = 0;
                _sTelefonoCelular = string.Empty;
                _sTelefonoFijo = string.Empty;
                _lActivo = true;

                TotalCreditos = 0;
                TotalCuotas = 0;
                TotalSaldo = 0;


                if (ex.Message.Contains("ningun"))
                    sMensaje = string.Empty;
                else
                    sMensaje = "No se encontro el Registro";

                Existe = false;                
                
            }
        }

        private void BuscarCreditos()
        {

            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter();
            param[0].ParameterName = "@sClienteID";
            param[0].Value = _sClienteID;
            param[0].SqlDbType = SqlDbType.VarChar;

            DTCreditos =  Global.Datos.SQLServer.EjecutarSPSelect("VENTCreditos_SP_SELECT", param);

            //Sumo el total de los creditos
            TotalCreditos = DTCreditos.AsEnumerable().Sum(x => x.Field<decimal>("nImporte"));
            TotalCantCreditos = DTCreditos.AsEnumerable().Count();

            ///Ahora las cuotas
            ///
            DTCuotas = Global.Datos.SQLServer.EjecutarSPSelect("VENTCreditosCuotas_SP_SELECT", param);
            //Sumo el total de lo pagado en Cuotas
            TotalCuotas = DTCuotas.AsEnumerable().Sum(x => x.Field<decimal>("nImporte"));

            TotalSaldo = TotalCreditos - TotalCuotas;
            

        }



        public bool Actualizar()
        {

            string cadena;
            cadena = tabla + "_SP";
            sMensaje = string.Empty;

            try
            {
                llenarParametros();
                Global.Datos.SQLServer.EjecutarSPInsertUpdate(cadena, par);
                return true;
            }
            catch (Exception ex)
            {
                sMensaje = ex.Message;
                return false;
            }
        }

        private void llenarParametros()
        {

            par[0] = new SqlParameter();
            par[0].ParameterName = "@sClienteID";
            par[0].Value = _sClienteID;
            par[0].SqlDbType = SqlDbType.VarChar;


            par[1] = new SqlParameter();
            par[1].ParameterName = "@sRazonSocial";
            par[1].Value = _sRazonSocial;
            par[1].SqlDbType = SqlDbType.VarChar;


            par[2] = new SqlParameter();
            par[2].ParameterName = "@sCalle";
            par[2].Value = _sCalle;
            par[2].SqlDbType = SqlDbType.VarChar;


            par[3] = new SqlParameter();
            par[3].ParameterName = "@sAltura";
            par[3].Value = _sAltura;
            par[3].SqlDbType = SqlDbType.VarChar;


            par[4] = new SqlParameter();
            par[4].ParameterName = "@sLocalidad";
            par[4].Value = _sLocalidad;
            par[4].SqlDbType = SqlDbType.VarChar;


            par[5] = new SqlParameter();
            par[5].ParameterName = "@sProvincia";
            par[5].Value = _sProvincia;
            par[5].SqlDbType = SqlDbType.VarChar;


            par[6] = new SqlParameter();
            par[6].ParameterName = "@sCodigoPostal";
            par[6].Value = _sCodigoPostal;
            par[6].SqlDbType = SqlDbType.VarChar;


            par[7] = new SqlParameter();
            par[7].ParameterName = "@nPaisID";
            par[7].Value = _nPaisID;
            par[7].SqlDbType = SqlDbType.Int;


            par[8] = new SqlParameter();
            par[8].ParameterName = "@sTelefonoCelular";
            par[8].Value = _sTelefonoCelular;
            par[8].SqlDbType = SqlDbType.VarChar;


            par[9] = new SqlParameter();
            par[9].ParameterName = "@sTelefonoFijo";
            par[9].Value = _sTelefonoFijo;
            par[9].SqlDbType = SqlDbType.VarChar;


            par[10] = new SqlParameter();
            par[10].ParameterName = "@lActivo";
            par[10].Value = _lActivo;
            par[10].SqlDbType = SqlDbType.Bit;

        }

    }
}
