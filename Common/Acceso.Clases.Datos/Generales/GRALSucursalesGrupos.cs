using System;
using System.Data;
using System.Data.SqlClient;

namespace Acceso.Clases.Datos.Generales
{
    public class GRALSucursalesGrupos
    {
        // Multi-tenant: redirigido a tabla Sucursal

        protected string _sSucursalGrupoID;
        protected string _sDescripcion;
        protected bool _lActivo;

        DataTable dt;

        public bool Existe;

        public string sMensaje;

        #region Propiedades

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

        public bool lActivo
        {
            set { _lActivo = value; }
            get { return _lActivo; }
        }

        #endregion

        public bool Eliminar()
        {
            sMensaje = string.Empty;
            if (!int.TryParse(_sSucursalGrupoID, out int id) || id <= 0)
            {
                sMensaje = "ID inválido";
                return false;
            }
            try
            {
                int empresaId = Global.Datos.TenantContext.EmpresaId;
                Global.Datos.SQLServer.EjecutarSPsinRespuesta(
                    "DELETE FROM Sucursal WHERE Id = " + id + " AND EmpresaId = " + empresaId, false);
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
            // Multi-tenant: tabla Sucursal
            _sDescripcion = string.Empty;
            Existe = false;
            if (!int.TryParse(_sSucursalGrupoID, out int id) || id <= 0) return;
            try
            {
                int empresaId = Global.Datos.TenantContext.EmpresaId;
                dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(
                    "SELECT * FROM Sucursal WHERE Id = " + id + " AND EmpresaId = " + empresaId);
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
            sMensaje = string.Empty;
            int empresaId = Global.Datos.TenantContext.EmpresaId;
            try
            {
                string nombre = (_sDescripcion ?? string.Empty).Replace("'", "''");
                if (int.TryParse(_sSucursalGrupoID, out int id) && id > 0)
                {
                    Global.Datos.SQLServer.EjecutarSPsinRespuesta(
                        "UPDATE Sucursal SET Nombre = '" + nombre + "' WHERE Id = " + id + " AND EmpresaId = " + empresaId, false);
                }
                else
                {
                    Global.Datos.SQLServer.EjecutarSPsinRespuesta(
                        "INSERT INTO Sucursal (Nombre, EmpresaId, Codigo, IsActive) VALUES ('" + nombre + "', " + empresaId + ", '', 1)", false);
                }
                return true;
            }
            catch (Exception ex)
            {
                sMensaje = ex.Message;
                return false;
            }
        }
    }
}
