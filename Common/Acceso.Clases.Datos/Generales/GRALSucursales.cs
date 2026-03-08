using System;
using System.Data;
using System.Data.SqlClient;

namespace Acceso.Clases.Datos.Generales
{
    public class GRALSucursales
    {
        // Multi-tenant: tabla Sucursal
        protected string _sSucursalID;
        protected string _sSucursalGrupoID;
        protected string _sDescripcion;

        protected bool _lActivo;

        private SqlParameter[] par = new SqlParameter[3];
        DataTable dt;

        public bool Existe;

        public string sMensaje;

        #region Propiedades


        public string sSucursalID
        {

            set { _sSucursalID = value; }
            get { return _sSucursalID; }
        }

        public string sSucursalGrupoID
        {

            set { _sSucursalGrupoID = value; }
            get { return _sSucursalGrupoID; }
        }

        public string sDescripcion
        {
            set { _sDescripcion = value; }
            get { return _sDescripcion; }
        }

        #endregion
        public bool Eliminar()
        {
            string cadena = "Sucursal_Delete";
            SqlParameter[] par = new SqlParameter[2];
            par[0] = new SqlParameter();
            par[0].ParameterName = "@Id";
            par[0].Value = _sSucursalID;
            par[0].SqlDbType = SqlDbType.Int;

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
            // Multi-tenant: tabla Sucursal, filtro por EmpresaId
            int empresaId = Global.Datos.TenantContext.EmpresaId;
            string cadena = "SELECT * FROM Sucursal WHERE Id = " + _sSucursalID + " AND EmpresaId = " + empresaId;
            _sSucursalGrupoID = String.Empty;
            try
            {
                dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(cadena);

                _sDescripcion = dt.Rows[0]["Nombre"].ToString();
                Existe = true;
            }
            catch (Exception)
            {
                _sDescripcion = string.Empty;
                Existe = false;
            }
        }

        public bool Actualizar()
        {
            string cadena = "Sucursal_SP";
            sMensaje = string.Empty;

            if (!ControlarCampos())
            {
                return false;
            }

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

        private bool ControlarCampos()
        {
            if (sSucursalID.Length == 0)
            {
                sMensaje = "La Sucursal no puede estar vacio";
                return false;
            }

            return true;
        }

        private void llenarParametros()
        {
            par[0] = new SqlParameter();
            par[0].ParameterName = "@Id";
            par[0].Value = _sSucursalID;
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
