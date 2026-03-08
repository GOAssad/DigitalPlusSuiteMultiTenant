using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace DigitalPlus.Licensing
{
    public static class ClockGuard
    {
        private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("DigitalPlus.ClockGuard.v1");

        public static bool IsClockTampered(DateTime lastServerTimeUtc)
        {
            var stored = LoadLastKnownTime();
            if (stored == null) return false;

            // If local UTC is more than 1 hour behind the last server time, suspect tampering
            if (DateTime.UtcNow < stored.Value.AddHours(-1))
                return true;

            return false;
        }

        public static void UpdateServerTime(DateTime serverTimeUtc)
        {
            try
            {
                var bytes = BitConverter.GetBytes(serverTimeUtc.ToBinary());
                var encrypted = ProtectedData.Protect(bytes, Entropy, DataProtectionScope.LocalMachine);
                File.WriteAllBytes(GetPath(), encrypted);
            }
            catch { }
        }

        private static DateTime? LoadLastKnownTime()
        {
            try
            {
                var path = GetPath();
                if (!File.Exists(path)) return null;

                var encrypted = File.ReadAllBytes(path);
                var decrypted = ProtectedData.Unprotect(encrypted, Entropy, DataProtectionScope.LocalMachine);
                return DateTime.FromBinary(BitConverter.ToInt64(decrypted, 0));
            }
            catch { return null; }
        }

        private static string GetPath()
        {
            var dir = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location
                ?? Assembly.GetExecutingAssembly().Location);
            return Path.Combine(dir, "clock.dat");
        }
    }
}
