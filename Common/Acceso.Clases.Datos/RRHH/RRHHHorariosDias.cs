using System;
using System.Data;
using System.Data.SqlClient;

namespace Acceso.Clases.Datos.RRHH
{
    public class RRHHHorariosDias
    {
        protected string _sHorarioID;
        protected int _nDia;
        protected int _hd, _hh, _md, _mh;

        public int[,] horas = new int[7, 4];
        public string sMensaje;
        public bool existe;

        private SqlParameter[] par = new SqlParameter[6];
        DataTable dt;

        #region Propiedades

        public string sHorarioID
        {
            set { _sHorarioID = value; }
            get { return _sHorarioID; }
        }

        public int nDia
        {
            set { _nDia = value; }
            get { return _nDia; }
        }

        public int hd
        {
            set { _hd = value; }
            get { return _hd; }
        }

        public int hh
        {
            set { _hh = value; }
            get { return _hh; }
        }

        public int md
        {
            set { _md = value; }
            get { return _md; }
        }

        public int mh
        {
            set { _mh = value; }
            get { return _mh; }
        }
        #endregion

        public void Inicializar()
        {
            // Multi-tenant: tabla HorarioDetalle, child de Horario (sin EmpresaId directo)
            // Columnas: HorarioId, DiaSemana, HoraDesde, HoraHasta, IsCerrado
            string cadena = "SELECT * FROM HorarioDetalle WHERE HorarioId = " + _sHorarioID;
            sMensaje = string.Empty;
            existe = false;

            try
            {
                dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(cadena);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // DiaSemana es enum (0=Lunes...6=Domingo)
                    int dia = Convert.ToInt32(dt.Rows[i]["DiaSemana"]);
                    if (dia >= 0 && dia < 7)
                    {
                        // HoraDesde/HoraHasta son TimeOnly en BD → TimeSpan en ADO.NET
                        TimeSpan horaDesde = (TimeSpan)dt.Rows[i]["HoraDesde"];
                        TimeSpan horaHasta = (TimeSpan)dt.Rows[i]["HoraHasta"];
                        horas[dia, 0] = horaDesde.Hours;
                        horas[dia, 1] = horaDesde.Minutes;
                        horas[dia, 2] = horaHasta.Hours;
                        horas[dia, 3] = horaHasta.Minutes;
                        existe = true;
                    }
                }
            }
            catch (Exception e)
            {
                sMensaje = e.Message;
            }
        }
        public bool Actualizar()
        {
            string cadena = "HorarioDetalle_SP";
            try
            {
                for (int i = 0; i < 7; i++)
                {
                    nDia = i;
                    hd = horas[i, 0];
                    md = horas[i, 1];
                    hh = horas[i, 2];
                    mh = horas[i, 3];
                    llenarParametros();
                    if (!Global.Datos.SQLServer.EjecutarSPInsertUpdateBool(cadena, par))
                    {
                        sMensaje = "Error Intentando Actualizar el Horario";
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                sMensaje = e.Message;
                return false;
            }
            return true;
        }

        #region Parametros
        private void llenarParametros()
        {
            par[0] = new SqlParameter();
            par[0].ParameterName = "@HorarioId";
            par[0].Value = _sHorarioID;
            par[0].SqlDbType = SqlDbType.Int;

            par[1] = new SqlParameter();
            par[1].ParameterName = "@DiaSemana";
            par[1].Value = _nDia;
            par[1].SqlDbType = SqlDbType.Int;

            par[2] = new SqlParameter();
            par[2].ParameterName = "@HoraDesdeH";
            par[2].Value = _hd;
            par[2].SqlDbType = SqlDbType.Int;

            par[3] = new SqlParameter();
            par[3].ParameterName = "@HoraDesdeM";
            par[3].Value = _md;
            par[3].SqlDbType = SqlDbType.Int;

            par[4] = new SqlParameter();
            par[4].ParameterName = "@HoraHastaH";
            par[4].Value = _hh;
            par[4].SqlDbType = SqlDbType.Int;

            par[5] = new SqlParameter();
            par[5].ParameterName = "@HoraHastaM";
            par[5].Value = _mh;
            par[5].SqlDbType = SqlDbType.Int;
        }
        #endregion
    }
}
