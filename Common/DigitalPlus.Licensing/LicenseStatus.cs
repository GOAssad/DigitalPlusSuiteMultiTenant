namespace DigitalPlus.Licensing
{
    public enum LicenseState
    {
        Valid,
        TrialActive,
        TrialExpired,
        TrialLegajosExceeded,
        Expired,
        Suspended,
        SuspendedBlocked,
        OfflineGrace,
        OfflineBlocked,
        InvalidSignature,
        ClockTampered,
        NoLicense
    }

    public class LicenseValidationResult
    {
        public LicenseState State { get; set; }
        public string UserMessage { get; set; }
        public int? TrialDaysRemaining { get; set; }
        public int MaxLegajos { get; set; }

        public bool IsBlocked
        {
            get
            {
                return State == LicenseState.TrialExpired
                    || State == LicenseState.TrialLegajosExceeded
                    || State == LicenseState.Expired
                    || State == LicenseState.SuspendedBlocked
                    || State == LicenseState.OfflineBlocked
                    || State == LicenseState.InvalidSignature
                    || State == LicenseState.ClockTampered
                    || State == LicenseState.NoLicense;
            }
        }
    }
}
