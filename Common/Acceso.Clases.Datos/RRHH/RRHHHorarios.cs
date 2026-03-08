using System;
using System.Data;
using System.Data.SqlClient;

namespace Acceso.Clases.Datos.RRHH
{
    public class RRHHHorarios
    {
        protected string _sHorarioID;
        protected string _sDescripcion;
        public string sMensaje;
        public bool existe;

        private SqlParameter[] par = new SqlParameter[3];
        DataTable dt;

        public string sHorarioID
        {
            set { _sHorarioID = value; }
            get { return _sHorarioID; }
        }

        public string sDescripcion
        {
            set { _sDescripcion = value; }
            get { return _sDescripcion; }
        }
        public void Inicializar()
        {
            // Multi-tenant: tabla Horario, columna Nombre
            int empresaId = Global.Datos.TenantContext.EmpresaId;
            string cadena = "SELECT * FROM Horario WHERE Id = " + _sHorarioID + " AND EmpresaId = " + empresaId;
            sMensaje = string.Empty;
            existe = false;
            sDescripcion = string.Empty;

            try
            {
                dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(cadena);

                sDescripcion = dt.Rows[0]["Nombre"].ToString();
                existe = true;
            }
            catch (Exception e)
            {
                sMensaje = e.Message;
            }
        }
        public bool Actualizar()
        {
            string cadena = "Horario_SP";
            try
            {
                llenarParametros();
                Global.Datos.SQLServer.EjecutarSPInsertUpdate(cadena, par);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void llenarParametros()
        {
            par[0] = new SqlParameter();
            par[0].ParameterName = "@Id";
            par[0].Value = _sHorarioID;
            par[0].SqlDbType = SqlDbType.Int;

            par[1] = new SqlParameter();
            par[1].ParameterName = "@Nombre";
            par[1].Value = _sDescripcion;
            par[1].SqlDbType = SqlDbType.NVarChar;

            par[2] = new SqlParameter();
            par[2].ParameterName = "@EmpresaId";
            par[2].Value = Global.Datos.TenantContext.EmpresaId;
            par[2].SqlDbType = SqlDbType.Int;
        }
    }
}
