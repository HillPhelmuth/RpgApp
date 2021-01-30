using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace RpgComponentLibrary.Components
{
    public partial class RpgSelectDropdown<TItem>
    {
        private bool showList;
        private string DisplaySelectedValue => SelectedValue?.GetStringPropValue(DisplayPropertyName);
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
    }

}
