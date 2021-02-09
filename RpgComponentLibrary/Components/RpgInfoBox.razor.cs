using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace RpgComponentLibrary.Components
{
    public partial class RpgInfoBox<TObject>
    {
        public class DisplayProperty
        {
            public DisplayProperty(int order, string propertyName)
            {
                Order = order;
                PropertyName = propertyName;
                
            }
            public int Order { get; set; }
            public string PropertyName { get; set; }
            public string PropertyValue { get; set; }
        }
        [Parameter]
        public Dictionary<int, string> DisplayPropertiesOrder { get; set; }
        
        private List<DisplayProperty> _displayItems = new();
        private Dictionary<string, List<DisplayProperty>> _displaySubItems = new();
        //[Parameter]
        //public Dictionary<string, TSubObject> NamedSubTypeValues { get; set; }
        //[Parameter]
        //public IReadOnlyList<TSubObject> SubTypeValueList { get; set; }
        [Parameter]
        public TObject DisplayObject { get; set; }
        //[Parameter]
        //public TSubObject SingleSubTypeValue { get; set; }
        [Parameter]
        public RenderFragment HeaderTemplate { get; set; }
        //[Parameter]
        //public string[] SubObjects { get; set; }
        //[Parameter]
        //public List<DisplayProperty> DisplayProperties { get; set; }
        [Parameter]
        public Frame Frame { get; set; }

        protected override Task OnParametersSetAsync()
        {
            _displayItems = DisplayPropertiesOrder.Select(x => new DisplayProperty(x.Key, x.Value)
                {PropertyValue = DisplayObject.GetStringPropValue(x.Value)}).ToList();
            int i = 1;
            //foreach (var namedValue in NamedSubTypeValues)
            //{
            //    var asDict = namedValue.Value.ToDictionary();
            //    var displayProps = asDict
            //        .Select(x => new DisplayProperty(i, x.Key) {PropertyValue = x.Value.ToString()}).ToList();
            //    _displaySubItems.Add(namedValue.Key, displayProps);
            //}
            return base.OnParametersSetAsync();
        }
    }
}
