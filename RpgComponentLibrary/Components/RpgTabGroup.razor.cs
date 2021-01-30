using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace RpgComponentLibrary.Components
{
    public partial class RpgTabGroup
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        public RpgTab ActivePage { get; set; }
        private List<RpgTab> _pages = new();
        internal void AddPage(RpgTab tabPage)
        {
            _pages.Add(tabPage);
            if (_pages.Count == 1)
                ActivePage = tabPage;
            StateHasChanged();
        }
        private string GetButtonClass(RpgTab page) => page == ActivePage ? "" : "silver";

        private void ActivatePage(RpgTab page) => ActivePage = page;
    }
}
