using Acceso.Clases.Datos.Generales;
using Acceso.Clases.Datos.RRHH;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Acceso.Generales
{
    public class FrmConfiguracion : Form
    {
        private TabControl tabConfig;
        private TabPage tabFichada;
        private TabPage tabPinVencidos;

        // Fichada config
        private CheckBox chkModoPIN;
        private Label lblExpiraDias;
        private NumericUpDown nudExpiraDias;
        private CheckBox chkModoDemo;
        private Button btnGuardar;
        private Label lblStatus;

        // PIN vencidos
        private DataGridView dgvPinVencidos;
        private Button btnForzarCambioTodos;
        private Label lblPinVencidosInfo;

        public FrmConfiguracion()
        {
            this.Text = "Configuracion";
            InitializeComponent();
            CargarConfiguracion();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(700, 500);
            this.BackColor = Color.FromArgb(34, 33, 74);
            this.ForeColor = Color.Gainsboro;

            tabConfig = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F)
            };

            // === Tab Fichada ===
            tabFichada = new TabPage("Fichada")
            {
                BackColor = Color.FromArgb(34, 33, 74),
                ForeColor = Color.Gainsboro,
                Padding = new Padding(20)
            };

            var lblTitulo = new Label
            {
                Text = "Configuracion de Fichada",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(172, 126, 241),
                Location = new Point(20, 20),
                Size = new Size(400, 30)
            };

            chkModoPIN = new CheckBox
            {
                Text = "Habilitar fichada por PIN",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.Gainsboro,
                Location = new Point(20, 70),
                Size = new Size(350, 30)
            };

            lblExpiraDias = new Label
            {
                Text = "Dias de expiracion de PIN (0 = no expira):",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.Gainsboro,
                Location = new Point(50, 110),
                Size = new Size(320, 25)
            };

            nudExpiraDias = new NumericUpDown
            {
                Font = new Font("Segoe UI", 12F),
                Location = new Point(380, 107),
                Size = new Size(80, 30),
                Minimum = 0,
                Maximum = 365,
                Value = 90
            };

            chkModoDemo = new CheckBox
            {
                Text = "Habilitar modo demostracion (sin lector de huellas)",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.Gainsboro,
                Location = new Point(20, 160),
                Size = new Size(450, 30)
            };

            var lblDemoInfo = new Label
            {
                Text = "El modo demo permite fichar seleccionando un legajo de una lista.\nUsar solo para evaluacion del sistema.",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.DarkGray,
                Location = new Point(50, 195),
                Size = new Size(500, 40)
            };

            btnGuardar = new Button
            {
                Text = "Guardar Configuracion",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                BackColor = Color.FromArgb(172, 126, 241),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(20, 260),
                Size = new Size(220, 40)
            };
            btnGuardar.Click += BtnGuardar_Click;

            lblStatus = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.LimeGreen,
                Location = new Point(260, 268),
                Size = new Size(300, 25)
            };

            tabFichada.Controls.AddRange(new Control[] {
                lblTitulo, chkModoPIN, lblExpiraDias, nudExpiraDias,
                chkModoDemo, lblDemoInfo, btnGuardar, lblStatus
            });

            // === Tab PIN Vencidos ===
            tabPinVencidos = new TabPage("PINs Vencidos")
            {
                BackColor = Color.FromArgb(34, 33, 74),
                ForeColor = Color.Gainsboro,
                Padding = new Padding(20)
            };

            lblPinVencidosInfo = new Label
            {
                Text = "Legajos con PIN vencido o sin PIN asignado:",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.Gainsboro,
                Location = new Point(20, 15),
                Size = new Size(500, 25)
            };

            dgvPinVencidos = new DataGridView
            {
                Location = new Point(20, 50),
                Size = new Size(620, 300),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.FromArgb(44, 43, 84),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10F),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            btnForzarCambioTodos = new Button
            {
                Text = "Forzar cambio de PIN (seleccionados)",
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.DarkOrange,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(20, 360),
                Size = new Size(300, 35)
            };
            btnForzarCambioTodos.Click += BtnForzarCambio_Click;

            tabPinVencidos.Controls.AddRange(new Control[] {
                lblPinVencidosInfo, dgvPinVencidos, btnForzarCambioTodos
            });

            tabConfig.TabPages.Add(tabFichada);
            tabConfig.TabPages.Add(tabPinVencidos);
            tabConfig.SelectedIndexChanged += (s, e) =>
            {
                if (tabConfig.SelectedTab == tabPinVencidos)
                    CargarPinVencidos();
            };

            this.Controls.Add(tabConfig);
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
    }
}
