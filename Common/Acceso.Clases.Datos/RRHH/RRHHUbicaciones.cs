using System;
using System.Data;
using System.Data.SqlClient;

namespace Acceso.Clases.Datos.RRHH
{
    public class RRHHUbicaciones
    {
        string tabla = "RRHHUbicaciones";

        protected string _sUbicacionID;
        protected string _sDescripcion;
        public string sMensaje;
        public bool existe;


        private SqlParameter[] par = new SqlParameter[2];
        DataTable dt;

        public string sUbicacionID
        {
            set { _sUbicacionID = value; }
            get { return _sUbicacionID; }
        }

        public string sDescripcion
        {
            set { _sDescripcion = value; }
            get { return _sDescripcion; }
        }
        public void Inicializar()
        {
            string cadena;
            cadena = "Select * from RRHHUbicaciones where sUbicacionID = '" + _sUbicacionID + "'";
            sMensaje = string.Empty;
            existe = false;
            sDescripcion = string.Empty;

            try
            {
                dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(cadena);

                sDescripcion = dt.Rows[0]["sDescripcion"].ToString();
                existe = true;
            }
            catch (Exception e)
            {

                sMensaje = e.Message;

            }
        }
        public bool Actualizar()
        {
            string cadena;
            cadena = tabla + "_SP";
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

        public bool Eliminar()
        {
            string cadena = "RRHHUbicaciones_Delete";
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter();
            par[0].ParameterName = "@sUbicacionID";
            par[0].Value = _sUbicacionID;
            par[0].SqlDbType = SqlDbType.VarChar;

            try
            {
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
            par[0].ParameterName = "@sUbicacionID";
            par[0].Value = _sUbicacionID;
            par[0].SqlDbType = SqlDbType.VarChar;

            par[1] = new SqlParameter();
            par[1].ParameterName = "@sDescripcion";
            par[1].Value = _sDescripcion;
            par[1].SqlDbType = SqlDbType.VarChar;

        }
    }
}
