using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
//using Newtonsoft.Json;
using RpgComponentLibrary.Services;

namespace RpgComponentLibrary.Components
{
    public partial class RpgSelectDropdown<TItem>
    {
        private bool showList;
        private string DisplaySelectedValue => SelectedValue?.GetStringPropValue(DisplayPropertyName);
        [Inject]
        private IJSRuntime jsRuntime { get; set; }
        private GuiInterop gui => new(jsRuntime);
        private ElementReference _selectHeader;
        private string _menuWidth;
        [Parameter]
        public string Label { get; set; }
        [Parameter]
        public IReadOnlyList<TItem> OptionsList { get; set; }
       
        [Parameter]
        public TItem SelectedValue { get; set; }

        [Parameter] 
        public string DisplayPropertyName { get; set; } = "";
        [Parameter]
        public EventCallback<TItem> OnSelectItem { get; set; }
        [Parameter]
        public EventCallback<TItem> SelectedValueChanged { get; set; }
        [Parameter] 
        public int Width { get; set; } = 100;

        protected override Task OnParametersSetAsync()
        {
            SelectedValue ??= OptionsList.FirstOrDefault();
            return base.OnParametersSetAsync();
        }

        private async Task SelectItem(TItem item)
        {
            showList = false;
            SelectedValue = item;
            await OnSelectItem.InvokeAsync(item);
            await SelectedValueChanged.InvokeAsync(item);
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var head = await gui.GetWidth(_selectHeader);
                _menuWidth = $"{head * .9}px";

            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
    public static class Extentions
    {
        public static string GetStringPropValue<TItem>(this TItem item, string propName)
        {
            if (string.IsNullOrWhiteSpace(propName))
                return item.ToString();
            var property = typeof(TItem).GetProperty(propName);
            if (property == null)
                return $"Property {propName} not found in type {nameof(item)}";
            return property.GetValue(item)?.ToString();
        }
        public static Dictionary<string, object> ToDictionary(this object obj)
        {
           // var json = JsonConvert.SerializeObject(obj);
           var json = JsonSerializer.Serialize(obj);
            var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            return dictionary;
        }
        private static string FormatToPercentWidth(this double num)
        {
            return num.ToString("P0", new NumberFormatInfo { PercentPositivePattern = 1 });
        }
    }

}
