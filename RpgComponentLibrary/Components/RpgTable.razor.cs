using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace RpgComponentLibrary.Components
{
    public partial class RpgTable<TItem>
    {
        [Parameter]
        public RenderFragment TableHeader { get; set; }

        [Parameter]
        public RenderFragment<TItem> RowTemplate { get; set; }

        [Parameter]
        public IReadOnlyList<TItem> Items { get; set; }
        [Parameter]
        public EventCallback<TItem> OnItemSelected { get; set; }
        [Parameter]
        public EventCallback<TItem> OnDblClickRow { get; set; }
        [Parameter]
        public bool ShowSelect { get; set; }

        [Parameter]
        public Frame TableFrame { get; set; } = Frame.FramedGolden;
        [Parameter]
        public int PixelWidth { get; set; }
        [Parameter]
        public int PercentWidth { get; set; }

        private string Width => PercentWidth > 0 ? $"width: {PercentWidth}%" : PixelWidth > 0 ? $"width: {PixelWidth}px" : "";
    }
}
