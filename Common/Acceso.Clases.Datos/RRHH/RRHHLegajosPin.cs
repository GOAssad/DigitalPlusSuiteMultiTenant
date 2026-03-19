using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Acceso.Clases.Datos.RRHH
{
    public class RRHHLegajosPin
    {
        public int nLegajoID { get; set; }
        public string sLegajoID { get; set; }
        public string sLegajoNombre { get; set; }
        public bool PinMustChange { get; set; }
        public bool PinExpired { get; set; }
        public bool HasPin { get; set; }

        /// <summary>
        /// Verifica el PIN ingresado contra el hash almacenado.
        /// Retorna true si el legajo existe, tiene PIN y el PIN coincide.
        /// </summary>
        public bool VerificarPin(string legajoId, string pinIngresado)
        {
            var par = new SqlParameter[]
            {
                new SqlParameter("@EmpresaId", SqlDbType.Int) { Value = Global.Datos.TenantContext.EmpresaId },
                new SqlParameter("@sLegajoID", SqlDbType.NVarChar) { Value = legajoId }
            };

            DataTable dt = Global.Datos.SQLServer.EjecutarSPSelect("EscritorioLegajoPIN_Verificar", par);

            if (dt.Rows.Count == 0)
                return false;

            var row = dt.Rows[0];
            nLegajoID = Convert.ToInt32(row["nLegajoID"]);
            sLegajoID = row["sLegajoID"].ToString();
            sLegajoNombre = row["sLegajoNombre"].ToString();

            string storedHash = row["PinHash"] == DBNull.Value ? null : row["PinHash"].ToString();
            string storedSalt = row["PinSalt"] == DBNull.Value ? null : row["PinSalt"].ToString();
            HasPin = !string.IsNullOrEmpty(storedHash);

            if (!HasPin)
                return false;

            PinMustChange = row["PinMustChange"] != DBNull.Value && Convert.ToBoolean(row["PinMustChange"]);

            // Verificar expiracion
            PinExpired = false;
            if (row["PinChangedAt"] != DBNull.Value)
            {
                int diasExpira = ObtenerDiasExpiracion();
                if (diasExpira > 0)
                {
                    DateTime pinChangedAt = Convert.ToDateTime(row["PinChangedAt"]);
                    PinExpired = (DateTime.UtcNow - pinChangedAt).TotalDays > diasExpira;
                }
            }

            // Comparar hash
            string hashIngresado = ComputeHash(pinIngresado, storedSalt);
            return string.Equals(hashIngresado, storedHash, StringComparison.Ordinal);
        }

        /// <summary>
        /// Cambia el PIN de un legajo. Genera salt nuevo y hashea.
        /// </summary>
        public bool CambiarPin(string legajoId, string nuevoPin)
        {
            string salt = GenerateSalt();
            string hash = ComputeHash(nuevoPin, salt);

            var par = new SqlParameter[]
            {
                new SqlParameter("@EmpresaId", SqlDbType.Int) { Value = Global.Datos.TenantContext.EmpresaId },
                new SqlParameter("@sLegajoID", SqlDbType.NVarChar) { Value = legajoId },
                new SqlParameter("@PinHash", SqlDbType.NVarChar) { Value = hash },
                new SqlParameter("@PinSalt", SqlDbType.NVarChar) { Value = salt }
            };

            try
            {
                Global.Datos.SQLServer.EjecutarSPInsertUpdate("EscritorioLegajoPIN_Cambiar", par);
                PinMustChange = false;
                PinExpired = false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Carga datos del legajo sin verificar PIN (para saber si tiene PIN asignado).
        /// </summary>
        public bool CargarLegajo(string legajoId)
        {
            var par = new SqlParameter[]
            {
                new SqlParameter("@EmpresaId", SqlDbType.Int) { Value = Global.Datos.TenantContext.EmpresaId },
                new SqlParameter("@sLegajoID", SqlDbType.NVarChar) { Value = legajoId }
            };

            DataTable dt = Global.Datos.SQLServer.EjecutarSPSelect("EscritorioLegajoPIN_Verificar", par);

            if (dt.Rows.Count == 0)
                return false;

            var row = dt.Rows[0];
            nLegajoID = Convert.ToInt32(row["nLegajoID"]);
            sLegajoID = row["sLegajoID"].ToString();
            sLegajoNombre = row["sLegajoNombre"].ToString();
            HasPin = row["PinHash"] != DBNull.Value && !string.IsNullOrEmpty(row["PinHash"].ToString());
            PinMustChange = row["PinMustChange"] != DBNull.Value && Convert.ToBoolean(row["PinMustChange"]);

            return true;
        }

        /// <summary>
        /// Retorna lista de legajos activos (para modo demo).
        /// </summary>
        public static DataTable ListaLegajosActivos()
        {
            var par = new SqlParameter[]
            {
                new SqlParameter("@EmpresaId", SqlDbType.Int) { Value = Global.Datos.TenantContext.EmpresaId }
            };
            return Global.Datos.SQLServer.EjecutarSPSelect("EscritorioLegajosActivos_Lista", par);
        }

        /// <summary>
        /// Obtiene la configuracion de dias de expiracion de PIN.
        /// </summary>
        public static int ObtenerDiasExpiracion()
        {
            string valor = Generales.GRALVariablesGlobales.TraerValorDataBase("PinExpiraDias");
            if (int.TryParse(valor, out int dias))
                return dias;
            return 90;
        }

        /// <summary>
        /// Verifica si el modo PIN esta habilitado.
        /// </summary>
        public static bool ModoPinHabilitado()
        {
            string valor = Generales.GRALVariablesGlobales.TraerValorDataBase("FichadaModoPIN");
            return string.Equals(valor, "true", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Verifica si el modo demo esta habilitado.
        /// </summary>
        public static bool ModoDemoHabilitado()
        {
            string valor = Generales.GRALVariablesGlobales.TraerValorDataBase("FichadaModoDemo");
            return string.Equals(valor, "true", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Busca un legajo activo por su QrToken (GUID).
        /// Retorna true si lo encuentra, y carga nLegajoID/sLegajoID/sLegajoNombre.
        /// </summary>
        public bool BuscarPorQrToken(string qrToken)
        {
            if (string.IsNullOrEmpty(qrToken))
                return false;

            // Validar que sea un GUID valido para prevenir inyeccion SQL
            Guid parsedGuid;
            if (!Guid.TryParse(qrToken, out parsedGuid))
                return false;

            int empresaId = Global.Datos.TenantContext.EmpresaId;
            // Comparar sin guiones (N format) porque la BD puede tener con o sin guiones
            string tokenSinGuiones = parsedGuid.ToString("N");
            string sql = "SELECT l.Id, CAST(l.NumeroLegajo AS NVARCHAR) AS sLegajoID, " +
                         "l.Apellido + ', ' + l.Nombre AS sLegajoNombre " +
                         "FROM Legajo l " +
                         "WHERE REPLACE(l.QrToken, '-', '') = '" + tokenSinGuiones + "' " +
                         "AND l.EmpresaId = " + empresaId + " AND l.IsActive = 1";

            DataTable dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(sql);
            if (dt.Rows.Count == 0)
                return false;

            var row = dt.Rows[0];
            nLegajoID = Convert.ToInt32(row["Id"]);
            sLegajoID = row["sLegajoID"].ToString();
            sLegajoNombre = row["sLegajoNombre"].ToString();
            return true;
        }

        private static string ComputeHash(string pin, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(salt + pin);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private static string GenerateSalt()
        {
            byte[] saltBytes = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
    }
}
