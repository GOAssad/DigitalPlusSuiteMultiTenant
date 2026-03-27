using System;
using System.Configuration;

namespace DigitalPlus.Licensing
{
    public class LicenseManager
    {
        private readonly string _apiKey;
        private readonly string _appName;
        private LicenseTicket _currentTicket;
        private string _currentTicketJson;
        private string _currentSignature;

        public LicenseManager(string apiKey, string appName = "Fichador")
        {
            _apiKey = apiKey ?? string.Empty;
            _appName = appName;
        }

        /// <summary>
        /// Lee EmpresaId (AdminEmpresaId) de app.config. Retorna 0 si no existe (clientes viejos).
        /// </summary>
        private static int GetEmpresaIdFromConfig()
        {
            var val = ConfigurationManager.AppSettings["AdminEmpresaId"];
            int id;
            if (!string.IsNullOrEmpty(val) && int.TryParse(val, out id))
                return id;
            return 0;
        }

        public LicenseTicket CurrentTicket => _currentTicket;

        /// <summary>
        /// Retorna texto descriptivo de la licencia activa para mostrar en UI.
        /// </summary>
        public string GetLicenseInfoText()
        {
            if (_currentTicket == null)
                return "Sin licencia";

            var t = _currentTicket;
            var tipo = t.LicenseType == "trial" ? "Prueba" :
                       t.LicenseType == "active" ? "Activa" :
                       t.LicenseType == "suspended" ? "Suspendida" : t.LicenseType;
            var plan = string.IsNullOrEmpty(t.Plan) ? "" : t.Plan.Substring(0, 1).ToUpper() + t.Plan.Substring(1);
            var empresa = t.CompanyId ?? "";
            if (empresa.StartsWith("DP_")) empresa = empresa.Substring(3).Replace("_", " ");

            string vencimiento;
            if (t.LicenseType == "trial" && t.TrialEndsAt.HasValue)
            {
                var dias = (int)(t.TrialEndsAt.Value - DateTime.UtcNow).TotalDays;
                if (dias < 0) dias = 0;
                vencimiento = t.TrialEndsAt.Value.ToLocalTime().ToString("dd/MM/yyyy") +
                    " (" + dias + " dia" + (dias != 1 ? "s" : "") + ")";
            }
            else if (t.ExpiresAt.HasValue)
            {
                var dias = (int)(t.ExpiresAt.Value - DateTime.UtcNow).TotalDays;
                if (dias < 0) dias = 0;
                vencimiento = t.ExpiresAt.Value.ToLocalTime().ToString("dd/MM/yyyy") +
                    " (" + dias + " dia" + (dias != 1 ? "s" : "") + ")";
            }
            else
            {
                vencimiento = "Sin vencimiento";
            }

            return string.Format("Empresa: {0}\nLicencia: {1} - Plan {2}\nMax legajos: {3}\nVence: {4}",
                empresa, tipo, plan, t.MaxLegajos, vencimiento);
        }

        /// <summary>
        /// Retorna texto corto para barra de estado.
        /// </summary>
        public string GetStatusBarText()
        {
            if (_currentTicket == null)
                return "Sin licencia";

            var t = _currentTicket;
            var empresa = t.CompanyId ?? "";
            if (empresa.StartsWith("DP_")) empresa = empresa.Substring(3).Replace("_", " ");

            DateTime? vence = t.LicenseType == "trial" ? t.TrialEndsAt : t.ExpiresAt;
            if (vence.HasValue)
            {
                var dias = (int)(vence.Value - DateTime.UtcNow).TotalDays;
                if (dias < 0) dias = 0;
                return string.Format("{0} | {1} | Vence: {2} ({3}d)",
                    empresa, t.Plan, vence.Value.ToLocalTime().ToString("dd/MM/yyyy"), dias);
            }

            return string.Format("{0} | {1}", empresa, t.Plan);
        }

        /// <summary>
        /// Retorna dias restantes de licencia (negativo si vencida, null si sin vencimiento).
        /// </summary>
        public int? GetDaysRemaining()
        {
            if (_currentTicket == null) return null;
            DateTime? vence = _currentTicket.LicenseType == "trial"
                ? _currentTicket.TrialEndsAt : _currentTicket.ExpiresAt;
            if (!vence.HasValue) return null;
            return (int)(vence.Value - DateTime.UtcNow).TotalDays;
        }

        /// <summary>
        /// Llamar al inicio de la app. Carga cache, verifica firma, intenta heartbeat.
        /// </summary>
        public LicenseValidationResult ValidateAtStartup(int currentLegajos = 0)
        {
            // 1. Cargar desde cache
            if (LicenseCache.TryLoad(out var ticketJson, out var sig))
            {
                // 2. Verificar firma
                if (!LicenseValidator.VerifySignature(ticketJson, sig))
                {
                    return new LicenseValidationResult
                    {
                        State = LicenseState.InvalidSignature,
                        UserMessage = "La licencia es invalida o fue modificada.\nContacte soporte."
                    };
                }

                _currentTicket = JsonHelper.Deserialize<LicenseTicket>(ticketJson);
                _currentTicketJson = ticketJson;
                _currentSignature = sig;

                // 3. Clock tampering
                if (ClockGuard.IsClockTampered(_currentTicket.ServerTimeUtc))
                {
                    return new LicenseValidationResult
                    {
                        State = LicenseState.ClockTampered,
                        UserMessage = "Se detecto un cambio en el reloj del sistema.\nConecte a internet y reinicie la aplicacion."
                    };
                }

                // 4. Intentar heartbeat para renovar
                TryHeartbeat(currentLegajos);

                // 5. Evaluar reglas
                return LicenseValidator.Evaluate(_currentTicket, DateTime.UtcNow, currentLegajos);
            }

            // No hay cache: intentar trial activation
            return AttemptTrialActivation();
        }

        /// <summary>
        /// Llamar periodicamente (ej: cada 4 horas).
        /// </summary>
        public LicenseValidationResult PeriodicCheck(int currentLegajos = 0)
        {
            TryHeartbeat(currentLegajos);

            if (_currentTicket == null)
                return new LicenseValidationResult
                {
                    State = LicenseState.NoLicense,
                    UserMessage = "No se encontro licencia."
                };

            return LicenseValidator.Evaluate(_currentTicket, DateTime.UtcNow, currentLegajos);
        }

        /// <summary>
        /// Activar con un codigo de activacion proporcionado por el usuario.
        /// </summary>
        public LicenseValidationResult ActivateWithCode(string activationCode, int currentLegajos = 0)
        {
            var companyName = ConfigurationManager.AppSettings["NombreEmpresa"] ?? "Unknown";
            var machineId = MachineIdProvider.GetMachineId();
            var empresaId = GetEmpresaIdFromConfig();

            var client = new LicenseClient(_apiKey);
            if (client.TryActivate(activationCode, companyName, machineId, "local", empresaId,
                out var ticketJson, out var sig, out var error))
            {
                if (LicenseValidator.VerifySignature(ticketJson, sig))
                {
                    LicenseCache.Save(ticketJson, sig);
                    _currentTicket = JsonHelper.Deserialize<LicenseTicket>(ticketJson);
                    _currentTicketJson = ticketJson;
                    _currentSignature = sig;
                    ClockGuard.UpdateServerTime(_currentTicket.ServerTimeUtc);

                    return LicenseValidator.Evaluate(_currentTicket, DateTime.UtcNow, currentLegajos);
                }

                return new LicenseValidationResult
                {
                    State = LicenseState.InvalidSignature,
                    UserMessage = "La respuesta del servidor es invalida."
                };
            }

            return new LicenseValidationResult
            {
                State = LicenseState.NoLicense,
                UserMessage = "No se pudo activar la licencia:\n" + (error ?? "Error desconocido")
            };
        }

        private LicenseValidationResult AttemptTrialActivation()
        {
            var companyName = ConfigurationManager.AppSettings["NombreEmpresa"] ?? "Unknown";
            var machineId = MachineIdProvider.GetMachineId();
            var empresaId = GetEmpresaIdFromConfig();

            var client = new LicenseClient(_apiKey);
            if (client.TryActivate(null, companyName, machineId, "local", empresaId,
                out var ticketJson, out var sig, out var error))
            {
                if (LicenseValidator.VerifySignature(ticketJson, sig))
                {
                    LicenseCache.Save(ticketJson, sig);
                    _currentTicket = JsonHelper.Deserialize<LicenseTicket>(ticketJson);
                    _currentTicketJson = ticketJson;
                    _currentSignature = sig;
                    ClockGuard.UpdateServerTime(_currentTicket.ServerTimeUtc);

                    return LicenseValidator.Evaluate(_currentTicket, DateTime.UtcNow);
                }
            }

            // Offline y sin cache → no license
            return new LicenseValidationResult
            {
                State = LicenseState.NoLicense,
                UserMessage = "No se pudo conectar al servidor de licencias.\nVerifique su conexion a internet."
            };
        }

        private void TryHeartbeat(int currentLegajos = 0)
        {
            if (_currentTicket == null) return;

            try
            {
                var empresaId = _currentTicket.EmpresaId > 0
                    ? _currentTicket.EmpresaId
                    : GetEmpresaIdFromConfig();

                var client = new LicenseClient(_apiKey);
                if (client.TryHeartbeat(
                    _currentTicket.CompanyId,
                    _currentTicket.MachineId,
                    _appName,
                    currentLegajos,
                    empresaId,
                    out var ticketJson,
                    out var sig,
                    out _))
                {
                    if (LicenseValidator.VerifySignature(ticketJson, sig))
                    {
                        LicenseCache.Save(ticketJson, sig);
                        _currentTicket = JsonHelper.Deserialize<LicenseTicket>(ticketJson);
                        _currentTicketJson = ticketJson;
                        _currentSignature = sig;
                        ClockGuard.UpdateServerTime(_currentTicket.ServerTimeUtc);
                    }
                }
            }
            catch
            {
                // Offline: continue with cached ticket
            }
        }
    }
}
