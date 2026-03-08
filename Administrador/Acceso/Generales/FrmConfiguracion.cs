using Acceso.Clases.Datos.Generales;
using Acceso.Clases.Datos.RRHH;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Acceso.Generales
{
    public class FrmConfiguracion : Form
    {
        private TabControl tabConfig;
        private TabPage tabFichada;
        private TabPage tabPinVencidos;
        private TabPage tabNoticias;

        // Fichada config
        private CheckBox chkModoPIN;
        private Label lblExpiraDias;
        private NumericUpDown nudExpiraDias;
        private CheckBox chkModoDemo;
        private Button btnGuardar;
        private Label lblStatus;
        private Label lblTitulo;
        private Label lblDemoInfo;

        // PIN vencidos
        private DataGridView dgvPinVencidos;
        private Button btnForzarCambioTodos;
        private Label lblPinVencidosInfo;

        // Noticias
        private DataGridView dgvNoticias;
        private Button btnNoticiaAgregar;
        private Button btnNoticiaEditar;
        private Button btnNoticiaEliminar;
        private Label lblNoticiasInfo;

        public FrmConfiguracion()
        {
            InitializeComponent();
            if (!DesignMode)
                CargarConfiguracion();
        }

        private void InitializeComponent()
        {
            this.tabConfig = new System.Windows.Forms.TabControl();
            this.tabFichada = new System.Windows.Forms.TabPage();
            this.tabPinVencidos = new System.Windows.Forms.TabPage();
            this.tabNoticias = new System.Windows.Forms.TabPage();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.chkModoPIN = new System.Windows.Forms.CheckBox();
            this.lblExpiraDias = new System.Windows.Forms.Label();
            this.nudExpiraDias = new System.Windows.Forms.NumericUpDown();
            this.chkModoDemo = new System.Windows.Forms.CheckBox();
            this.lblDemoInfo = new System.Windows.Forms.Label();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblPinVencidosInfo = new System.Windows.Forms.Label();
            this.dgvPinVencidos = new System.Windows.Forms.DataGridView();
            this.btnForzarCambioTodos = new System.Windows.Forms.Button();
            this.dgvNoticias = new System.Windows.Forms.DataGridView();
            this.btnNoticiaAgregar = new System.Windows.Forms.Button();
            this.btnNoticiaEditar = new System.Windows.Forms.Button();
            this.btnNoticiaEliminar = new System.Windows.Forms.Button();
            this.lblNoticiasInfo = new System.Windows.Forms.Label();
            this.tabConfig.SuspendLayout();
            this.tabFichada.SuspendLayout();
            this.tabPinVencidos.SuspendLayout();
            this.tabNoticias.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudExpiraDias)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPinVencidos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNoticias)).BeginInit();
            this.SuspendLayout();
            //
            // tabConfig
            //
            this.tabConfig.Controls.Add(this.tabFichada);
            this.tabConfig.Controls.Add(this.tabPinVencidos);
            this.tabConfig.Controls.Add(this.tabNoticias);
            this.tabConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabConfig.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabConfig.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.tabConfig.ItemSize = new System.Drawing.Size(160, 36);
            this.tabConfig.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabConfig.Location = new System.Drawing.Point(0, 0);
            this.tabConfig.Name = "tabConfig";
            this.tabConfig.SelectedIndex = 0;
            this.tabConfig.Size = new System.Drawing.Size(784, 511);
            this.tabConfig.TabIndex = 0;
            this.tabConfig.SelectedIndexChanged += new System.EventHandler(this.tabConfig_SelectedIndexChanged);
            this.tabConfig.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabConfig_DrawItem);
            //
            // tabFichada
            //
            this.tabFichada.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(33)))), ((int)(((byte)(74)))));
            this.tabFichada.Controls.Add(this.lblTitulo);
            this.tabFichada.Controls.Add(this.chkModoPIN);
            this.tabFichada.Controls.Add(this.lblExpiraDias);
            this.tabFichada.Controls.Add(this.nudExpiraDias);
            this.tabFichada.Controls.Add(this.chkModoDemo);
            this.tabFichada.Controls.Add(this.lblDemoInfo);
            this.tabFichada.Controls.Add(this.btnGuardar);
            this.tabFichada.Controls.Add(this.lblStatus);
            this.tabFichada.ForeColor = System.Drawing.Color.Gainsboro;
            this.tabFichada.Location = new System.Drawing.Point(4, 26);
            this.tabFichada.Name = "tabFichada";
            this.tabFichada.Padding = new System.Windows.Forms.Padding(20);
            this.tabFichada.Size = new System.Drawing.Size(776, 481);
            this.tabFichada.TabIndex = 0;
            this.tabFichada.Text = "Fichada";
            //
            // tabPinVencidos
            //
            this.tabPinVencidos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(33)))), ((int)(((byte)(74)))));
            this.tabPinVencidos.Controls.Add(this.lblPinVencidosInfo);
            this.tabPinVencidos.Controls.Add(this.dgvPinVencidos);
            this.tabPinVencidos.Controls.Add(this.btnForzarCambioTodos);
            this.tabPinVencidos.ForeColor = System.Drawing.Color.Gainsboro;
            this.tabPinVencidos.Location = new System.Drawing.Point(4, 26);
            this.tabPinVencidos.Name = "tabPinVencidos";
            this.tabPinVencidos.Padding = new System.Windows.Forms.Padding(20);
            this.tabPinVencidos.Size = new System.Drawing.Size(776, 481);
            this.tabPinVencidos.TabIndex = 1;
            this.tabPinVencidos.Text = "PINs Vencidos";
            //
            // tabNoticias
            //
            this.tabNoticias.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(33)))), ((int)(((byte)(74)))));
            this.tabNoticias.Controls.Add(this.lblNoticiasInfo);
            this.tabNoticias.Controls.Add(this.dgvNoticias);
            this.tabNoticias.Controls.Add(this.btnNoticiaAgregar);
            this.tabNoticias.Controls.Add(this.btnNoticiaEditar);
            this.tabNoticias.Controls.Add(this.btnNoticiaEliminar);
            this.tabNoticias.ForeColor = System.Drawing.Color.Gainsboro;
            this.tabNoticias.Location = new System.Drawing.Point(4, 26);
            this.tabNoticias.Name = "tabNoticias";
            this.tabNoticias.Padding = new System.Windows.Forms.Padding(20);
            this.tabNoticias.Size = new System.Drawing.Size(776, 481);
            this.tabNoticias.TabIndex = 2;
            this.tabNoticias.Text = "Noticias";
            //
            // lblTitulo
            //
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(126)))), ((int)(((byte)(241)))));
            this.lblTitulo.Location = new System.Drawing.Point(20, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(400, 30);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Configuracion de Fichada";
            //
            // chkModoPIN
            //
            this.chkModoPIN.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.chkModoPIN.ForeColor = System.Drawing.Color.Gainsboro;
            this.chkModoPIN.Location = new System.Drawing.Point(20, 70);
            this.chkModoPIN.Name = "chkModoPIN";
            this.chkModoPIN.Size = new System.Drawing.Size(350, 30);
            this.chkModoPIN.TabIndex = 1;
            this.chkModoPIN.Text = "Habilitar fichada por PIN";
            //
            // lblExpiraDias
            //
            this.lblExpiraDias.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblExpiraDias.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblExpiraDias.Location = new System.Drawing.Point(50, 110);
            this.lblExpiraDias.Name = "lblExpiraDias";
            this.lblExpiraDias.Size = new System.Drawing.Size(320, 25);
            this.lblExpiraDias.TabIndex = 2;
            this.lblExpiraDias.Text = "Dias de expiracion de PIN (0 = no expira):";
            //
            // nudExpiraDias
            //
            this.nudExpiraDias.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.nudExpiraDias.Location = new System.Drawing.Point(380, 107);
            this.nudExpiraDias.Maximum = new decimal(new int[] { 365, 0, 0, 0 });
            this.nudExpiraDias.Name = "nudExpiraDias";
            this.nudExpiraDias.Size = new System.Drawing.Size(80, 29);
            this.nudExpiraDias.TabIndex = 3;
            this.nudExpiraDias.Value = new decimal(new int[] { 90, 0, 0, 0 });
            //
            // chkModoDemo
            //
            this.chkModoDemo.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.chkModoDemo.ForeColor = System.Drawing.Color.Gainsboro;
            this.chkModoDemo.Location = new System.Drawing.Point(20, 160);
            this.chkModoDemo.Name = "chkModoDemo";
            this.chkModoDemo.Size = new System.Drawing.Size(450, 30);
            this.chkModoDemo.TabIndex = 4;
            this.chkModoDemo.Text = "Habilitar modo demostracion (sin lector de huellas)";
            //
            // lblDemoInfo
            //
            this.lblDemoInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDemoInfo.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDemoInfo.Location = new System.Drawing.Point(50, 195);
            this.lblDemoInfo.Name = "lblDemoInfo";
            this.lblDemoInfo.Size = new System.Drawing.Size(500, 40);
            this.lblDemoInfo.TabIndex = 5;
            this.lblDemoInfo.Text = "El modo demo permite fichar seleccionando un legajo de una lista.\r\nUsar solo para" +
    " evaluacion del sistema.";
            //
            // btnGuardar
            //
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(126)))), ((int)(((byte)(241)))));
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Location = new System.Drawing.Point(20, 260);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(220, 40);
            this.btnGuardar.TabIndex = 6;
            this.btnGuardar.Text = "Guardar Configuracion";
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            //
            // lblStatus
            //
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblStatus.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblStatus.Location = new System.Drawing.Point(260, 268);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(300, 25);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "";
            //
            // lblPinVencidosInfo
            //
            this.lblPinVencidosInfo.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblPinVencidosInfo.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblPinVencidosInfo.Location = new System.Drawing.Point(20, 15);
            this.lblPinVencidosInfo.Name = "lblPinVencidosInfo";
            this.lblPinVencidosInfo.Size = new System.Drawing.Size(500, 25);
            this.lblPinVencidosInfo.TabIndex = 0;
            this.lblPinVencidosInfo.Text = "Legajos con PIN vencido o sin PIN asignado:";
            //
            // dgvPinVencidos
            //
            this.dgvPinVencidos.AllowUserToAddRows = false;
            this.dgvPinVencidos.AllowUserToDeleteRows = false;
            this.dgvPinVencidos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPinVencidos.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(43)))), ((int)(((byte)(84)))));
            this.dgvPinVencidos.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dgvPinVencidos.ForeColor = System.Drawing.Color.Black;
            this.dgvPinVencidos.Location = new System.Drawing.Point(20, 50);
            this.dgvPinVencidos.Name = "dgvPinVencidos";
            this.dgvPinVencidos.ReadOnly = true;
            this.dgvPinVencidos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPinVencidos.Size = new System.Drawing.Size(736, 340);
            this.dgvPinVencidos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPinVencidos.TabIndex = 1;
            //
            // btnForzarCambioTodos
            //
            this.btnForzarCambioTodos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnForzarCambioTodos.BackColor = System.Drawing.Color.DarkOrange;
            this.btnForzarCambioTodos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnForzarCambioTodos.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnForzarCambioTodos.ForeColor = System.Drawing.Color.White;
            this.btnForzarCambioTodos.Location = new System.Drawing.Point(20, 400);
            this.btnForzarCambioTodos.Name = "btnForzarCambioTodos";
            this.btnForzarCambioTodos.Size = new System.Drawing.Size(300, 35);
            this.btnForzarCambioTodos.TabIndex = 2;
            this.btnForzarCambioTodos.Text = "Forzar cambio de PIN (seleccionados)";
            this.btnForzarCambioTodos.UseVisualStyleBackColor = false;
            this.btnForzarCambioTodos.Click += new System.EventHandler(this.BtnForzarCambio_Click);
            //
            // lblNoticiasInfo
            //
            this.lblNoticiasInfo.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblNoticiasInfo.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblNoticiasInfo.Location = new System.Drawing.Point(20, 15);
            this.lblNoticiasInfo.Name = "lblNoticiasInfo";
            this.lblNoticiasInfo.Size = new System.Drawing.Size(500, 25);
            this.lblNoticiasInfo.TabIndex = 0;
            this.lblNoticiasInfo.Text = "Noticias para los empleados:";
            //
            // dgvNoticias
            //
            this.dgvNoticias.AllowUserToAddRows = false;
            this.dgvNoticias.AllowUserToDeleteRows = false;
            this.dgvNoticias.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvNoticias.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(43)))), ((int)(((byte)(84)))));
            this.dgvNoticias.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dgvNoticias.ForeColor = System.Drawing.Color.Black;
            this.dgvNoticias.Location = new System.Drawing.Point(20, 50);
            this.dgvNoticias.Name = "dgvNoticias";
            this.dgvNoticias.ReadOnly = true;
            this.dgvNoticias.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNoticias.Size = new System.Drawing.Size(736, 340);
            this.dgvNoticias.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvNoticias.TabIndex = 1;
            //
            // btnNoticiaAgregar
            //
            this.btnNoticiaAgregar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNoticiaAgregar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(126)))), ((int)(((byte)(241)))));
            this.btnNoticiaAgregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNoticiaAgregar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnNoticiaAgregar.ForeColor = System.Drawing.Color.White;
            this.btnNoticiaAgregar.Location = new System.Drawing.Point(20, 400);
            this.btnNoticiaAgregar.Name = "btnNoticiaAgregar";
            this.btnNoticiaAgregar.Size = new System.Drawing.Size(120, 35);
            this.btnNoticiaAgregar.TabIndex = 2;
            this.btnNoticiaAgregar.Text = "Agregar";
            this.btnNoticiaAgregar.UseVisualStyleBackColor = false;
            this.btnNoticiaAgregar.Click += new System.EventHandler(this.BtnNoticiaAgregar_Click);
            //
            // btnNoticiaEditar
            //
            this.btnNoticiaEditar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNoticiaEditar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(195)))), ((int)(((byte)(247)))));
            this.btnNoticiaEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNoticiaEditar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnNoticiaEditar.ForeColor = System.Drawing.Color.White;
            this.btnNoticiaEditar.Location = new System.Drawing.Point(150, 400);
            this.btnNoticiaEditar.Name = "btnNoticiaEditar";
            this.btnNoticiaEditar.Size = new System.Drawing.Size(120, 35);
            this.btnNoticiaEditar.TabIndex = 3;
            this.btnNoticiaEditar.Text = "Editar";
            this.btnNoticiaEditar.UseVisualStyleBackColor = false;
            this.btnNoticiaEditar.Click += new System.EventHandler(this.BtnNoticiaEditar_Click);
            //
            // btnNoticiaEliminar
            //
            this.btnNoticiaEliminar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNoticiaEliminar.BackColor = System.Drawing.Color.IndianRed;
            this.btnNoticiaEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNoticiaEliminar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnNoticiaEliminar.ForeColor = System.Drawing.Color.White;
            this.btnNoticiaEliminar.Location = new System.Drawing.Point(280, 400);
            this.btnNoticiaEliminar.Name = "btnNoticiaEliminar";
            this.btnNoticiaEliminar.Size = new System.Drawing.Size(120, 35);
            this.btnNoticiaEliminar.TabIndex = 4;
            this.btnNoticiaEliminar.Text = "Eliminar";
            this.btnNoticiaEliminar.UseVisualStyleBackColor = false;
            this.btnNoticiaEliminar.Click += new System.EventHandler(this.BtnNoticiaEliminar_Click);
            //
            // FrmConfiguracion
            //
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(33)))), ((int)(((byte)(74)))));
            this.ClientSize = new System.Drawing.Size(784, 511);
            this.Controls.Add(this.tabConfig);
            this.ForeColor = System.Drawing.Color.Gainsboro;
            this.Name = "FrmConfiguracion";
            this.Text = "Configuracion";
            this.tabConfig.ResumeLayout(false);
            this.tabFichada.ResumeLayout(false);
            this.tabPinVencidos.ResumeLayout(false);
            this.tabNoticias.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudExpiraDias)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPinVencidos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNoticias)).EndInit();
            this.ResumeLayout(false);
        }

        private void tabConfig_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tab = (TabControl)sender;
            TabPage page = tab.TabPages[e.Index];
            bool selected = (e.Index == tab.SelectedIndex);

            Color backColor = selected
                ? Color.FromArgb(172, 126, 241)
                : Color.FromArgb(44, 43, 84);
            Color foreColor = selected ? Color.White : Color.FromArgb(180, 180, 180);
            Font font = selected
                ? new Font("Segoe UI", 12F, FontStyle.Bold)
                : new Font("Segoe UI", 11F, FontStyle.Regular);

            using (SolidBrush bgBrush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(bgBrush, e.Bounds);
            }

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            using (SolidBrush txtBrush = new SolidBrush(foreColor))
            {
                e.Graphics.DrawString(page.Text, font, txtBrush, e.Bounds, sf);
            }

            font.Dispose();
        }

        private void tabConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabConfig.SelectedTab == tabPinVencidos)
                CargarPinVencidos();
            else if (tabConfig.SelectedTab == tabNoticias)
                CargarNoticias();
        }

        private void CargarConfiguracion()
        {
            try
            {
                string modoPIN = GRALVariablesGlobales.TraerValorDataBase("FichadaModoPIN");
                chkModoPIN.Checked = string.Equals(modoPIN, "true", StringComparison.OrdinalIgnoreCase);

                string expiraDias = GRALVariablesGlobales.TraerValorDataBase("PinExpiraDias");
                if (int.TryParse(expiraDias, out int dias))
                    nudExpiraDias.Value = dias;

                string modoDemo = GRALVariablesGlobales.TraerValorDataBase("FichadaModoDemo");
                chkModoDemo.Checked = string.Equals(modoDemo, "true", StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.OrangeRed;
                lblStatus.Text = "Error cargando: " + ex.Message;
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                GRALVariablesGlobales.GuardarVariable("FichadaModoPIN", chkModoPIN.Checked ? "true" : "false");
                GRALVariablesGlobales.GuardarVariable("PinExpiraDias", nudExpiraDias.Value.ToString());
                GRALVariablesGlobales.GuardarVariable("FichadaModoDemo", chkModoDemo.Checked ? "true" : "false");

                lblStatus.ForeColor = Color.LimeGreen;
                lblStatus.Text = "Configuracion guardada.";
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.OrangeRed;
                lblStatus.Text = "Error: " + ex.Message;
            }
        }

        #region PIN Vencidos

        private void CargarPinVencidos()
        {
            try
            {
                int diasExpira = (int)nudExpiraDias.Value;
                string query;

                if (diasExpira > 0)
                {
                    query = "SELECT l.NumeroLegajo AS [Legajo], l.Apellido + ' ' + l.Nombre AS [Nombre], " +
                        "CASE WHEN p.PinHash IS NULL THEN 'Sin PIN' " +
                        "WHEN p.PinMustChange = 1 THEN 'Cambio pendiente' " +
                        "WHEN DATEDIFF(DAY, p.PinChangedAt, GETUTCDATE()) > " + diasExpira + " THEN 'Vencido (' + CAST(DATEDIFF(DAY, p.PinChangedAt, GETUTCDATE()) AS VARCHAR) + ' dias)' " +
                        "ELSE 'Vigente' END AS [Estado PIN] " +
                        "FROM Legajo l LEFT JOIN LegajoPin p ON l.Id = p.LegajoId " +
                        "WHERE l.IsActive = 1 AND l.EmpresaId = " + Global.Datos.TenantContext.EmpresaId + " AND (p.PinHash IS NULL OR p.PinMustChange = 1 OR DATEDIFF(DAY, p.PinChangedAt, GETUTCDATE()) > " + diasExpira + ") " +
                        "ORDER BY l.Apellido, l.Nombre";
                }
                else
                {
                    query = "SELECT l.NumeroLegajo AS [Legajo], l.Apellido + ' ' + l.Nombre AS [Nombre], " +
                        "CASE WHEN p.PinHash IS NULL THEN 'Sin PIN' " +
                        "WHEN p.PinMustChange = 1 THEN 'Cambio pendiente' " +
                        "ELSE 'Vigente' END AS [Estado PIN] " +
                        "FROM Legajo l LEFT JOIN LegajoPin p ON l.Id = p.LegajoId " +
                        "WHERE l.IsActive = 1 AND l.EmpresaId = " + Global.Datos.TenantContext.EmpresaId + " AND (p.PinHash IS NULL OR p.PinMustChange = 1) " +
                        "ORDER BY l.Apellido, l.Nombre";
                }

                DataTable dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(query);
                dgvPinVencidos.DataSource = dt;
                lblPinVencidosInfo.Text = "Legajos con PIN vencido o sin PIN: " + dt.Rows.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando datos: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnForzarCambio_Click(object sender, EventArgs e)
        {
            if (dgvPinVencidos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione al menos un legajo.", "Atencion",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show(
                "Se marcara el cambio de PIN obligatorio para " + dgvPinVencidos.SelectedRows.Count + " legajo(s).\n¿Continuar?",
                "Forzar cambio de PIN",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            int count = 0;
            foreach (DataGridViewRow row in dgvPinVencidos.SelectedRows)
            {
                string legajoId = row.Cells["Legajo"].Value?.ToString();
                if (!string.IsNullOrEmpty(legajoId))
                {
                    try
                    {
                        Global.Datos.SQLServer.EjecutarSPsinRespuesta(
                            "EXEC EscritorioLegajoPIN_ForzarCambio '" + legajoId + "'", false);
                        count++;
                    }
                    catch { }
                }
            }

            MessageBox.Show("Se marco cambio obligatorio para " + count + " legajo(s).",
                "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CargarPinVencidos();
        }

        #endregion

        #region Noticias

        private void CargarNoticias()
        {
            try
            {
                string query = "SELECT Id, Titulo, " +
                    "CONVERT(VARCHAR(10), FechaDesde, 103) AS [Desde], " +
                    "CONVERT(VARCHAR(10), FechaHasta, 103) AS [Hasta], " +
                    "CASE WHEN IsPrivada = 1 THEN 'Si' ELSE 'No' END AS [Privada], " +
                    "CASE WHEN FechaDesde <= CAST(GETDATE() AS DATE) AND FechaHasta >= CAST(GETDATE() AS DATE) THEN 'Vigente' " +
                    "WHEN FechaDesde > CAST(GETDATE() AS DATE) THEN 'Programada' " +
                    "ELSE 'Vencida' END AS [Estado] " +
                    "FROM Noticia " +
                    "WHERE EmpresaId = " + Global.Datos.TenantContext.EmpresaId + " " +
                    "ORDER BY FechaDesde DESC";

                DataTable dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(query);
                dgvNoticias.DataSource = dt;

                // Ocultar columna Id
                if (dgvNoticias.Columns.Contains("Id"))
                    dgvNoticias.Columns["Id"].Visible = false;

                lblNoticiasInfo.Text = "Noticias para los empleados: " + dt.Rows.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando noticias: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNoticiaAgregar_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmNoticiaEdit(0))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    CargarNoticias();
            }
        }

        private void BtnNoticiaEditar_Click(object sender, EventArgs e)
        {
            if (dgvNoticias.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una noticia.", "Atencion",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int id = Convert.ToInt32(dgvNoticias.SelectedRows[0].Cells["Id"].Value);
            using (var frm = new FrmNoticiaEdit(id))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    CargarNoticias();
            }
        }

        private void BtnNoticiaEliminar_Click(object sender, EventArgs e)
        {
            if (dgvNoticias.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una noticia.", "Atencion",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string titulo = dgvNoticias.SelectedRows[0].Cells["Titulo"].Value?.ToString() ?? "";
            var result = MessageBox.Show(
                "¿Eliminar la noticia \"" + titulo + "\"?",
                "Confirmar eliminacion",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                int id = Convert.ToInt32(dgvNoticias.SelectedRows[0].Cells["Id"].Value);
                string sql = "DELETE FROM Noticia WHERE Id = " + id + " AND EmpresaId = " + Global.Datos.TenantContext.EmpresaId;
                Global.Datos.SQLServer.EjecutarSPsinRespuesta(sql, false);
                CargarNoticias();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error eliminando: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }

    /// <summary>
    /// Formulario para agregar/editar una noticia.
    /// </summary>
    internal class FrmNoticiaEdit : Form
    {
        private TextBox txtTitulo;
        private TextBox txtContenido;
        private DateTimePicker dtpDesde;
        private DateTimePicker dtpHasta;
        private CheckBox chkPrivada;
        private Button btnAceptar;
        private Button btnCancelar;
        private Label lblTituloLabel;
        private Label lblContenidoLabel;
        private Label lblDesdeLabel;
        private Label lblHastaLabel;

        private int _noticiaId;

        public FrmNoticiaEdit(int noticiaId)
        {
            _noticiaId = noticiaId;
            InitializeComponent();
            if (_noticiaId > 0)
                CargarNoticia();
        }

        private void InitializeComponent()
        {
            this.txtTitulo = new System.Windows.Forms.TextBox();
            this.txtContenido = new System.Windows.Forms.TextBox();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.chkPrivada = new System.Windows.Forms.CheckBox();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.lblTituloLabel = new System.Windows.Forms.Label();
            this.lblContenidoLabel = new System.Windows.Forms.Label();
            this.lblDesdeLabel = new System.Windows.Forms.Label();
            this.lblHastaLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // lblTituloLabel
            //
            this.lblTituloLabel.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.lblTituloLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblTituloLabel.Location = new System.Drawing.Point(25, 25);
            this.lblTituloLabel.Name = "lblTituloLabel";
            this.lblTituloLabel.Size = new System.Drawing.Size(120, 30);
            this.lblTituloLabel.TabIndex = 0;
            this.lblTituloLabel.Text = "Titulo:";
            //
            // txtTitulo
            //
            this.txtTitulo.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtTitulo.Location = new System.Drawing.Point(25, 58);
            this.txtTitulo.MaxLength = 200;
            this.txtTitulo.Name = "txtTitulo";
            this.txtTitulo.Size = new System.Drawing.Size(660, 32);
            this.txtTitulo.TabIndex = 1;
            //
            // lblContenidoLabel
            //
            this.lblContenidoLabel.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.lblContenidoLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblContenidoLabel.Location = new System.Drawing.Point(25, 100);
            this.lblContenidoLabel.Name = "lblContenidoLabel";
            this.lblContenidoLabel.Size = new System.Drawing.Size(150, 30);
            this.lblContenidoLabel.TabIndex = 2;
            this.lblContenidoLabel.Text = "Contenido:";
            //
            // txtContenido
            //
            this.txtContenido.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtContenido.Location = new System.Drawing.Point(25, 133);
            this.txtContenido.Multiline = true;
            this.txtContenido.Name = "txtContenido";
            this.txtContenido.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtContenido.Size = new System.Drawing.Size(660, 180);
            this.txtContenido.TabIndex = 3;
            //
            // lblDesdeLabel
            //
            this.lblDesdeLabel.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.lblDesdeLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblDesdeLabel.Location = new System.Drawing.Point(25, 330);
            this.lblDesdeLabel.Name = "lblDesdeLabel";
            this.lblDesdeLabel.Size = new System.Drawing.Size(90, 30);
            this.lblDesdeLabel.TabIndex = 4;
            this.lblDesdeLabel.Text = "Desde:";
            //
            // dtpDesde
            //
            this.dtpDesde.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde.Location = new System.Drawing.Point(115, 327);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(180, 31);
            this.dtpDesde.TabIndex = 5;
            //
            // lblHastaLabel
            //
            this.lblHastaLabel.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.lblHastaLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblHastaLabel.Location = new System.Drawing.Point(330, 330);
            this.lblHastaLabel.Name = "lblHastaLabel";
            this.lblHastaLabel.Size = new System.Drawing.Size(80, 30);
            this.lblHastaLabel.TabIndex = 6;
            this.lblHastaLabel.Text = "Hasta:";
            //
            // dtpHasta
            //
            this.dtpHasta.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta.Location = new System.Drawing.Point(410, 327);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(180, 31);
            this.dtpHasta.TabIndex = 7;
            this.dtpHasta.Value = System.DateTime.Today.AddDays(30);
            //
            // chkPrivada
            //
            this.chkPrivada.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.chkPrivada.ForeColor = System.Drawing.Color.Gainsboro;
            this.chkPrivada.Location = new System.Drawing.Point(25, 380);
            this.chkPrivada.Name = "chkPrivada";
            this.chkPrivada.Size = new System.Drawing.Size(400, 30);
            this.chkPrivada.TabIndex = 8;
            this.chkPrivada.Text = "Noticia privada (solo visible en portal)";
            //
            // btnAceptar
            //
            this.btnAceptar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(126)))), ((int)(((byte)(241)))));
            this.btnAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAceptar.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.btnAceptar.ForeColor = System.Drawing.Color.White;
            this.btnAceptar.Location = new System.Drawing.Point(25, 430);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(200, 50);
            this.btnAceptar.TabIndex = 9;
            this.btnAceptar.Text = "Guardar";
            this.btnAceptar.UseVisualStyleBackColor = false;
            this.btnAceptar.Click += new System.EventHandler(this.BtnAceptar_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(240, 430);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(200, 50);
            this.btnCancelar.TabIndex = 10;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            //
            // FrmNoticiaEdit
            //
            this.AcceptButton = this.btnAceptar;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(33)))), ((int)(((byte)(74)))));
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(720, 505);
            this.Controls.Add(this.lblTituloLabel);
            this.Controls.Add(this.txtTitulo);
            this.Controls.Add(this.lblContenidoLabel);
            this.Controls.Add(this.txtContenido);
            this.Controls.Add(this.lblDesdeLabel);
            this.Controls.Add(this.dtpDesde);
            this.Controls.Add(this.lblHastaLabel);
            this.Controls.Add(this.dtpHasta);
            this.Controls.Add(this.chkPrivada);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.btnCancelar);
            this.ForeColor = System.Drawing.Color.Gainsboro;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmNoticiaEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nueva Noticia";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void CargarNoticia()
        {
            try
            {
                string query = "SELECT Titulo, Contenido, FechaDesde, FechaHasta, IsPrivada " +
                    "FROM Noticia WHERE Id = " + _noticiaId + " AND EmpresaId = " + Global.Datos.TenantContext.EmpresaId;

                DataTable dt = Global.Datos.SQLServer.EjecutarParaSoloLectura(query);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtTitulo.Text = row["Titulo"].ToString();
                    txtContenido.Text = row["Contenido"] == DBNull.Value ? "" : row["Contenido"].ToString();
                    dtpDesde.Value = Convert.ToDateTime(row["FechaDesde"]);
                    dtpHasta.Value = Convert.ToDateTime(row["FechaHasta"]);
                    chkPrivada.Checked = Convert.ToBoolean(row["IsPrivada"]);
                    this.Text = "Editar Noticia";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando noticia: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MessageBox.Show("El titulo es obligatorio.", "Validacion",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitulo.Focus();
                return;
            }

            if (dtpHasta.Value.Date < dtpDesde.Value.Date)
            {
                MessageBox.Show("La fecha Hasta no puede ser anterior a Desde.", "Validacion",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int empresaId = Global.Datos.TenantContext.EmpresaId;

                if (_noticiaId > 0)
                {
                    // UPDATE
                    SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@Titulo", txtTitulo.Text.Trim()),
                        new SqlParameter("@Contenido", string.IsNullOrWhiteSpace(txtContenido.Text) ? (object)DBNull.Value : txtContenido.Text.Trim()),
                        new SqlParameter("@FechaDesde", dtpDesde.Value.Date),
                        new SqlParameter("@FechaHasta", dtpHasta.Value.Date),
                        new SqlParameter("@IsPrivada", chkPrivada.Checked),
                        new SqlParameter("@Id", _noticiaId),
                        new SqlParameter("@EmpresaId", empresaId)
                    };

                    Global.Datos.SQLServer.EjecutarSPInsertUpdateBool(
                        "UPDATE Noticia SET Titulo = @Titulo, Contenido = @Contenido, " +
                        "FechaDesde = @FechaDesde, FechaHasta = @FechaHasta, IsPrivada = @IsPrivada " +
                        "WHERE Id = @Id AND EmpresaId = @EmpresaId",
                        pars, false);
                }
                else
                {
                    // INSERT
                    SqlParameter[] pars = new SqlParameter[]
                    {
                        new SqlParameter("@EmpresaId", empresaId),
                        new SqlParameter("@Titulo", txtTitulo.Text.Trim()),
                        new SqlParameter("@Contenido", string.IsNullOrWhiteSpace(txtContenido.Text) ? (object)DBNull.Value : txtContenido.Text.Trim()),
                        new SqlParameter("@FechaDesde", dtpDesde.Value.Date),
                        new SqlParameter("@FechaHasta", dtpHasta.Value.Date),
                        new SqlParameter("@IsPrivada", chkPrivada.Checked),
                        new SqlParameter("@CreatedAt", DateTime.UtcNow),
                        new SqlParameter("@CreatedBy", "Administrador")
                    };

                    Global.Datos.SQLServer.EjecutarSPInsertUpdateBool(
                        "INSERT INTO Noticia (EmpresaId, Titulo, Contenido, FechaDesde, FechaHasta, IsPrivada, CreatedAt, CreatedBy) " +
                        "VALUES (@EmpresaId, @Titulo, @Contenido, @FechaDesde, @FechaHasta, @IsPrivada, @CreatedAt, @CreatedBy)",
                        pars, false);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error guardando: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
