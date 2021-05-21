using Blazor.ModalDialog;
using Microsoft.AspNetCore.Components;

namespace RpgApp.Client.Pages.Modals
{
    public partial class MessageBoxModal
    {
        [Inject]
        public IModalDialogService ModalService { get; set; }
        [Parameter] 
        public string HtmlMarkupContent { get; set; }
        [Parameter]
        public string PreFormattedTextContent { get; set; }
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private void Close()
        {
            ModalService.Close(true);
        }
    }
}
