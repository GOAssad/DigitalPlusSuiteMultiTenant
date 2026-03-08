using System;
using System.Data;
using System.Data.SqlClient;

namespace Acceso.Clases.Datos.RRHH
{
    public class RRHHFeriados
    {
        protected int _nFeriadoID;
        protected string _sDescripcion;
        protected DateTime _dDesde;
        protected DateTime _dHasta;
        public string sMensaje;
        public bool existe;

        private SqlParameter[] par = new SqlParameter[4];
        DataTable dt;


        public int nFeriadoID
        {
            set { _nFeriadoID = value; }
            get { return _nFeriadoID; }
        }

        public string sDescripcion
        {
            set { _sDescripcion = value; }
            get { return _sDescripcion; }
        }

        public DateTime dDesde
        {
            set { _dDesde = value; }
            get { return _dDesde; }
        }

        public DateTime dHasta
        {
            set { _dHasta = value; }
            get { return _dHasta; }
        }
        public void Inicializar()
        {
            // Multi-tenant: tabla Feriado, columnas Id, Nombre, Fecha, EmpresaId
            int empresaId = Global.Datos.TenantContext.EmpresaId;
            string cadena = "SELECT * FROM Feriado WHERE Id = " + _nFeriadoID + " AND EmpresaId = " + empresaId;
            sMensaje = string.Empty;
            existe = false;
            sDescripcion = string.Empty;

            try
            {
                dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(cadena);

                if (dt.Rows.Count > 0)
                {
                    sDescripcion = dt.Rows[0]["Nombre"].ToString();
                    // Feriado tiene una sola Fecha, la usamos como Desde y Hasta
                    DateTime fecha = Convert.ToDateTime(dt.Rows[0]["Fecha"]);
                    dDesde = fecha;
                    dHasta = fecha;
                    existe = true;
                }
                else
                {
                    sDescripcion = string.Empty;
                    dDesde = DateTime.Today;
                    dHasta = DateTime.Today;
                    existe = false;
                }
            }
            catch (Exception e)
            {
                sMensaje = e.Message;
            }
        }
        public bool Actualizar()
        {
            string cadena = "Feriado_SP";
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
            string cadena = "Feriado_Delete";
            SqlParameter[] par = new SqlParameter[2];
            par[0] = new SqlParameter();
            par[0].ParameterName = "@Id";
            par[0].Value = _nFeriadoID;
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
        public bool EsFeriado(DateTime fecha)
        {
            // Multi-tenant: buscar en tabla Feriado por Fecha y EmpresaId
            int empresaId = Global.Datos.TenantContext.EmpresaId;
            string cadena = "SELECT Id FROM Feriado WHERE Fecha = @Fecha AND EmpresaId = " + empresaId;
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter();
            par[0].ParameterName = "@Fecha";
            par[0].Value = fecha.Date;
            par[0].SqlDbType = SqlDbType.Date;

            dt = Global.Datos.SQLServer.EjecutarSPSelect(cadena, par, false);
            if (dt.Rows.Count > 0) return true;
            else return false;
        }
        private void llenarParametros()
        {
            par[0] = new SqlParameter();
            par[0].ParameterName = "@Id";
            par[0].Value = _nFeriadoID;
            par[0].SqlDbType = SqlDbType.Int;

            par[1] = new SqlParameter();
            par[1].ParameterName = "@Nombre";
            par[1].Value = _sDescripcion;
            par[1].SqlDbType = SqlDbType.NVarChar;

            // Feriado: una sola Fecha (usamos dDesde)
            par[2] = new SqlParameter();
            par[2].ParameterName = "@Fecha";
            par[2].Value = _dDesde.Date;
            par[2].SqlDbType = SqlDbType.Date;

            par[3] = new SqlParameter();
            par[3].ParameterName = "@EmpresaId";
            par[3].Value = Global.Datos.TenantContext.EmpresaId;
            par[3].SqlDbType = SqlDbType.Int;
        }

    }
}
