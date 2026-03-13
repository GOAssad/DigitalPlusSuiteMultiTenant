using System;
using System.Data;
using System.Data.SqlClient;

namespace Acceso.Clases.Datos.Generales
{
    /// <summary>
    /// Acceso a datos para SucursalGeoconfig (v2 - Terminal Movil).
    /// </summary>
    public class SucursalGeoconfigDAL
    {
        /// <summary>
        /// Obtiene la geoconfiguración de una sucursal.
        /// </summary>
        public static DataTable ObtenerPorSucursal(int sucursalId)
        {
            string sql = @"SELECT Id, SucursalId, EmpresaId, WifiBSSID, WifiSSID,
                           Latitud, Longitud, RadioMetros, MetodoValidacion, Activo
                           FROM SucursalGeoconfigs (NOLOCK)
                           WHERE SucursalId = @SucursalId";

            SqlParameter[] par = new SqlParameter[]
            {
                new SqlParameter("@SucursalId", SqlDbType.Int) { Value = sucursalId }
            };

            return Global.Datos.SQLServer.EjecutarSPSelect(sql, par, false);
        }

        /// <summary>
        /// Guarda o actualiza la geoconfiguración de una sucursal.
        /// Si ya existe, actualiza; si no, inserta.
        /// </summary>
        public static bool Guardar(int sucursalId, int empresaId, string wifiBssid, string wifiSsid,
            decimal? latitud, decimal? longitud, int radioMetros, string metodoValidacion, bool activo)
        {
            string sql = @"
                IF EXISTS (SELECT 1 FROM SucursalGeoconfigs WHERE SucursalId = @SucursalId)
                BEGIN
                    UPDATE SucursalGeoconfigs SET
                        WifiBSSID = @WifiBSSID, WifiSSID = @WifiSSID,
                        Latitud = @Latitud, Longitud = @Longitud,
                        RadioMetros = @RadioMetros, MetodoValidacion = @MetodoValidacion,
                        Activo = @Activo
                    WHERE SucursalId = @SucursalId
                END
                ELSE
                BEGIN
                    INSERT INTO SucursalGeoconfigs
                        (SucursalId, EmpresaId, WifiBSSID, WifiSSID, Latitud, Longitud, RadioMetros, MetodoValidacion, Activo)
                    VALUES
                        (@SucursalId, @EmpresaId, @WifiBSSID, @WifiSSID, @Latitud, @Longitud, @RadioMetros, @MetodoValidacion, @Activo)
                END";

            SqlParameter[] par = new SqlParameter[]
            {
                new SqlParameter("@SucursalId", SqlDbType.Int) { Value = sucursalId },
                new SqlParameter("@EmpresaId", SqlDbType.Int) { Value = empresaId },
                new SqlParameter("@WifiBSSID", SqlDbType.NVarChar, 50) { Value = (object)wifiBssid ?? DBNull.Value },
                new SqlParameter("@WifiSSID", SqlDbType.NVarChar, 100) { Value = (object)wifiSsid ?? DBNull.Value },
                new SqlParameter("@Latitud", SqlDbType.Decimal) { Value = latitud.HasValue ? (object)latitud.Value : DBNull.Value },
                new SqlParameter("@Longitud", SqlDbType.Decimal) { Value = longitud.HasValue ? (object)longitud.Value : DBNull.Value },
                new SqlParameter("@RadioMetros", SqlDbType.Int) { Value = radioMetros },
                new SqlParameter("@MetodoValidacion", SqlDbType.NVarChar, 20) { Value = metodoValidacion },
                new SqlParameter("@Activo", SqlDbType.Bit) { Value = activo }
            };

            return Global.Datos.SQLServer.EjecutarSPInsertUpdateBool(sql, par, false);
        }

        /// <summary>
        /// Lista todas las geoconfiguraciones de la empresa con nombre de sucursal.
        /// </summary>
        public static DataTable ListarPorEmpresa(int empresaId)
        {
            string sql = @"SELECT sg.Id, sg.SucursalId, s.Nombre AS SucursalNombre,
                           sg.WifiBSSID, sg.WifiSSID, sg.Latitud, sg.Longitud,
                           sg.RadioMetros, sg.MetodoValidacion, sg.Activo
                           FROM SucursalGeoconfigs sg (NOLOCK)
                           INNER JOIN Sucursal s (NOLOCK) ON s.Id = sg.SucursalId
                           WHERE sg.EmpresaId = @EmpresaId
                           ORDER BY s.Nombre";

            SqlParameter[] par = new SqlParameter[]
            {
                new SqlParameter("@EmpresaId", SqlDbType.Int) { Value = empresaId }
            };

            return Global.Datos.SQLServer.EjecutarSPSelect(sql, par, false);
        }

        /// <summary>
        /// Elimina la geoconfiguración de una sucursal.
        /// </summary>
        public static bool Eliminar(int sucursalGeoconfigId)
        {
            string sql = "DELETE FROM SucursalGeoconfigs WHERE Id = @Id";

            SqlParameter[] par = new SqlParameter[]
            {
                new SqlParameter("@Id", SqlDbType.Int) { Value = sucursalGeoconfigId }
            };

            return Global.Datos.SQLServer.EjecutarSPInsertUpdateBool(sql, par, false);
        }
    }
}
