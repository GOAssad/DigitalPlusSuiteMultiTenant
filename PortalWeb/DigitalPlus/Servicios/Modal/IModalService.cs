using DigitalPlus.Entidades.Modal;
using Microsoft.AspNetCore.Components;

namespace DigitalPlus.Servicios.Modal
{
    public interface IModalService
    {
        event Action<ModalResult> OnClose;

        void Show<T>(string title, ModalParameters parameters) where T : ComponentBase;
        void Show<T>(string title, ModalParameters parameters = null, ModalOptions options = null) where T : ComponentBase;
        void Show(Type contentComponent, string title, ModalParameters parameters, ModalOptions options);
        void Close(ModalResult modalResult);
        void Cancel();

    }
}
