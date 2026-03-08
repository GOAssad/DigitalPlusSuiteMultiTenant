using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace DigitalPlus.Licensing
{
    [DataContract]
    internal class CacheWrapper
    {
        [DataMember(Name = "ticket")]
        public string Ticket { get; set; }

        [DataMember(Name = "signature")]
        public string Signature { get; set; }
    }

    public static class LicenseCache
    {
        private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("DigitalPlus.Licensing.v1");

        public static string GetCachePath()
        {
            var dir = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location
                ?? Assembly.GetExecutingAssembly().Location);
            return Path.Combine(dir, "license.dat");
        }

        public static bool TryLoad(out string ticketJson, out string signature)
        {
            ticketJson = null;
            signature = null;

            try
            {
                var path = GetCachePath();
                if (!File.Exists(path)) return false;

                var encrypted = File.ReadAllBytes(path);
                var decrypted = ProtectedData.Unprotect(encrypted, Entropy, DataProtectionScope.LocalMachine);
                var json = Encoding.UTF8.GetString(decrypted);

                var wrapper = JsonHelper.Deserialize<CacheWrapper>(json);
                if (wrapper == null || string.IsNullOrEmpty(wrapper.Ticket)) return false;

                ticketJson = wrapper.Ticket;
                signature = wrapper.Signature;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void Save(string ticketJson, string signature)
        {
            try
            {
                var wrapper = new CacheWrapper { Ticket = ticketJson, Signature = signature };
                var json = JsonHelper.Serialize(wrapper);
                var plainBytes = Encoding.UTF8.GetBytes(json);
                var encrypted = ProtectedData.Protect(plainBytes, Entropy, DataProtectionScope.LocalMachine);
                File.WriteAllBytes(GetCachePath(), encrypted);
            }
            catch
            {
                // Non-fatal: app can still work online
            }
        }

        public static void Delete()
        {
            try
            {
                var path = GetCachePath();
                if (File.Exists(path)) File.Delete(path);
            }
            catch { }
        }
    }
}
