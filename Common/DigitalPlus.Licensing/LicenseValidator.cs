using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DigitalPlus.Licensing
{
    public static class LicenseValidator
    {
        // Certificado publico X509 (DER base64) generado por generate-license-keys.ps1
        private const string PublicCertBase64 =
            "MIIDJjCCAg6gAwIBAgIQWQ9PmFIV7qJHVDTh/0EU4DANBgkqhkiG9w0BAQsFADAm" +
            "MSQwIgYDVQQDDBtEaWdpdGFsUGx1cyBMaWNlbnNlIFNpZ25pbmcwHhcNMjYwMzA0" +
            "MjM1OTQ3WhcNNDYwMzA1MDAwOTQ3WjAmMSQwIgYDVQQDDBtEaWdpdGFsUGx1cyBM" +
            "aWNlbnNlIFNpZ25pbmcwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDm" +
            "6JEKoh2Grc48JNi0HOnPkGXkIMlZiDtHlpca2xMjCkgc8yio9FP8oKyoXkL8ZSDD" +
            "GMQ7yzQJ/x/9hZecZC+Hec4iZg68va5cIMm8ZHB+lVjke7vg1c8Xm7uBLEABmXS" +
            "5usLEKvirNXjgmeljQvk19Nl+qd3Cnn+XzW6/14k+88FnqaPQ6jyjof3i0glP6rG" +
            "wICWi/FCiYmtWab8UyXTk0e4qhgaa8gOBN72FKIWZ3Lx2fYiWOCIDncnv/Id+Ge" +
            "OQp4lBnZpKeCvBdGNVJPPOFfo8lchJLXTPhRKPMx/Re5Xu3rsnwOuLZt0ACB6zTK" +
            "gDaUAfuU4O4JjekcdJEF8xAgMBAAGjUDBOMA4GA1UdDwEB/wQEAwIHgDAdBgNVHSUE" +
            "FjAUBggrBgEFBQcDAgYIKwYBBQUHAwEwHQYDVR0OBBYEFEUwb23DnowgDRTEKjoD" +
            "+R9UO1pOMA0GCSqGSIb3DQEBCwUAA4IBAQCsNumOja0HlF3FGO0IjcXdI99rNRtg" +
            "4lrO4lDPu74B4Q0cHO4ProOB3r3ZUzk1nyssH7xEpwN9/hpuyT1YuDlEXHrtknuv" +
            "1GAKEd/Vh+aTHu7vj6zZomVPsQ+DQjrK5WDRh88dRLDut8YXA/aa+x2GzJQg2rx" +
            "hfrXg/AdgGzZzc8nn4UrIzMXk/6W4zhXD2gEmV8RbmujUQb8JwtvSYMhoHK7hyAG" +
            "c4rD9fYBMpnR5lLCMccSl1aCb8xg5hiZttDhB5IhgMAomdxCqnxDjo1ykGkG2M5" +
            "fH5Utng4xglB0gLahLkSqw/yusYhOoh80wEbL0O/kDIOe+Ekwf2wkIwpkQ";

        public static bool VerifySignature(string ticketJson, string signatureBase64)
        {
            try
            {
                var certBytes = Convert.FromBase64String(PublicCertBase64);
                var cert = new X509Certificate2(certBytes);
                var rsa = (RSACryptoServiceProvider)cert.PublicKey.Key;

                var data = Encoding.UTF8.GetBytes(ticketJson);
                var sig = Convert.FromBase64String(signatureBase64);

                return rsa.VerifyData(data, CryptoConfig.MapNameToOID("SHA256"), sig);
            }
            catch
            {
                return false;
            }
        }

        public static LicenseValidationResult Evaluate(LicenseTicket ticket, DateTime utcNow, int currentLegajos = 0)
        {
            if (ticket == null)
                return new LicenseValidationResult
                {
                    State = LicenseState.NoLicense,
                    UserMessage = "No se encontro licencia. Contacte soporte."
                };

            // Rule 1: Trial
            if (ticket.LicenseType == "trial")
            {
                if (ticket.TrialEndsAt.HasValue && utcNow > ticket.TrialEndsAt.Value)
                    return new LicenseValidationResult
                    {
                        State = LicenseState.TrialExpired,
                        UserMessage = "El periodo de prueba ha finalizado.\nActive su licencia para continuar usando el sistema.",
                        MaxLegajos = ticket.MaxLegajos
                    };

                if (currentLegajos > ticket.MaxLegajos)
                    return new LicenseValidationResult
                    {
                        State = LicenseState.TrialLegajosExceeded,
                        UserMessage = string.Format(
                            "La version de prueba permite hasta {0} legajos.\nTiene {1} legajos activos.\nActive su licencia para continuar.",
                            ticket.MaxLegajos, currentLegajos),
                        MaxLegajos = ticket.MaxLegajos
                    };

                var daysLeft = ticket.TrialEndsAt.HasValue
                    ? (int)(ticket.TrialEndsAt.Value - utcNow).TotalDays
                    : 0;
                if (daysLeft < 0) daysLeft = 0;

                return new LicenseValidationResult
                {
                    State = LicenseState.TrialActive,
                    TrialDaysRemaining = daysLeft,
                    MaxLegajos = ticket.MaxLegajos,
                    UserMessage = string.Format("Periodo de prueba: {0} dia(s) restante(s).", daysLeft)
                };
            }

            // Rule 2: Suspended
            if (ticket.LicenseType == "suspended")
            {
                if (ticket.GraceEndsAt.HasValue && utcNow > ticket.GraceEndsAt.Value)
                    return new LicenseValidationResult
                    {
                        State = LicenseState.SuspendedBlocked,
                        UserMessage = "Licencia suspendida por falta de pago.\nEl periodo de gracia ha terminado.\nContacte soporte para regularizar.",
                        MaxLegajos = ticket.MaxLegajos
                    };

                return new LicenseValidationResult
                {
                    State = LicenseState.Suspended,
                    UserMessage = "Licencia suspendida por falta de pago.\nRegularice su situacion para evitar el bloqueo.",
                    MaxLegajos = ticket.MaxLegajos
                };
            }

            // Rule 3: Active license expiry
            if (ticket.ExpiresAt.HasValue && utcNow > ticket.ExpiresAt.Value)
                return new LicenseValidationResult
                {
                    State = LicenseState.Expired,
                    UserMessage = "Su licencia ha expirado.\nRenueve para continuar usando el sistema.",
                    MaxLegajos = ticket.MaxLegajos
                };

            // Rule 4: Offline check (NextCheckRequiredAt)
            if (utcNow > ticket.NextCheckRequiredAt && ticket.NextCheckRequiredAt > DateTime.MinValue)
                return new LicenseValidationResult
                {
                    State = LicenseState.OfflineBlocked,
                    UserMessage = "No se pudo verificar la licencia en linea.\nConecte el equipo a internet y reinicie la aplicacion.",
                    MaxLegajos = ticket.MaxLegajos
                };

            // All OK
            return new LicenseValidationResult
            {
                State = LicenseState.Valid,
                UserMessage = string.Empty,
                MaxLegajos = ticket.MaxLegajos
            };
        }
    }
}
