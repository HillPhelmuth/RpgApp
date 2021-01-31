using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using RpgComponentLibrary.Services;

namespace RpgComponentLibrary.Components
{
    public partial class RpgSlider
    {
        private string _thumbLocation;
        private bool _isDragging;
        private double _trackWidth;
        private double _oldDataValue;
        private bool _hasDataChanged;
        [Inject]
        private IJSRuntime jsRuntime { get; set; }
        private GuiInterop gui => new(jsRuntime);
        private ElementReference _trackRef;
        private double _dataValue;

        [Parameter]
        public string Label { get; set; }
        [Parameter]
        public double MaxValue { get; set; }
        [Parameter]
        public double MinValue { get; set; }
        [Parameter]
        public bool IsGolden { get; set; }
        [Parameter]
        public string ElementId { get; set; }

        [Parameter]
        public double DataValue
        {
            get => _dataValue;
            set
            {
                _dataValue = value;
                _hasDataChanged = true;
            }
        }

        [Parameter]
        public EventCallback<double> DataValueChanged { get; set; }
        
        protected override Task OnParametersSetAsync()
        {
            if (_hasDataChanged && _trackWidth > 0)
            {
                var totalValue = Math.Abs(MinValue) + Math.Abs(MaxValue);
                var dataPercent = DataValue / totalValue;
                var locationValue = _trackWidth * dataPercent;
                _thumbLocation = $"{locationValue}px";
                _hasDataChanged = false;
            }
            return base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _oldDataValue = DataValue;
                _hasDataChanged = true;
                _trackWidth = await gui.GetWidth(_trackRef);
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private void SlideClick(MouseEventArgs e)
        {
            var locationValue = e.OffsetX;
            Console.WriteLine($"click: {locationValue}");
           SetThumbLocationData(locationValue);
        }

        private void SlideMove(MouseEventArgs e)
        {
            if (!_isDragging) return;
            var locationValue = e.OffsetX;
            Console.WriteLine($"move: {locationValue}");
            SetThumbLocationData(locationValue);
        }

        private void SetThumbLocationData(double locationValue)
        {
            if (locationValue > _trackWidth)
                locationValue = _trackWidth;
            _thumbLocation = $"{locationValue}px";
            var locationPercent = locationValue / _trackWidth;
            var totalValue = Math.Abs(MinValue) + Math.Abs(MaxValue);
            var adjustedValue = totalValue * locationPercent;
            DataValue = MinValue + adjustedValue;
            DataValueChanged.InvokeAsync(DataValue);
        }

        private void MouseDown(MouseEventArgs e) => _isDragging = true;

        private void MouseUpOrOut(MouseEventArgs e) => _isDragging = false;
    }
}
