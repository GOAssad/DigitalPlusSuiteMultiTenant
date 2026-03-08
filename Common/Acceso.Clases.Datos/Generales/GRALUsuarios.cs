using System;
using System.Data;
using System.Data.SqlClient;

namespace Acceso.Clases.Datos.Generales
{
    public class GRALUsuarios
    {
        // Multi-tenant: GRALUsuarios NO EXISTE (usa Identity del portal).
        // Implementación soft-fail para que el Administrador no crashee.

        protected string _sUsuarioID;
        protected string _sApellido;
        protected string _sNombre;
        protected string _sEmail;
        protected string _sPassword;
        protected bool _lActivo;

        private SqlParameter[] par = new SqlParameter[7];
        DataTable dt;

        public bool Existe;
        public string sMensaje;

        #region Propiedades

        public string sUsuarioID
        {
            set { _sUsuarioID = value; }
            get { return _sUsuarioID; }
        }

        public string sApellido
        {
            set { _sApellido = value; }
            get { return _sApellido; }
        }

        public string sNombre
        {
            set { _sNombre = value; }
            get { return _sNombre; }
        }

        public string sEmail
        {
            set { _sEmail = value; }
            get { return _sEmail; }
        }

        public string sClave
        {
            set { _sPassword = value; }
            get { return _sPassword; }
        }

        public bool lActivo
        {
            set { _lActivo = value; }
            get { return _lActivo; }
        }

        private int? _nNivel;

        public int? nNivel
        {
            get { return _nNivel; }
            set { _nNivel = value; }
        }

        #endregion
        public bool Eliminar()
        {
            // Soft-fail: tabla no existe en multi-tenant
            sMensaje = "Gestión de usuarios disponible desde el portal web";
            return false;
        }
        public bool Login(string sPassForm)
        {
            sMensaje = string.Empty;

            if (_sPassword == string.Empty)
                return false;

            if (sPassForm == _sPassword)
                return true;
            return false;

        }

        public void Inicializar()
        {
            sMensaje = string.Empty;

            // Soft-fail: verificar si la tabla existe
            try
            {
                dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(
                    "SELECT OBJECT_ID('GRALUsuarios') AS id");
                if (dt.Rows.Count > 0 && dt.Rows[0]["id"] != DBNull.Value)
                {
                    // Tabla existe (BD legacy), intentar leer
                    dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(
                        "SELECT * FROM GRALUsuarios WHERE sUsuarioID = '" + _sUsuarioID + "'");

                    _sApellido = dt.Rows[0]["sApellido"].ToString();
                    _sNombre = dt.Rows[0]["sNombre"].ToString();
                    _sEmail = dt.Rows[0]["sEmail"].ToString();
                    _sPassword = dt.Rows[0]["sPassword"].ToString() ?? "";
                    if (_sPassword.Length > 0)
                        _sPassword = Global.Funciones.Cadenas.DesencriptarString(_sPassword);
                    _lActivo = (bool)dt.Rows[0]["lActivo"];
                    _nNivel = (int)dt.Rows[0]["nNivel"];
                    Existe = true;
                }
                else
                {
                    // Multi-tenant: tabla no existe, soft-fail
                    _sApellido = "Admin";
                    _sNombre = "DigitalPlus";
                    _sEmail = "";
                    _sPassword = "";
                    _lActivo = true;
                    _nNivel = 99;
                    Existe = false;
                }
            }
            catch (Exception ex)
            {
                _sApellido = string.Empty;
                _sNombre = string.Empty;
                _sEmail = string.Empty;
                _sPassword = string.Empty;
                _lActivo = true;
                _nNivel = 0;

                sMensaje = ex.Message;
                Existe = false;
            }
        }

        public bool Actualizar()
        {
            // Soft-fail: tabla no existe en multi-tenant
            sMensaje = "Gestión de usuarios disponible desde el portal web";
            return false;
        }

        private bool ControlarCampos()
        {
            if (sUsuarioID.Length == 0)
                sMensaje = "El codigo de Usuario no puede estar vacio" + "\n";

            if (sApellido.Length == 0)
                sMensaje = "Ingrese el Apellido " + "\n";

            if (sNombre.Length == 0)
                sMensaje = "Ingrese el Nombre " + "\n";

            if (sMensaje.Length > 0)
                return false;

            return true;
        }

        public bool PerteneceASucursal(string sucursal, string usuario)
        {
            // Sin login activo → acceso completo
            if (string.IsNullOrEmpty(usuario))
                return true;

            return true;
        }

        private void llenarParametros()
        {
            par[0] = new SqlParameter();
            par[0].ParameterName = "@sUsuarioID";
            par[0].Value = _sUsuarioID;
            par[0].SqlDbType = SqlDbType.VarChar;

            par[1] = new SqlParameter();
            par[1].ParameterName = "@sApellido";
            par[1].Value = _sApellido;
            par[1].SqlDbType = SqlDbType.VarChar;

            par[2] = new SqlParameter();
            par[2].ParameterName = "@sNombre";
            par[2].Value = _sNombre;
            par[2].SqlDbType = SqlDbType.VarChar;

            par[3] = new SqlParameter();
            par[3].ParameterName = "@sEmail";
            par[3].Value = _sEmail;
            par[3].SqlDbType = SqlDbType.VarChar;

            par[4] = new SqlParameter();
            par[4].ParameterName = "@sPassword";
            par[4].Value = _sPassword;
            par[4].SqlDbType = SqlDbType.VarChar;

            par[5] = new SqlParameter();
            par[5].ParameterName = "@lActivo";
            par[5].Value = _lActivo;
            par[5].SqlDbType = SqlDbType.Bit;

            par[6] = new SqlParameter();
            par[6].ParameterName = "@nNivel";
            par[6].Value = _nNivel;
            par[6].SqlDbType = SqlDbType.Int;
        }
    }
}
