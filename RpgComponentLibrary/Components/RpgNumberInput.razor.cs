using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace RpgComponentLibrary.Components
{
    public partial class RpgNumberInput<TNum>
    {

        [Parameter] 
        public string Label { get; set; } = "";
        [Parameter]
        public TNum? NumberValue { get; set; }
        [Parameter]
        public EventCallback<TNum> NumberValueChanged { get; set; }
#nullable enable
        [Parameter]
        public TNum? MinValue { get; set; }

        [Parameter]
        public TNum? MaxValue { get; set; }
#nullable disable
        [Parameter]
        public double? Step { get; set; } = GetDefaultStep();
        [Parameter]
        public bool IsReadOnly { get; set; }

        protected override Task OnInitializedAsync()
        {
            Console.WriteLine($"MaxValue on Init: {MaxValue}");
            return base.OnInitializedAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            var max = GetDefaultMax(Type.GetTypeCode(typeof(TNum)));
            var maxValue = int.TryParse(MaxValue.ToString(), out var val) ? val : 0;
            if (maxValue == 0) 
                MaxValue = max;
            return base.OnParametersSetAsync();
        }
        //private void HandleInput(ChangeEventArgs args)
        //{
        //    NumberValueChanged.InvokeAsync(NumberValue);
        //}
        private static double GetDefaultStep()
        {
            return Type.GetTypeCode(typeof(TNum)) switch
            {
                TypeCode.Int32 => 1,
                TypeCode.Decimal => .1,
                TypeCode.Double => .01,
                _ => 1
            };
        }

        private void HandleInput(ChangeEventArgs args)
        {
            var input = args.Value?.ToString();
            var type = Type.GetTypeCode(typeof(TNum));
            object value = type switch
            {
                TypeCode.Int32 => int.TryParse(input, out var val) ? val : 0,
                TypeCode.Decimal => decimal.TryParse(input, out var val) ? val : 0,
                TypeCode.Double => double.TryParse(input, out var val) ? val : 0,
                _ => NumberValue
            };
            try
            {
                NumberValue = (TNum) Convert.ChangeType(value, typeof(TNum));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            NumberValueChanged.InvokeAsync(NumberValue);
        }

        private static TNum GetDefaultMax(TypeCode type)
        {
            object maxValue = type switch
            {
                TypeCode.Int32 => int.MaxValue,
                TypeCode.Decimal => decimal.MaxValue,
                TypeCode.Double => double.MaxValue,
                _ => int.MaxValue
            };
            Console.WriteLine($"maxVal {maxValue}");
            try
            {
                return (TNum) Convert.ChangeType(maxValue, typeof(TNum));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default;
            }
           
        }

    }

}
