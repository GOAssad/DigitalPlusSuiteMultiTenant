using System;
using System.Data;
using System.Data.SqlClient;
using Global.Datos;
using System.Collections.Generic;

namespace Acceso.Clases.Datos.RRHH
{
    public class RRHHIncidenciasLegajos
    {
        private SqlParameter[] par = new SqlParameter[3];

        public string legajodesde;
        public string legajohasta;
        public DateTime fechaDesde;
        public DateTime fechaHasta;
        public string sMensaje;

        public List<RRHHIncidenciasLegajos> Lincleg;

        protected string _sIncidenciaID;
        protected string _sLegajoID;
        protected DateTime _dRegistro;

        

        #region Propiedades

        public string sIncidenciaID
        {
            set { _sIncidenciaID = value; }
            get { return _sIncidenciaID; }
        }

        public string sLegajoID
        {

            set { _sLegajoID = value; }
            get { return _sLegajoID; }
        }

        public DateTime dRegistro
        {

            set { _dRegistro = value; }
            get { return _dRegistro; }
        }
        #endregion



        public DataTable TraerAusencias(string cadena)
        {
            
            llenarparametros();
            return SQLServer.EjecutarSPSelect(cadena, par);
           

            //Lincleg = new List<RRHHIncidenciasLegajos>();
            //Lincleg = Global.Funciones.BasedeDatos.PasaraLista<RRHHIncidenciasLegajos>(dt);

        }

        public bool Eliminar()
        {

            string cadena = "RRHHIncidenciasLegajos_Delete";
            sMensaje = string.Empty;
            try
            {
                llenarparametrosActualizar();
                Global.Datos.SQLServer.EjecutarSPInsertUpdate(cadena, par);
                return true;
            }
            catch (Exception ex)
            {
                sMensaje = ex.Message;
                return false;
            }

        }

        

        public bool Actualizar(string cadena)
        {
            
            sMensaje = "";


            try
            {
                llenarparametrosActualizar();
                Global.Datos.SQLServer.EjecutarSPInsertUpdate(cadena, par);
                return true;
            }
            catch (Exception ex)
            {
                sMensaje = ex.Message;
                return false;
            }
        }

        private void llenarparametrosActualizar()
        {
            par[0] = new SqlParameter();
            par[0].ParameterName = "@sIncidenciaID";
            par[0].Value = _sIncidenciaID;
            par[0].SqlDbType = SqlDbType.VarChar;

            par[1] = new SqlParameter();
            par[1].ParameterName = "@sLegajoID";
            par[1].Value = _sLegajoID;
            par[1].SqlDbType = SqlDbType.VarChar;   

            par[2] = new SqlParameter();
            par[2].ParameterName = "@dRegistro";
            par[2].Value = _dRegistro;
            par[2].SqlDbType = SqlDbType.DateTime;
        }
        private void llenarparametros()
        {
            par[0] = new SqlParameter();
            par[0].ParameterName = "@ld";
            par[0].Value = legajodesde;
            par[0].SqlDbType = SqlDbType.VarChar;

            par[1] = new SqlParameter();
            par[1].ParameterName = "@lh";
            par[1].Value = legajohasta;
            par[1].SqlDbType = SqlDbType.VarChar;

            par[2] = new SqlParameter();
            par[2].ParameterName = "@fd";
            par[2].Value = fechaDesde;
            par[2].SqlDbType = SqlDbType.DateTime;
        }
    }
}
