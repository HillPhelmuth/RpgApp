using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace RpgComponentLibrary.Components
{
    public partial class RpgCheckbox
    {
        private string _cssClass;
        [Parameter]
        public bool IsChecked { get; set; }
        [Parameter]
        public bool IsDisabled { get; set; }
        [Parameter]
        public string Label { get; set; }
        [Parameter]
        public CheckboxStyle CheckboxStyle { get; set; }
        [Parameter]
        public EventCallback<bool> IsCheckedChanged { get; set; }

        protected override Task OnParametersSetAsync()
        {
            _cssClass = CheckboxStyle == CheckboxStyle.Golden ? "golden" : "";
            return base.OnParametersSetAsync();
        }

        private void HandleClick(MouseEventArgs args)
        {
            if (IsDisabled) return;
            IsChecked = !IsChecked;
            IsCheckedChanged.InvokeAsync(IsChecked);
        }
    }

    public enum CheckboxStyle
    {
        Standard,
        Golden
    }
}
