using System;
using System.Runtime.Serialization;

namespace DigitalPlus.Licensing
{
    [DataContract]
    public class LicenseTicket
    {
        [DataMember(Name = "v")]
        public int Version { get; set; }

        [DataMember(Name = "companyId")]
        public string CompanyId { get; set; }

        [DataMember(Name = "machineId")]
        public string MachineId { get; set; }

        [DataMember(Name = "licenseType")]
        public string LicenseType { get; set; }

        [DataMember(Name = "plan")]
        public string Plan { get; set; }

        [DataMember(Name = "maxLegajos")]
        public int MaxLegajos { get; set; }

        [DataMember(Name = "trialEndsAt")]
        public string TrialEndsAtStr { get; set; }

        [DataMember(Name = "expiresAt")]
        public string ExpiresAtStr { get; set; }

        [DataMember(Name = "suspendedAt")]
        public string SuspendedAtStr { get; set; }

        [DataMember(Name = "graceEndsAt")]
        public string GraceEndsAtStr { get; set; }

        [DataMember(Name = "issuedAt")]
        public string IssuedAtStr { get; set; }

        [DataMember(Name = "nextCheckRequiredAt")]
        public string NextCheckRequiredAtStr { get; set; }

        [DataMember(Name = "serverTimeUtc")]
        public string ServerTimeUtcStr { get; set; }

        // Parsed DateTime helpers
        public DateTime? TrialEndsAt => ParseDate(TrialEndsAtStr);
        public DateTime? ExpiresAt => ParseDate(ExpiresAtStr);
        public DateTime? SuspendedAt => ParseDate(SuspendedAtStr);
        public DateTime? GraceEndsAt => ParseDate(GraceEndsAtStr);
        public DateTime IssuedAt => ParseDate(IssuedAtStr) ?? DateTime.MinValue;
        public DateTime NextCheckRequiredAt => ParseDate(NextCheckRequiredAtStr) ?? DateTime.MinValue;
        public DateTime ServerTimeUtc => ParseDate(ServerTimeUtcStr) ?? DateTime.MinValue;

        private static DateTime? ParseDate(string s)
        {
            if (string.IsNullOrEmpty(s)) return null;
            DateTime dt;
            if (DateTime.TryParse(s, null, System.Globalization.DateTimeStyles.RoundtripKind, out dt))
                return dt.ToUniversalTime();
            return null;
        }
    }
}
