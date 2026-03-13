using Acceso.Clases.Datos.Generales;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Acceso.Clases.Datos.RRHH
{
    public class RRHHLegajos
    {

        protected string _sLegajoID;
        protected string _sApellido;
        protected string _sNombre;
        protected int _nSector;
        protected int _nCategoria;
        protected bool _lActivo;
        protected string _sHorarioID;
        protected string _sSucursalID;
        protected bool _lSeguimiento;
        protected byte[] _iFoto;

        private SqlParameter[] par = new SqlParameter[11];
        DataTable dt;

        public bool Existe;

        public string sMensaje;
        public bool SeguimientoActivado;

        // Id entero de Legajo (necesario para EscritorioLegajosHuellasActualizar)
        public int nLegajoID { get; set; }

        public GRALSucursales oSucursalID = new GRALSucursales();

        public RRHHLegajosDomicilios oDomicilio = new RRHHLegajosDomicilios();

        public RRHHLegajos()
        {
        }

        #region Propiedades


        public string sLegajoID
        {

            set { _sLegajoID = value; }
            get { return _sLegajoID; }
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

        public int nSector
        {
            set { _nSector = value; }
            get { return _nSector; }
        }

        public bool lActivo
        {
            set { _lActivo = value; }
            get { return _lActivo; }
        }

        public bool lSeguimiento
        {
            set { _lSeguimiento = value; }
            get { return _lSeguimiento; }
        }

        public int nCategoria
        {
            set { _nCategoria = value; }
            get { return _nCategoria; }
        }

        public string sHorarioID
        {

            set { _sHorarioID = value; }
            get { return _sHorarioID; }
        }

        public string sSucursalID
        {
            set { _sSucursalID = value; }
            get { return _sSucursalID; }
        }

        public byte[] iFoto
        {
            set { _iFoto = value; }
            get { return _iFoto; }
        }

        private int _TotalHuellas;
        public int TotalHuellas
        {
            get { return _TotalHuellas; }
            set { _TotalHuellas = value; }
        }

        private int _QueDedos;
        public int QueDedos
        {
            get { return _QueDedos; }
            set { _QueDedos = value; }
        }

        private string _ultLegajo;
        public string ultLegajo
        {
            get { return ultimoLegajo(); }
            set { _ultLegajo = value; }
        }

        private int _Ausencias;

        public int Ausencias
        {
            get { return _Ausencias; }
            set { _Ausencias = value; }
        }

        private int _Incidencias;

        public int Incidencias
        {
            get { return _Incidencias; }
            set { _Incidencias = value; }
        }


        public int TotalFichadas;
        public decimal Eficiencia;

        #endregion

        public bool eliminar()
        {
            sMensaje = string.Empty;
            try
            {
                // Multi-tenant: SP con EmpresaId
                SqlParameter[] delPar = new SqlParameter[2];
                delPar[0] = new SqlParameter("@NumeroLegajo", SqlDbType.VarChar) { Value = sLegajoID };
                delPar[1] = new SqlParameter("@EmpresaId", SqlDbType.Int) { Value = Global.Datos.TenantContext.EmpresaId };
                Global.Datos.SQLServer.EjecutarSPInsertUpdate("RRHHLegajos_DeleteTodo", delPar);
            }
            catch (Exception e)
            {
                sMensaje = sMensaje + "\n\r" + e.Message;
                return false;
            }

            return true;
        }

        public void Inicializar()
        {
            // Multi-tenant: tabla Legajo, columnas NumeroLegajo, Apellido, Nombre, IsActive, HasCalendarioPersonalizado
            // LegajoSucursal: child table sin EmpresaId, JOIN por LegajoId
            int empresaId = Global.Datos.TenantContext.EmpresaId;
            string cadena =
                "SELECT l.Id, l.NumeroLegajo, l.Apellido, l.Nombre, l.SectorId, l.CategoriaId, " +
                "l.HorarioId, l.IsActive, l.HasCalendarioPersonalizado, l.Foto, " +
                "ISNULL(ls.SucursalId, 0) AS SucursalId " +
                "FROM Legajo l " +
                "LEFT JOIN LegajoSucursal ls ON l.Id = ls.LegajoId " +
                "WHERE l.NumeroLegajo = '" + _sLegajoID + "' AND l.EmpresaId = " + empresaId;

            Existe = false;

            try
            {
                dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(cadena);

                _iFoto = dt.Rows[0]["Foto"] == DBNull.Value ? null : (byte[])dt.Rows[0]["Foto"];

                nLegajoID = (int)dt.Rows[0]["Id"];

                _sApellido = dt.Rows[0]["Apellido"].ToString();
                _sNombre   = dt.Rows[0]["Nombre"].ToString();

                _nSector    = (int)dt.Rows[0]["SectorId"];
                _nCategoria = (int)dt.Rows[0]["CategoriaId"];
                _sHorarioID = dt.Rows[0]["HorarioId"] == DBNull.Value
                    ? string.Empty
                    : dt.Rows[0]["HorarioId"].ToString();
                _lActivo      = (bool)dt.Rows[0]["IsActive"];
                _lSeguimiento = (bool)dt.Rows[0]["HasCalendarioPersonalizado"];
                _sSucursalID  = dt.Rows[0]["SucursalId"].ToString();

                oSucursalID.sSucursalID = _sSucursalID;
                oSucursalID.Inicializar();

                // Total huellas enroladas (LegajoHuella: child, JOIN por LegajoId)
                cadena = "SELECT COUNT(*) AS Cantidad FROM LegajoHuella WHERE LegajoId = " + nLegajoID;
                dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(cadena);
                _TotalHuellas = Convert.ToInt32(dt.Rows[0]["Cantidad"]);

                // Máscara de dedos enrolados
                _QueDedos = Int32.Parse(Global.Datos.SQLServer.EjecutarParaSoloLectura(
                    "SELECT ISNULL(SUM(FingerMask), 0) Cantidad " +
                    "FROM LegajoHuella WHERE LegajoId = " + nLegajoID).Rows[0][0].ToString());

                Existe = true;
            }
            catch (Exception)
            {
                _sApellido    = "";
                _sNombre      = "";
                _nSector      = 0;
                _nCategoria   = 0;
                _QueDedos     = 0;
                _sHorarioID   = String.Empty;
                _lActivo      = false;
                _lSeguimiento = false;
                _iFoto        = null;
                _sSucursalID  = String.Empty;
                nLegajoID     = 0;

                Existe = false;
            }

            oDomicilio.sLegajoID = _sLegajoID;
            oDomicilio.Inicializar();
        }

        public bool Actualizar()
        {
            // EscritorioLegajoActualizar: multi-tenant con @EmpresaId
            string cadena = "EscritorioLegajoActualizar";
            if (!ControlarCampos())
                return false;

            try
            {
                llenarParametros();
                Global.Datos.SQLServer.EjecutarSPInsertUpdate(cadena, par);

                // Recuperar el Id (PK int) del legajo recién insertado/actualizado
                int empresaId = Global.Datos.TenantContext.EmpresaId;
                dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(
                    "SELECT Id FROM Legajo WHERE NumeroLegajo = '" + _sLegajoID + "' AND EmpresaId = " + empresaId);
                if (dt.Rows.Count > 0)
                    nLegajoID = (int)dt.Rows[0]["Id"];

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
            // Codigo vacio
            if (sLegajoID.Length == 0)
            {
                sMensaje = "El legajo no puede estar vacio";
                return false;
            }

            return true;
        }
        private string ultimoLegajo()
        {
            int empresaId = Global.Datos.TenantContext.EmpresaId;
            return Global.Datos.SQLServer.EjecutarParaSoloLectura(
                "SELECT ISNULL(MAX(NumeroLegajo),'') sLegajoID FROM Legajo WHERE EmpresaId = " + empresaId).Rows[0][0].ToString();
        }
        public DataTable todaslasFichadas()
        {
            // Multi-tenant: tabla Fichada, columna FechaHora, tabla Sucursal
            int empresaId = Global.Datos.TenantContext.EmpresaId;
            string cadena =
                "SELECT s.Nombre AS Sucursal, f.FechaHora AS [Fecha y Hora] " +
                "FROM Fichada f " +
                "INNER JOIN Sucursal s ON f.SucursalId = s.Id " +
                "INNER JOIN Legajo l ON f.LegajoId = l.Id " +
                "WHERE l.NumeroLegajo = '" + sLegajoID + "' AND f.EmpresaId = " + empresaId +
                " ORDER BY f.FechaHora DESC";
            return Global.Datos.SQLServer.EjecutarParaSoloLectura(cadena);
        }

        public void CantidadAusencias(DateTime fdesde, DateTime fhasta)
        {
            // SPs de DigitalOne no existen en multi-tenant; retornar valores neutros
            Ausencias     = 0;
            Incidencias   = 0;
            TotalFichadas = 0;
            Eficiencia    = 100.00M;
        }

        public DataTable ActividadDiaria()
        {
            // SP RRHHActividadLegajo_SP_SELECT no existe en multi-tenant
            return new DataTable();
        }

        private void llenarParametros()
        {
            // EscritorioLegajoActualizar: @NumeroLegajo, @Apellido, @Nombre, @SectorId, @CategoriaId,
            //   @IsActive, @HorarioID, @HasCalendarioPersonalizado, @nSucursalId, @EmpresaId
            par[0] = new SqlParameter();
            par[0].ParameterName = "@NumeroLegajo";
            par[0].Value         = _sLegajoID;
            par[0].SqlDbType     = SqlDbType.VarChar;

            par[1] = new SqlParameter();
            par[1].ParameterName = "@Apellido";
            par[1].Value     = (_sApellido ?? string.Empty).Trim();
            par[1].SqlDbType = SqlDbType.NVarChar;

            par[2] = new SqlParameter();
            par[2].ParameterName = "@Nombre";
            par[2].Value     = (_sNombre ?? string.Empty).Trim();
            par[2].SqlDbType = SqlDbType.NVarChar;

            par[3] = new SqlParameter();
            par[3].ParameterName = "@SectorId";
            par[3].Value         = _nSector;
            par[3].SqlDbType     = SqlDbType.Int;

            par[4] = new SqlParameter();
            par[4].ParameterName = "@CategoriaId";
            par[4].Value         = _nCategoria;
            par[4].SqlDbType     = SqlDbType.Int;

            par[5] = new SqlParameter();
            par[5].ParameterName = "@IsActive";
            par[5].Value         = _lActivo;
            par[5].SqlDbType     = SqlDbType.Bit;

            // HorarioID es nullable en Legajo
            par[6] = new SqlParameter();
            par[6].ParameterName = "@HorarioID";
            par[6].SqlDbType     = SqlDbType.Int;
            if (int.TryParse(_sHorarioID, out int hId) && hId > 0)
                par[6].Value = hId;
            else
                par[6].Value = DBNull.Value;

            // HasCalendarioPersonalizado equivale a lSeguimiento
            par[7] = new SqlParameter();
            par[7].ParameterName = "@HasCalendarioPersonalizado";
            par[7].Value         = _lSeguimiento;
            par[7].SqlDbType     = SqlDbType.Bit;

            par[8] = new SqlParameter();
            par[8].ParameterName = "@nSucursalId";
            par[8].SqlDbType     = SqlDbType.Int;
            if (int.TryParse(_sSucursalID, out int sId) && sId > 0)
                par[8].Value = sId;
            else
                par[8].Value = 0;

            par[9] = new SqlParameter();
            par[9].ParameterName = "@EmpresaId";
            par[9].Value         = Global.Datos.TenantContext.EmpresaId;
            par[9].SqlDbType     = SqlDbType.Int;

            par[10] = new SqlParameter();
            par[10].ParameterName = "@Foto";
            par[10].SqlDbType     = SqlDbType.VarBinary;
            par[10].Value         = _iFoto != null ? (object)_iFoto : DBNull.Value;
        }


    }

}
