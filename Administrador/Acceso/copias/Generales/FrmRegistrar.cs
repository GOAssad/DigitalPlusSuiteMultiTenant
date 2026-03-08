using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Acceso.Clases.Datos.Generales;

namespace Acceso.Generales
{
    public partial class FrmRegistrar : Global.Formularios.FrmEntidadesBase
    {
        private DPFP.Template Template;
        bool yapaso = false;
        GRALUsuariosRep oUsuario = new GRALUsuariosRep();

        public FrmRegistrar()
        {
            InitializeComponent();
            textoEmail.DataBindings.Add("Valor", oUsuario, "sEmail", true, DataSourceUpdateMode.OnValidation);
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        public override void ActualizarTodo()
        {
            base.ActualizarTodo();
            oUsuario.InicializarporsID(controlEntidadUsuarios.textoCodigo.Text.Trim());
            //			textoemail.Text = oUsuario.sEmail;
            textoEmail.Valor = oUsuario.sEmail;
            
            if (yapaso)
                return;

        }

        private void BotonRegistrar_Click(object sender, EventArgs e)
        {
            uAreu.CapturarHuella capturar = new uAreu.CapturarHuella();
            capturar.OnTemplate += this.OnTemplate;
            capturar.ShowDialog();
        }
        private void OnTemplate(DPFP.Template template)
        {
            this.Invoke(new uAreu.Function(delegate ()
            {
                Template = template;
                //btnAgregar.Enabled = (Template != null);
                //if (Template != null)
                //{
                //    MessageBox.Show("The fingerprint template is ready for fingerprint verification.", "Fingerprint Enrollment");
                //    txtHuella.Text = "Huella capturada correctamente";
                //}
                //else
                //{
                //    MessageBox.Show("The fingerprint template is not valid. Repeat fingerprint enrollment.", "Fingerprint Enrollment");
                //}
            }));
        }
    }
}
