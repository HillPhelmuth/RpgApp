using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace RpgComponentLibrary.Components
{
    public partial class RpgTextInput
    {
        [Parameter]
        public string Label { get; set; }
        [Parameter]
        public string InputValue { get; set; }
        [Parameter]
        public EventCallback<string> InputValueChanged { get; set; }
        [Parameter]
        public bool IsTextArea { get; set; }

        [Parameter] 
        public int MaxLength { get; set; } = 1000;
        [Parameter]
        public string Placeholder { get; set; }
        [Parameter]
        public bool IsReadOnly { get; set; }

        private void HandleInput(ChangeEventArgs args)
        {
            InputValue = args.Value?.ToString();
            InputValueChanged.InvokeAsync(InputValue);
        }

    }

}
