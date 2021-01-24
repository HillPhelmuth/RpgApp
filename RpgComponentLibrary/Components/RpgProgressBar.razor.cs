using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RpgComponentLibrary.Services;

namespace RpgComponentLibrary.Components
{
    public partial class RpgProgressBar
    {
        [Inject]
        private IJSRuntime jsRuntime { get; set; }
        private GuiInterop gui => new(jsRuntime);
        private ElementReference _barRef;
        private double _convertedVal;
        private string fillWidth;
        [Parameter]
        public string Label { get; set; }
        [Parameter]
        public int MaxValue { get; set; }
        [Parameter]
        public int MinValue { get; set; }
        [Parameter]
        public ProgressBarColor ProgressBarColor { get; set; }
        [Parameter]
        public string ElementId { get; set; }

        [Parameter]
        public double DataValue { get; set; }

        protected override void OnParametersSet()
        {
            _convertedVal = DataValue <= MinValue ? 0 : DataValue >= MaxValue ? 1.0 : DataValue / MaxValue;
            fillWidth = FormatToPercentWidth(_convertedVal);
            base.OnParametersSet();
        }

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    //await UpdateData();
        //    await base.OnAfterRenderAsync(firstRender);
        //}

        private async Task UpdateData()
        {
            await gui.UpdateProgressBar(_barRef, _convertedVal);
            Console.WriteLine($"Progress Bar Updated to {_convertedVal}");
            //await InvokeAsync(StateHasChanged);
        }

        private string FormatToPercentWidth(double num)
        {
            return num.ToString("P0", new NumberFormatInfo { PercentPositivePattern = 1});
        }
    }

    public enum ProgressBarColor
    {
        Blue, Green, Red
    }
}
