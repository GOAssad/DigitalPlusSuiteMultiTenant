namespace DigitalPlus.Helpers
{
    static public class FuncionesDeCadena
    {
		public static string EncriptarString(this string _cadenaAencriptar)
		{
			string result = string.Empty;
			byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
			result = Convert.ToBase64String(encryted);
			return result;
		}
		public static string DesencriptarString(this string _cadenaAdesencriptar)
		{
			string result = string.Empty;
			byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
			//result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
			result = System.Text.Encoding.Unicode.GetString(decryted);
			return result;
		}

		/// <summary>
		/// Leer una linea determinada de un archivo de texto
		/// </summary>
		/// <param name="path">Camino completo sin barra invertida final</param>
		/// <param name="archivo">Nombre del archivo que se va a leer hubicado en path</param>
		/// <param name="linea">Numero de linea que se va a leer</param>
		/// <returns></returns>
		public static string LeerLineaArchivoTexto(string path, string archivo, int linea)
		{
			string line;
			string pathcompleto = path + "\\" + archivo;

			try
			{
				StreamReader sr = new StreamReader(pathcompleto);
				int numLinea = 1;

				line = sr.ReadLine();
				while (line != null)
				{
					if (numLinea == linea)
						return line;

					line = sr.ReadLine();
					numLinea++;
				}

			}
			catch (Exception)
			{
				return string.Empty;
			}


			return string.Empty;
		}

		public static System.Net.IPAddress ValidarIP(string directionIP)
		{
			System.Net.IPAddress ip = new System.Net.IPAddress(new byte[] { 0, 0, 0, 0 });
			System.Net.IPAddress.TryParse(directionIP, out ip);
			return ip;
		}

		public static string FormatoNumeroConDecimales(decimal numero)
		{
			return Math.Abs(numero).ToString("N");
		}
	}
}

