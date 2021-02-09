using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace RpgComponentLibrary.Components
{
    public partial class RpgContainer
    {
        [Parameter]
        public double Width { get; set; }
        [Parameter]
        public double MinWidth { get; set; }
        [Parameter]
        public double MinHeight { get; set; }
        [Parameter]
        public Frame Frame { get; set; }
        [Parameter]
        public RenderFragment ChildContent { get; set; }

       

        protected override Task OnParametersSetAsync()
        {
          
            return base.OnParametersSetAsync();
        }
    }
}
