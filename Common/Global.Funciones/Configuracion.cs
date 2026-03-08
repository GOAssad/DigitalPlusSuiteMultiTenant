using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Global.Funciones
{
    /// <summary>
    /// Lectura de Archivo de configuracion
    /// </summary>
    public static class Configuracion
    {
        /// <summary>
        /// Buscar el valor de archivo config
        /// </summary>
        /// <param name="pLlave">Clave para buscar el valor</param>
        /// <param name="pPredeterminado">Mensaje que muestra si no se encuentra la llave</param>
        /// <returns></returns>
        public static string RecuperaValor(string pLlave, string pPredeterminado)
        {
            string retorno = ConfigurationManager.AppSettings[pLlave];
            if (retorno == null) { retorno = pPredeterminado; }
            if (retorno == string.Empty) { retorno = pPredeterminado; }
            return retorno;
        }
        /// <summary>
        /// Establecer Valor de las variables en el archivo config
        /// </summary>
        /// <param name="pLlave">Clave para buscar el valor</param>
        /// <param name="pValor">Valor de la llave que se esta buscando</param>
        public static void EstablecerValor(string pLlave, string pValor)
        {
            //Creo el objeto de la configuracion
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //Borro la configuracion actual
            config.AppSettings.Settings.Remove(pLlave);
            //Guardo los cambios
            config.Save(ConfigurationSaveMode.Modified);
            //Forzar recarga
            ConfigurationManager.RefreshSection("appSettings");
            //Guardar la configuracion
            config.AppSettings.Settings.Add(pLlave, pValor);
            //Guardar los cambios
            config.Save(ConfigurationSaveMode.Modified);
            //Forzar Recarga
            ConfigurationManager.RefreshSection("appSettings");
        }

    }

}
