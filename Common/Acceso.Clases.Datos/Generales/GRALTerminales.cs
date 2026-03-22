using System;
using System.Data;
using System.Data.SqlClient;

namespace Acceso.Clases.Datos.Generales
{
    public class GRALTerminales
    {
        // Multi-tenant: tabla Terminal

        private SqlParameter[] par = new SqlParameter[4];
        DataTable dt;

        public bool Existe;

        public string sMensaje;

        #region Propiedades

        public int nId { get; set; }
        public string sTerminalID { get; set; }
        public GRALSucursales sSucursalID = new GRALSucursales();
        public string sDescripcion { get; set; }
        public string sMensajeBienVenida { get; set; }
        public string sIPV4 { get; set; }

        public bool lActivo { get; set; }

        #endregion


        public bool Eliminar()
        {
            sMensaje = string.Empty;

            string cadena = "Terminal_Delete";
            SqlParameter[] par = new SqlParameter[2];
            par[0] = new SqlParameter();
            par[0].ParameterName = "@Nombre";
            par[0].Value = sTerminalID;
            par[0].SqlDbType = SqlDbType.NVarChar;

            par[1] = new SqlParameter();
            par[1].ParameterName = "@EmpresaId";
            par[1].Value = Global.Datos.TenantContext.EmpresaId;
            par[1].SqlDbType = SqlDbType.Int;

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
        public void Inicializar()
        {
            // Multi-tenant: tabla Terminal, filtro por EmpresaId
            int empresaId = Global.Datos.TenantContext.EmpresaId;
            string cadena = "SELECT * FROM Terminal WHERE Nombre = '" + sTerminalID + "' AND EmpresaId = " + empresaId;

            try
            {
                dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(cadena);

                nId = Convert.ToInt32(dt.Rows[0]["Id"]);
                sDescripcion = dt.Rows[0]["Descripcion"].ToString();
                sMensajeBienVenida = "Hola!";
                sIPV4 = "";

                sSucursalID.sSucursalID = dt.Rows[0]["SucursalId"].ToString();
                sSucursalID.Inicializar();

                Existe = true;
            }
            catch (Exception)
            {
                sDescripcion = string.Empty;
                sSucursalID.sSucursalID = string.Empty;
                sMensajeBienVenida = string.Empty;
                sIPV4 = string.Empty;
                Existe = false;
            }
        }

        public bool Actualizar()
        {
            string cadena = "Terminal_SP";
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
            par[0].ParameterName = "@Nombre";
            par[0].Value = sTerminalID;
            par[0].SqlDbType = SqlDbType.NVarChar;

            par[1] = new SqlParameter();
            par[1].ParameterName = "@Descripcion";
            par[1].Value = sDescripcion;
            par[1].SqlDbType = SqlDbType.NVarChar;

            par[2] = new SqlParameter();
            par[2].ParameterName = "@SucursalId";
            par[2].Value = sSucursalID.sSucursalID;
            par[2].SqlDbType = SqlDbType.Int;

            par[3] = new SqlParameter();
            par[3].ParameterName = "@EmpresaId";
            par[3].Value = Global.Datos.TenantContext.EmpresaId;
            par[3].SqlDbType = SqlDbType.Int;
        }

    }
}
