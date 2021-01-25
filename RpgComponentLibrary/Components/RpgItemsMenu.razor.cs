using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace RpgComponentLibrary.Components
{
    public partial class RpgItemsMenu<TItem>
    {
        private List<MenuItemTemplate> _indexImageItems = new();
        private int _spillOver;
        private string _draggale = "";
        private string _width;
        private int _menuColumns = 3;

        private class MenuItemTemplate
        {
            public MenuItemTemplate(int index, string image, bool isOpen, TItem item)
            {
                Index = index;
                Image = image;
                IsOpen = isOpen;
                Item = item;
            }
            public int Index { get; }
            public string Image { get; }
            public bool IsOpen { get; set; }
            public TItem Item { get; }
        }

        [Parameter]
        public RenderFragment MenuHeader { get; set; }

        [Parameter]
        public RenderFragment<TItem> ActionTemplate { get; set; }
        [Parameter]
        public RenderFragment<TItem> ToolTipTemplate { get; set; }
        [Parameter]
        public IReadOnlyList<KeyValuePair<string, TItem>> ImageItemActions { get; set; }

        [Parameter]
        public int MenuColumns
        {
            get
            {
                return _menuColumns switch
                {
                    <= 3 => 3,
                    >= 7 => 7,
                    _ => _menuColumns
                };
            }
            set => _menuColumns = value;
        }

        [Parameter]
        public Frame MenuFrame { get; set; }
        [Parameter]
        public bool IsDraggable { get; set; }


        protected override Task OnParametersSetAsync()
        {
            _width = $"{(MenuColumns * 64)+35}px";
            _draggale = IsDraggable ? "rpgui-draggable" : "";
            var imagesWithIndex = new List<MenuItemTemplate>();
            var i = 0;
            foreach (var imageItem in ImageItemActions)
            {
                imagesWithIndex.Add(new MenuItemTemplate(i, imageItem.Key, false, imageItem.Value));
                i++;
            }
            //_spillOver = 0;
            //while (i % MenuColumns != 0)
            //{
            //    _spillOver++;
            //    i++;
            //}

            _indexImageItems = imagesWithIndex;
            return base.OnParametersSetAsync();
        }
    }

    public enum Frame
    {
        Framed, FramedGolden, FramedGolden2, FramedGrey
    }
}
