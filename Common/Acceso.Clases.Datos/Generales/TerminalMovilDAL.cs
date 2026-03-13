using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace Acceso.Clases.Datos.Generales
{
    /// <summary>
    /// Acceso a datos para TerminalMovil y CodigoActivacionMovil (v2 - Terminal Movil).
    /// </summary>
    public class TerminalMovilDAL
    {
        /// <summary>
        /// Obtiene el dispositivo móvil activo de un legajo.
        /// </summary>
        public static DataTable ObtenerPorLegajo(int legajoId, int empresaId)
        {
            string sql = @"SELECT tm.Id, tm.DeviceId, tm.Nombre, tm.Plataforma,
                           tm.FechaRegistro, tm.UltimoUso, tm.Activo
                           FROM TerminalesMoviles tm (NOLOCK)
                           WHERE tm.LegajoId = @LegajoId AND tm.EmpresaId = @EmpresaId AND tm.Activo = 1";

            SqlParameter[] par = new SqlParameter[]
            {
                new SqlParameter("@LegajoId", SqlDbType.Int) { Value = legajoId },
                new SqlParameter("@EmpresaId", SqlDbType.Int) { Value = empresaId }
            };

            return Global.Datos.SQLServer.EjecutarSPSelect(sql, par, false);
        }

        /// <summary>
        /// Genera un código de activación para vincular un dispositivo a un empleado.
        /// Retorna el código generado (8 caracteres alfanuméricos mayúscula).
        /// </summary>
        public static string GenerarCodigo(int legajoId, int empresaId)
        {
            string codigo = GenerarCodigoAleatorio(8);

            string sql = @"INSERT INTO CodigosActivacionMovil
                           (EmpresaId, LegajoId, Codigo, FechaCreacion, FechaExpira, Usado)
                           VALUES (@EmpresaId, @LegajoId, @Codigo, GETDATE(), DATEADD(HOUR, 24, GETDATE()), 0)";

            SqlParameter[] par = new SqlParameter[]
            {
                new SqlParameter("@EmpresaId", SqlDbType.Int) { Value = empresaId },
                new SqlParameter("@LegajoId", SqlDbType.Int) { Value = legajoId },
                new SqlParameter("@Codigo", SqlDbType.NVarChar, 10) { Value = codigo }
            };

            Global.Datos.SQLServer.EjecutarSPInsertUpdateBool(sql, par, false);
            return codigo;
        }

        /// <summary>
        /// Desactiva un dispositivo móvil.
        /// </summary>
        public static bool Desactivar(int terminalMovilId)
        {
            string sql = "UPDATE TerminalesMoviles SET Activo = 0 WHERE Id = @Id";

            SqlParameter[] par = new SqlParameter[]
            {
                new SqlParameter("@Id", SqlDbType.Int) { Value = terminalMovilId }
            };

            return Global.Datos.SQLServer.EjecutarSPInsertUpdateBool(sql, par, false);
        }

        /// <summary>
        /// Lista todos los dispositivos móviles de una empresa (activos e inactivos).
        /// </summary>
        public static DataTable ListarPorEmpresa(int empresaId)
        {
            string sql = @"SELECT tm.Id, tm.LegajoId, l.Apellido + ', ' + l.Nombre AS Empleado,
                           tm.Nombre AS Dispositivo, tm.Plataforma, tm.FechaRegistro,
                           tm.UltimoUso, tm.Activo
                           FROM TerminalesMoviles tm (NOLOCK)
                           INNER JOIN Legajo l (NOLOCK) ON l.Id = tm.LegajoId
                           WHERE tm.EmpresaId = @EmpresaId
                           ORDER BY tm.Activo DESC, tm.UltimoUso DESC";

            SqlParameter[] par = new SqlParameter[]
            {
                new SqlParameter("@EmpresaId", SqlDbType.Int) { Value = empresaId }
            };

            return Global.Datos.SQLServer.EjecutarSPSelect(sql, par, false);
        }

        private static string GenerarCodigoAleatorio(int longitud)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789"; // sin I,O,0,1 para evitar confusion
            byte[] randomBytes = new byte[longitud];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }

            char[] result = new char[longitud];
            for (int i = 0; i < longitud; i++)
            {
                result[i] = chars[randomBytes[i] % chars.Length];
            }
            return new string(result);
        }
    }
}
