using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace Acceso.Clases.Datos.Generales
{
    /// <summary>
    /// Informacion de empresa obtenida de DigitalPlusAdmin.
    /// </summary>
    public class EmpresaInfo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string RazonSocial { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public byte[] Logo { get; set; }
        public string LogoContentType { get; set; }
        public string Estado { get; set; }
        public string CodigoActivacion { get; set; }
        public string IdentificacionFiscal { get; set; }

        // Identidad / Redes sociales
        public string PaginaWeb { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string LinkedIn { get; set; }
        public string Twitter { get; set; }
        public string YouTube { get; set; }
        public string TikTok { get; set; }
    }

    /// <summary>
    /// Servicio para consultar informacion de empresa desde la BD DigitalPlusAdmin.
    /// Requiere connection string "Admin" y appSetting "AdminEmpresaId" en el .exe.config.
    /// Los resultados se cachean durante toda la sesion de la app.
    /// </summary>
    public static class EmpresaInfoService
    {
        private static EmpresaInfo _cache;
        private static bool _consultado;
        private static byte[] _integraIALogo;

        /// <summary>
        /// Obtiene la info de empresa desde DigitalPlusAdmin.
        /// Cachea el resultado; devuelve null si no se puede conectar o no existe.
        /// </summary>
        public static EmpresaInfo ObtenerEmpresa()
        {
            if (_consultado) return _cache;
            _consultado = true;

            string adminEmpresaId = ConfigurationManager.AppSettings["AdminEmpresaId"];
            if (string.IsNullOrEmpty(adminEmpresaId)) return null;

            int empresaId;
            if (!int.TryParse(adminEmpresaId, out empresaId) || empresaId <= 0) return null;

            var cs = ConfigurationManager.ConnectionStrings["Admin"];
            if (cs == null || string.IsNullOrEmpty(cs.ConnectionString)) return null;

            try
            {
                using (var conn = new SqlConnection(cs.ConnectionString))
                {
                    conn.Open();
                    string sql = @"SELECT Id, Nombre, RazonSocial, Email, Telefono, Direccion,
                                          Logo, LogoContentType, Estado, CodigoActivacion, IdentificacionFiscal,
                                          PaginaWeb, Facebook, Instagram, LinkedIn, Twitter, YouTube, TikTok
                                   FROM Empresas WHERE Id = @Id";
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = empresaId;
                        cmd.CommandTimeout = 10;
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                _cache = new EmpresaInfo
                                {
                                    Id = reader.GetInt32(0),
                                    Nombre = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                    RazonSocial = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                    Email = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                    Telefono = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                    Direccion = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                    Logo = reader.IsDBNull(6) ? null : (byte[])reader[6],
                                    LogoContentType = reader.IsDBNull(7) ? "" : reader.GetString(7),
                                    Estado = reader.IsDBNull(8) ? "" : reader.GetString(8),
                                    CodigoActivacion = reader.IsDBNull(9) ? "" : reader.GetString(9),
                                    IdentificacionFiscal = reader.IsDBNull(10) ? "" : reader.GetString(10),
                                    PaginaWeb = reader.IsDBNull(11) ? "" : reader.GetString(11),
                                    Facebook = reader.IsDBNull(12) ? "" : reader.GetString(12),
                                    Instagram = reader.IsDBNull(13) ? "" : reader.GetString(13),
                                    LinkedIn = reader.IsDBNull(14) ? "" : reader.GetString(14),
                                    Twitter = reader.IsDBNull(15) ? "" : reader.GetString(15),
                                    YouTube = reader.IsDBNull(16) ? "" : reader.GetString(16),
                                    TikTok = reader.IsDBNull(17) ? "" : reader.GetString(17)
                                };
                            }
                        }
                    }
                }
            }
            catch
            {
                // Logo es no-critico; si falla la conexion, las apps siguen funcionando
            }

            return _cache;
        }

        /// <summary>
        /// Obtiene el logo de IntegraIA embebido como recurso en el assembly.
        /// </summary>
        public static byte[] ObtenerLogoIntegraIA()
        {
            if (_integraIALogo != null) return _integraIALogo;

            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream("Acceso.Clases.Datos.Resources.Logo-IntegraIA.png"))
                {
                    if (stream != null)
                    {
                        _integraIALogo = new byte[stream.Length];
                        stream.Read(_integraIALogo, 0, _integraIALogo.Length);
                    }
                }
            }
            catch { }

            return _integraIALogo;
        }

        /// <summary>
        /// Fuerza re-consulta en el proximo llamado (util si se actualizo el logo desde el portal).
        /// </summary>
        public static void Reset()
        {
            _cache = null;
            _consultado = false;
        }
    }
}
