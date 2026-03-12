using System;
using System.IO;

namespace Acceso.uAreu
{
    // LOG TEMPORAL - borrar despues del diagnostico
    static class HuellaLog
    {
        private static readonly string LogFile = Path.Combine(
            Path.GetTempPath(), "huella_debug.log");

        public static void Write(string msg)
        {
            try
            {
                File.AppendAllText(LogFile,
                    DateTime.Now.ToString("HH:mm:ss.fff") + " | " + msg + Environment.NewLine);
            }
            catch { }
        }
    }
}
