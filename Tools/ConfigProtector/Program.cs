using System;
using System.Configuration;
using System.IO;

namespace ConfigProtector
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("DigitalPlus ConfigProtector — Cifra/descifra connectionStrings con DPAPI");
                Console.WriteLine();
                Console.WriteLine("Uso:");
                Console.WriteLine("  ConfigProtector.exe <ruta-al-.config>              Encriptar");
                Console.WriteLine("  ConfigProtector.exe <ruta-al-.config> --decrypt     Desencriptar (soporte)");
                return 1;
            }

            string configPath = args[0];
            bool decrypt = args.Length > 1 &&
                           args[1].Equals("--decrypt", StringComparison.OrdinalIgnoreCase);

            if (!File.Exists(configPath))
            {
                Console.WriteLine("ERROR: No se encontro el archivo: " + configPath);
                return 2;
            }

            try
            {
                // Abrir el .config usando ExeConfigurationFileMap
                var map = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = configPath
                };
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(
                    map, ConfigurationUserLevel.None);

                ConfigurationSection section = config.GetSection("connectionStrings");
                if (section == null)
                {
                    Console.WriteLine("ERROR: No se encontro la seccion connectionStrings en " + configPath);
                    return 3;
                }

                if (decrypt)
                {
                    if (!section.SectionInformation.IsProtected)
                    {
                        Console.WriteLine("Already unprotected: " + configPath);
                        return 0;
                    }

                    section.SectionInformation.UnprotectSection();
                    config.Save(ConfigurationSaveMode.Modified);
                    Console.WriteLine("Decrypted: " + configPath);
                    return 0;
                }
                else
                {
                    if (section.SectionInformation.IsProtected)
                    {
                        Console.WriteLine("Already protected: " + configPath);
                        return 0;
                    }

                    section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                    section.SectionInformation.ForceSave = true;
                    config.Save(ConfigurationSaveMode.Full);
                    Console.WriteLine("Protected: " + configPath);
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("       " + ex.InnerException.Message);
                return 4;
            }
        }
    }
}
