using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Global.Datos
{
	public static class TXTArchivos
	{

		public static void AgregarLinea(string path, string texto)
		{

			StreamWriter archivo = File.AppendText(path);
			archivo.WriteLine(texto);
			archivo.Close();
		}

	}
}
