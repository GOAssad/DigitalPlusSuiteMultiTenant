using Global.Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acceso.Clases.Datos.RRHH
{
    public class RRHHLegajosTurnos
    {
        string tabla = "RRHHLegajosTurnos";
        

        protected string _sLegajoID;
        protected string _sHorarioID;
        protected DateTime _dEntrada;
        protected DateTime _dSalida;

		public string sMensaje;

        public DataTable dtLegajosTurnos;
        private SqlParameter[] par = new SqlParameter[4];
        public bool Existe;

        public string sLegajoID
        {
            set { _sLegajoID = value; }
            get { return _sLegajoID; }
        }


        public string  sHorarioID
        {
            get { return _sHorarioID; }
            set { _sHorarioID = value; }
        }

        public DateTime dEntrada {
            get {return _dEntrada; }
            set { _dEntrada = value; } 
        }
        public DateTime dSalida
        {
            get { return _dSalida; }
            set { _dSalida = value; }
        }

        public void Inicializar()
        {
            // RRHHLegajosTurnos no existe en DigitalPlus — devolver tabla vacía
            sMensaje = "";
            dtLegajosTurnos = new System.Data.DataTable();
            Existe = false;
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
            catch (Exception)
            {

                return false;
            }
        }

        public void BorrarTodos()
        {
            sMensaje = string.Empty;
            try
            {
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@sLegajoID", _sLegajoID);
                Global.Datos.SQLServer.EjecutarSPInsertUpdate("RRHHLegajosTurnos_SP_Delete", p);
            }
            catch (Exception ex)
            {

                sMensaje = ex.Message;
            }
        }
        private void llenarParametros()
        {

            par[0] = new SqlParameter();
            par[0].ParameterName = "@sLegajoID";
            par[0].Value = _sLegajoID;
            par[0].SqlDbType = SqlDbType.VarChar;

            par[1] = new SqlParameter();
            par[1].ParameterName = "@sHorarioID";
            par[1].Value = _sHorarioID;
            par[1].SqlDbType = SqlDbType.VarChar;

            par[2] = new SqlParameter();
            par[2].ParameterName = "@dEntrada";
            par[2].Value = _dEntrada;
            par[2].SqlDbType = SqlDbType.DateTime;

            par[3] = new SqlParameter();
            par[3].ParameterName = "@dSalida";
            par[3].Value = _dSalida;
            par[3].SqlDbType = SqlDbType.DateTime;


        }

    }
}
