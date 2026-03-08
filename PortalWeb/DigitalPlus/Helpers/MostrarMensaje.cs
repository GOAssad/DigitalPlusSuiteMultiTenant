using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalPlus.Helpers
{
    public interface IMostrarMensajes
    {
        Task MostrarMensajeError(string mensaje);
        Task MostrarMensajeExitoso(string mensaje);
        Task<bool> MostrarMensajeBorrar(string cuerpo);
        Task<bool> MostrarMensajeSiNo(string cuerpo);
        Task MostrarMensajeAlerta(string mensaje);
        Task<bool> MostrarMensajeTemporal(string mensaje, string texto);
    }
    public class MostrarMensajes : IMostrarMensajes
    {
        private readonly IJSRuntime js;
        private readonly ListadoVariablesGlobales _variablesGlobales;

        public MostrarMensajes(IJSRuntime js, ListadoVariablesGlobales variablesGlobales)
        {
            this.js = js;
            _variablesGlobales = variablesGlobales;
        }

        public async Task MostrarMensajeError(string mensaje)
        {
            await MostrarMensaje("Error", mensaje, "error");
        }

        public async Task MostrarMensajeExitoso(string mensaje)
        {
            await MostrarMensaje("Exitoso", mensaje, "success");
        }

        private async ValueTask MostrarMensaje(string titulo, string mensaje, string tipoMensaje)
        {
            await js.InvokeVoidAsync("Swal.fire", titulo, mensaje, tipoMensaje);
            //await js.InvokeVoidAsync("confirm", mensaje);
        }
               

        public async Task<bool> MostrarMensajeBorrar(string cuerpo)
        {
            string empresa = _variablesGlobales.Variables.Where(x => x.sId == "EMP_NOMBRE_CORTO").Select(x => x.Valor).FirstOrDefault();
            string cuerpofinal = $"Esta seguro que desea eliminar el registro: {cuerpo}";

            return await js.InvokeAsync<bool>("confirmar", $"{empresa}: Eliminar Registro", cuerpofinal, "question");
        }

        public async Task<bool> MostrarMensajeSiNo(string cuerpo)
        {

            return await js.InvokeAsync<bool>("SiNo", "Que desea Hacer?", cuerpo, "Si");
        }

        public async Task MostrarMensajeAlerta(string mensaje)
        {
            await MostrarMensaje("Atencion", mensaje, "warning");
        }

        public async Task<bool> MostrarMensajeTemporal(string titulo, string texto)
        {
            return await js.InvokeAsync<bool>("ConfirmarSinBotones", titulo, texto);
        }
    }
}
