using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Acceso.Clases.Datos.Generales;

namespace Acceso.uAreu
{
    public partial class FrmRegistrarSimple : Global.Formularios.FrmBase
    {
        GRALUsuariosRep oUsuario = new GRALUsuariosRep();
        bool yapaso = false;


        // UareU
        private DPFP.Template Template;


        public FrmRegistrarSimple()
        {
            InitializeComponent();
            textoEtiquetaEmail.DataBindings.Add("Valor", oUsuario, "sEmail", true, DataSourceUpdateMode.OnValidation);
        }

        public override void ActualizarTodo()
        {
            base.ActualizarTodo();
            oUsuario.InicializarporsID(controlEntidadUsuarios.textoCodigo.Text.Trim());
            //			textoemail.Text = oUsuario.sEmail;
            textoEtiquetaEmail.Valor = oUsuario.sEmail;

            if (yapaso)
                return;

        }

        private void btnRegistrarHuella_Click(object sender, EventArgs e)
        {
            CapturarHuella capturar = new CapturarHuella();
            capturar.OnTemplate += this.OnTemplate;
            capturar.ShowDialog();

        }

        private void  OnTemplate(DPFP.Template template)
        {
            this.Invoke(new Function(delegate ()
            {
                Template = template;
                btnAgregar.Enabled = (Template != null);
                if (Template != null)
                {
                    MessageBox.Show("The fingerprint template is ready for fingerprint verification.", "Fingerprint Enrollment");
                    txtHuella.Text = "Huella capturada correctamente";
                }
                else
                {
                    MessageBox.Show("The fingerprint template is not valid. Repeat fingerprint enrollment.", "Fingerprint Enrollment");
                }
            }));
        }
    }
}
