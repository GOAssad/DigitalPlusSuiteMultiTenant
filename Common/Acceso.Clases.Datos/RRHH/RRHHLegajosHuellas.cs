using System;
using System.Data;
using System.Data.SqlClient;
using Global.Datos;

namespace Acceso.Clases.Datos.RRHH
{
    public class RRHHLegajosHuellas
    {
        public string sLegajoID { get; set; }
        public int nLegajoID { get; set; }
        public int nDedo { get; set; }
        public byte[] iHuella { get; set; }
        public int nFingerMask { get; set; }
        public string sLegajoNombre { get; set; }
        public string sMensajeError;

        public DataTable dtLegajosHuellas;
        public DataTable dtFichadas;


        private SqlParameter[] par = new SqlParameter[6];
        public bool Existe;


        public void Inicializar()
        {
            // Multi-tenant: tabla LegajoHuella, columnas LegajoId, DedoId, Huella, FingerMask
            // LegajoHuella es child table (sin EmpresaId), se filtra por LegajoId
            // Necesitamos el Id del legajo, buscamos por NumeroLegajo + EmpresaId
            int empresaId = TenantContext.EmpresaId;
            string cadena = "SELECT lh.LegajoId AS nLegajoId, l.NumeroLegajo AS sLegajoID, " +
                            "lh.DedoId AS nDedo, lh.Huella AS iHuella, lh.FingerMask AS nFingerMask, " +
                            "l.Apellido + ', ' + l.Nombre AS sLegajoNombre " +
                            "FROM LegajoHuella lh " +
                            "INNER JOIN Legajo l ON lh.LegajoId = l.Id " +
                            "WHERE l.NumeroLegajo = '" + sLegajoID + "' AND l.EmpresaId = " + empresaId;

            sMensajeError = "";
            try
            {
                dtLegajosHuellas = SQLServer.EjecutarParaSoloLectura(cadena);

                Existe = true;
            }
            catch (Exception)
            {
                Existe = false;
            }
        }

        public bool Actualizar()
        {
            string cadena = "EscritorioLegajosHuellasActualizar";
            try
            {
                llenarParametros();
                SQLServer.EjecutarSPInsertUpdate(cadena, par);
                return true;
            }
            catch (Exception e)
            {
                sMensajeError = e.Message;
                return false;
            }
        }



        public void TodasLasHuellas()
        {
            // Multi-tenant: vista con filtro por EmpresaId
            int empresaId = TenantContext.EmpresaId;
            sMensajeError = "";
            try
            {
                string cadena = "SELECT lh.LegajoId AS nLegajoId, l.NumeroLegajo AS sLegajoID, " +
                                "lh.DedoId AS nDedo, lh.Huella AS iHuella, lh.FingerMask AS nFingerMask, " +
                                "l.Apellido + ', ' + l.Nombre AS sLegajoNombre " +
                                "FROM LegajoHuella lh " +
                                "INNER JOIN Legajo l ON lh.LegajoId = l.Id " +
                                "WHERE l.EmpresaId = " + empresaId + " AND l.IsActive = 1";
                dtLegajosHuellas = SQLServer.EjecutarParaSoloLectura(cadena);
            }
            catch (Exception ex)
            {

                sMensajeError = ex.Message;
            }

        }


        private void llenarParametros()
        {
            // EscritorioLegajosHuellasActualizar: @LegajoId int, @DedoId int, @Huella varbinary,
            //   @FingerMask int, @sLegajoNombre varchar, @sLegajoID varchar
            par[0] = new SqlParameter();
            par[0].ParameterName = "@LegajoId";
            par[0].Value = nLegajoID;
            par[0].SqlDbType = SqlDbType.Int;

            par[1] = new SqlParameter();
            par[1].ParameterName = "@DedoId";
            par[1].Value = nDedo;
            par[1].SqlDbType = SqlDbType.Int;

            par[2] = new SqlParameter();
            par[2].ParameterName = "@Huella";
            par[2].Value = iHuella;
            par[2].SqlDbType = SqlDbType.VarBinary;

            par[3] = new SqlParameter();
            par[3].ParameterName = "@FingerMask";
            par[3].Value = nFingerMask;
            par[3].SqlDbType = SqlDbType.Int;

            par[4] = new SqlParameter();
            par[4].ParameterName = "@sLegajoNombre";
            par[4].Value = sLegajoNombre;
            par[4].SqlDbType = SqlDbType.VarChar;

            par[5] = new SqlParameter();
            par[5].ParameterName = "@sLegajoID";
            par[5].Value = sLegajoID;
            par[5].SqlDbType = SqlDbType.VarChar;
        }

    }

}
