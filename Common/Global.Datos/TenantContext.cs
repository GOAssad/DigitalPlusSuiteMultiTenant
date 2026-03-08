using System.Configuration;

namespace Global.Datos
{
    /// <summary>
    /// Contexto de tenant para apps desktop (Fichador / Administrador).
    /// Lee EmpresaId de appSettings en el .exe.config.
    /// En desarrollo: se setea manualmente (ej. EmpresaId=2 para Kosiuko).
    /// En producción: el instalador escribe el valor durante la activación.
    /// </summary>
    public static class TenantContext
    {
        private static int? _empresaId;

        /// <summary>
        /// EmpresaId del tenant actual.
        /// Lee de appSettings["EmpresaId"], fallback a 1 (SuperAdmin).
        /// </summary>
        public static int EmpresaId
        {
            get
            {
                if (!_empresaId.HasValue)
                {
                    string val = ConfigurationManager.AppSettings["EmpresaId"];
                    _empresaId = int.TryParse(val, out int id) && id > 0 ? id : 1;
                }
                return _empresaId.Value;
            }
            set
            {
                _empresaId = value;
            }
        }

        /// <summary>
        /// Fuerza re-lectura del EmpresaId desde config (útil para tests).
        /// </summary>
        public static void Reset()
        {
            _empresaId = null;
        }
    }
}
