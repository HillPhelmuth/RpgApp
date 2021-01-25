using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace RpgComponentLibrary.Components
{
    public partial class RpgButton
    {
        private string _rpgCssClass = "";

        [Parameter]
        public string Label { get; set; }
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }
        [Parameter]
        public RpgButtonStyle RpgButtonStyle { get; set; }
        [Parameter]
        public EventCallback<MouseEventArgs> OnMouseDown { get; set; }
        [Parameter]
        public EventCallback<MouseEventArgs> OnMouseUp { get; set; }
        [Parameter]
        public EventCallback<MouseEventArgs> OnDoubleClick { get; set; }
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool CanRender { get; set; } = true;

        protected override bool ShouldRender()
        {
            return CanRender;
        }

        protected override Task OnParametersSetAsync()
        {
            _rpgCssClass = RpgButtonStyle switch
            {
                RpgButtonStyle.Golden => "golden",
                RpgButtonStyle.Silver => "silver",
                _ => ""
            };
            return base.OnParametersSetAsync();
        }
    }

    public enum RpgButtonStyle
    {
        Standard,
        Golden,
        Silver
    }
}
