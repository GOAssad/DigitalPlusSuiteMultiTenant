using DigitalPlus.Entidades.Modal;
using DigitalPlus.Servicios.Modal;
using Microsoft.AspNetCore.Components;

namespace DigitalPlus.Pages.Shared
{
    public partial class BlazoredModal : IDisposable
    {
        const string _defaultStyle = "blazored-modal";
        const string _defaultPosition = "blazored-modal-center";

        [Inject] private IModalService ModalService { get; set; }

        [Parameter] public bool HideHeader { get; set; }
        [Parameter] public bool HideCloseButton { get; set; }
        [Parameter] public bool DisableBackgroundCancel { get; set; }
        [Parameter] public string Position { get; set; }
        [Parameter] public string Style { get; set; }

        private bool ComponentdisableBackgroundCancel { get; set; }
        private bool ComponentHideHeader { get; set; }
        private bool ComponentHideCloseButton { get; set; }
        private string ComponentPosition { get; set; }
        private string ComponentStyle { get; set; }
        private bool IsVisible { get; set; }
        private string Title { get; set; }
        private RenderFragment Content { get; set; }
        private ModalParameters Parameters { get; set; }

        protected override void OnInitialized()
        {
            ((ModalService)ModalService).OnShow -= ShowModal;
            ((ModalService)ModalService).CloseModal -= CloseModal;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private async void ShowModal(string title, RenderFragment content, ModalParameters parameters, ModalOptions options)
        {
            Title = title;
            Content = content;
            Parameters = parameters;
            IsVisible = true;
            await InvokeAsync(StateHasChanged);
        }

        private async void CloseModal()
        {
            Title = string.Empty;
            Content = null;
            ComponentStyle = string.Empty;
            IsVisible = false;
            await InvokeAsync(StateHasChanged);
        }



        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ((ModalService)ModalService).OnShow -= ShowModal;
                ((ModalService)ModalService).CloseModal -= CloseModal;

            }
        }

        private void HandleBackgroundClieck()
        {
            if (ComponentdisableBackgroundCancel) return;
            ModalService.Cancel();
        }

        private void SetModalOptions(ModalOptions options)
        {

            ComponentHideHeader = HideHeader;
            if(options.HideHeader.HasValue)
                ComponentHideHeader = options.HideHeader.Value;

            ComponentHideCloseButton = HideCloseButton;
            if(options.HideCloseButton.HasValue)
                ComponentHideCloseButton= options.HideCloseButton.Value;

            ComponentdisableBackgroundCancel = DisableBackgroundCancel;
            if(options.DisableBackgroundCancel.HasValue)
                ComponentdisableBackgroundCancel= options.DisableBackgroundCancel.Value;

            ComponentPosition = string.IsNullOrWhiteSpace(options.Position) ? Position : options.Position;
            if (string.IsNullOrWhiteSpace(ComponentPosition))
                ComponentPosition = _defaultPosition;

            ComponentStyle = string.IsNullOrWhiteSpace(options?.Style) ? Style : options.Style;
            if(string.IsNullOrWhiteSpace(ComponentStyle))
                ComponentStyle = _defaultStyle;
        }

        public void SetTitle(string title)
        {
            Title = title;
            StateHasChanged();
        }
    }
}
