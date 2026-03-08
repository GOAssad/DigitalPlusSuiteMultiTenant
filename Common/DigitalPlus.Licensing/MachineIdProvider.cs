using System;
using System.IO;
using System.Management;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

namespace DigitalPlus.Licensing
{
    public static class MachineIdProvider
    {
        public static string GetMachineId()
        {
            var persistPath = GetPersistPath();

            // Try loading persisted ID first
            try
            {
                if (File.Exists(persistPath))
                {
                    var cached = File.ReadAllText(persistPath).Trim();
                    if (!string.IsNullOrEmpty(cached) && cached.Length >= 16)
                        return cached;
                }
            }
            catch { }

            // Generate from hardware
            var raw = GetMachineGuid() + "|" + GetBiosSerial() + "|" + GetProcessorId();

            string id;
            using (var sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
                id = BitConverter.ToString(hash).Replace("-", "").Substring(0, 32);
            }

            // Persist
            try { File.WriteAllText(persistPath, id); } catch { }

            return id;
        }

        private static string GetPersistPath()
        {
            var dir = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location
                ?? Assembly.GetExecutingAssembly().Location);
            return Path.Combine(dir, "machine.id");
        }

        private static string GetMachineGuid()
        {
            try
            {
                using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography"))
                {
                    return key?.GetValue("MachineGuid")?.ToString() ?? "unknown";
                }
            }
            catch { return "unknown"; }
        }

        private static string GetBiosSerial()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BIOS"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                        return obj["SerialNumber"]?.ToString() ?? "";
                }
            }
            catch { }
            return "unknown";
        }

        private static string GetProcessorId()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                        return obj["ProcessorId"]?.ToString() ?? "";
                }
            }
            catch { }
            return "unknown";
        }
    }
}
