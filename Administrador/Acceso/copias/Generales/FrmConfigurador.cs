using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Acceso.Clases.Datos;
using Global.Funciones;
using Acceso.Datos.Model;
using System.Linq;

namespace Acceso.Generales
{
	public partial class FrmConfigurador : Global.Formularios.FrmEntidadesBase
	{
		private List<GRALVariablesGlobales> lVar;
		AccesosEntidades db = new AccesosEntidades();


		public FrmConfigurador()
		{
			InitializeComponent();
			CargarVistas();
			CargarVariablesGlobales();
			trackNumericoDigitosDocumento.ValorCambiado += new EventHandler(trackNumericoDigitosDocumento_ValorCambiado);
		}

		private void CargarVistas()
		{
			//comboBasesDeDatos.DataSource = db.DYGPDATABASES.ToList();
			//comboBasesDeDatos.DisplayMember = "name";
			//comboBasesDeDatos.ValueMember = "name";
		}

		private void CargarVariablesGlobales()
		{
			//lVar = (from n in db.GRALVariablesGlobales select n).ToList();
			lVar = db.GRALVariablesGlobales.ToList();
			foreach (var ovar in lVar)
			{
				switch (ovar.sVariableGlobalID)
				{
					case "GRALEmpresaRazonSocial":
						TextoEmpresaRazonSocial.Text = ovar.sValor;
						break;
					case "GRALCompatibilidadDynamics":
						checkDynamics.Checked = ovar.sValor == "True";
						break;
					case "GRALCompatibilidadSumma":
						checkSumma.Checked = ovar.sValor == "True";
						break;
					case "GRALDigitosCodigoSucursal":
						trackSucursal.Valor = int.Parse(ovar.sValor.ToString());
						break;
					case "INVEDigitosCodigoArticulo":
						trackDigitosCodigoArticulo.Valor = int.Parse(ovar.sValor);
						break;
					case "INVEDigitosCodigoClaseArticulo":
						trackDigitosCodigoClaseArticulo.Valor = int.Parse(ovar.sValor);
						break;
					case "INVEDigitosCodigoTipoMovimiento":
						trackDigitosTipoMovimientoInventario.Valor = int.Parse(ovar.sValor);
						break;
					case "INVEMascDocumentoIncluirTipo":
						checkTipoDocumento.Checked = bool.Parse(ovar.sValor);
						break;
					case "INVEMascDocumentoIncluirSucursal":
						checkDocumentoSucursal.Checked = bool.Parse(ovar.sValor);
						break;
					case "INVEMascDigitosDocumento":
						trackNumericoDigitosDocumento.Valor = int.Parse(ovar.sValor);
						break;
					case "INVEMascDocumentoIncluirTipoSeparador":
						comboSeparadorTipo.Text = ovar.sValor;
						break;
					case "INVEMascDocumentoIncluirSucursalSeparador":
						comboSeparadorSucursal.Text = ovar.sValor;
						break;
					case "INVEDigitosCodigoDeposito":
						trackDeposito.Valor = int.Parse(ovar.sValor.ToString());
						break;
					case "RRHHDigitosCodigoTurnos":
						trackDigitosCodigoTurnos.Valor = int.Parse(ovar.sValor);
						break;
					case "RRHHDigitosCodigoIncidencia":
						trackDigitosCodigoIncidencias.Valor = int.Parse(ovar.sValor);
						break;
                    case "RRHHDigitosCodigoTiposLegajos":
                        trackDigitoCodigoTipo.Valor = int.Parse(ovar.sValor);
						break;
                    case "CTRLRegistrosMaximosLupita":
						numericoMaxRegLupita.Value = int.Parse(ovar.sValor);
						break;
					case "DYGPModuloProduccionInstalado":
						checkBoxManufacturingGP.Checked = bool.Parse(ovar.sValor);
						break;
					case "DYGPCONTOpenYear":
						numericoGPOpenYear.Value = int.Parse(ovar.sValor);
						break;
					case "GRALFeriadoDesde":
						fechaDesdeHastaFeriados.FechaDesde = DateTime.Parse(ovar.sValor).Date;
						break;
					case "GRALFeriadoHasta":
						fechaDesdeHastaFeriados.FechaHasta = DateTime.Parse(ovar.sValor).Date;
						break;
					case "GRALDigitosControlEntidadTextoBusqueda":
						trackControlEntidadBusqueda.Valor = int.Parse(ovar.sValor);
						break;
					case "DYGPBasedeDatos":
						if (ovar.sValor != string.Empty)
							comboBasesDeDatos.Text = ovar.sValor;
						break;
					default:
						break;
				}
			}
			
		}
		private void GuardarVariablesGlobales()
		{
			foreach (GRALVariablesGlobales ovar in lVar)
			{
				switch (ovar.sVariableGlobalID)
				{
					case "GRALEmpresaRazonSocial":
						ovar.sValor = TextoEmpresaRazonSocial.Text;
						break;
					case "GRALCompatibilidadDynamics":
						ovar.sValor = checkDynamics.Checked.ToString();
						break;
					case "GRALCompatibilidadSumma":
						ovar.sValor = checkSumma.Checked.ToString();
						break;
					case "GRALDigitosCodigoSucursal":
						ovar.sValor = trackSucursal.Valor.ToString();
						break;
					case "GRALDigitosControlEntidadTextoBusqueda":
						ovar.sValor = trackControlEntidadBusqueda.Valor.ToString();
						break;
					case "INVEDigitosCodigoArticulo":
						trackDigitosCodigoArticulo.Valor = int.Parse(ovar.sValor);
						break;
					case "INVEDigitosCodigoClaseArticulo":
						ovar.sValor = trackDigitosCodigoClaseArticulo.Valor.ToString();
						break;
					case "INVEDigitosCodigoTipoMovimiento":
						ovar.sValor = trackDigitosTipoMovimientoInventario.Valor.ToString();
						break;
					case "INVEMascDocumentoIncluirTipo":
						ovar.sValor = checkTipoDocumento.Checked.ToString();
						break;
					case "INVEMascDocumentoIncluirSucursal":
						ovar.sValor = checkDocumentoSucursal.Checked.ToString();
						break;
					case "INVEMascDigitosDocumento":
						ovar.sValor = trackNumericoDigitosDocumento.Valor.ToString();
						break;
					case "INVEMascDocumentoIncluirTipoSeparador":
						ovar.sValor = comboSeparadorTipo.Text.ToString();
						break;
					case "INVEMascDocumentoIncluirSucursalSeparador":
						ovar.sValor = comboSeparadorSucursal.Text.ToString();
						break;
					case "INVEDigitosCodigoDeposito":
						ovar.sValor = trackDeposito.Valor.ToString();
						break;
					case "RRHHDigitosCodigoTurnos":
						ovar.sValor = trackDigitosCodigoTurnos.Valor.ToString();
						break;
                    case "RRHHDigitosCodigoTiposLegajos":
                        ovar.sValor = trackDigitoCodigoTipo.Valor.ToString();
                        break;
					case "RRHHDigitosCodigoIncidencia":
						ovar.sValor = trackDigitosCodigoIncidencias.Valor.ToString();
						break;
					case "CTRLRegistrosMaximosLupita":
						ovar.sValor = numericoMaxRegLupita.Value.ToString();
						break;
					case "DYGPModuloProduccionInstalado":
						ovar.sValor = checkBoxManufacturingGP.Checked.ToString();
						break;
					case "DYGPBasedeDatos":
						ovar.sValor = comboBasesDeDatos.Text.Trim();
						break;
					case "DYGPCONTOpenYear":
						ovar.sValor = numericoGPOpenYear.Value.ToString();
						break;
					case "GRALFeriadoDesde":
						ovar.sValor = fechaDesdeHastaFeriados.FechaDesde.Date.ToShortDateString();
						break;
					case "GRALFeriadoHasta":
						ovar.sValor = fechaDesdeHastaFeriados.FechaHasta.Date.ToShortDateString();
						break;
					default:
						break;
				}

			}
			db.SaveChanges();
		}

		private void botonMenuGuardar_Click(object sender, EventArgs e)
		{
			try
			{
				GuardarVariablesGlobales();
				Informar("Variables Globales Almacenadas");
			}
			catch (Exception ex)
			{

				MessageBox.Show("No se pudieron almacenar las variables Globales " + (Char)13 + ex.Message);
			}
		}

		private void checkDynamics_CheckedChanged(object sender, EventArgs e)
		{
			checkBoxManufacturingGP.Enabled = checkDynamics.Checked;
			comboBasesDeDatos.Enabled = checkDynamics.Checked;
			numericoGPOpenYear.Enabled = checkDynamics.Checked;

			if (checkDynamics.Checked == false)
			{
				checkBoxManufacturingGP.Checked = false;
			}

		}

		private void checkTipoDocumento_CheckedChanged(object sender, EventArgs e)
		{

			if (!checkTipoDocumento.Checked)
				comboSeparadorTipo.Text = " ";

			comboSeparadorTipo.Enabled = checkTipoDocumento.Checked;


			MascaraDocumentoInventario();
		}
		private void MascaraDocumentoInventario()
		{
			int digitos = trackNumericoDigitosDocumento.Valor;
			this.textoMascaraDocumento.Text = String.Empty;

			if (checkTipoDocumento.Checked)
				textoMascaraDocumento.Text = "E";

			switch (comboSeparadorTipo.Text)
			{
				case " ":
					break;
				default:
					textoMascaraDocumento.Text = textoMascaraDocumento.Text + comboSeparadorTipo.Text.Trim();
					break;
			}


			if (checkDocumentoSucursal.Checked)
				textoMascaraDocumento.Text = textoMascaraDocumento.Text + "9999";


			switch (comboSeparadorSucursal.Text)
			{
				case " ":
					break;
				default:
					textoMascaraDocumento.Text = textoMascaraDocumento.Text + comboSeparadorSucursal.Text.Trim();
					break;
			}


			textoMascaraDocumento.Text = textoMascaraDocumento.Text + Global.Funciones.Formatos.StringReplicate('5', digitos);

		}

		private void checkDocumentoSucursal_CheckedChanged(object sender, EventArgs e)
		{
			if (!checkDocumentoSucursal.Checked)
				comboSeparadorSucursal.Text = " ";

			comboSeparadorSucursal.Enabled = checkDocumentoSucursal.Checked;

			MascaraDocumentoInventario();
		}

		private void trackNumericoDigitosDocumento_ValorCambiado(object sender, EventArgs e)
		{
			MascaraDocumentoInventario();
		}

		private void comboDesplegable1_SelectedIndexChanged(object sender, EventArgs e)
		{
			MascaraDocumentoInventario();
		}

		private void comboDesplegable2_SelectedIndexChanged(object sender, EventArgs e)
		{
			MascaraDocumentoInventario();
		}

		private void trackSucursal_Load(object sender, EventArgs e)
		{

		}

		private void trackNumericoCombo1_Load(object sender, EventArgs e)
		{

		}
	}
}
