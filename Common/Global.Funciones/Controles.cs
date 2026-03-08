using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Global.Funciones
{
    static public class Controles
    {

        /// <summary>
        /// Se usa Generics para crear un metodo extensor que devuelve una coleccion del tipo de 
        /// control que le pases. Esto sirve para recorrer un formulario y ubicar un tipo de control
        /// determinado, leer alguna propiedad y actuar en consecuencia.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <returns></returns>
        public static List<T> GetControls<T>(this Control container) where T : Control
        {
           List<T> controls = new List<T>();
            foreach (Control c in container.Controls)
            {
                if (c is T)
                    controls.Add((T)c);

                controls.AddRange(GetControls<T>(c));
            }
            return controls;
        }

        public static List<T> GetControlarEntidades<T>(this Control container) where T : Control
        {
            List<T> controls = new List<T>();
            foreach (Control c in container.Controls)
            {
                if (c is T)
                    controls.Add((T)c);

                controls.AddRange(GetControlarEntidades<T>(c));
            }
            return controls;
        }

        public static void AbrirPagina(string target)
        {
            //string target = "https://www.digitaloneplus.com/manual-01";
            try
            {
                System.Diagnostics.Process.Start(target);
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == 2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }
    }
}
