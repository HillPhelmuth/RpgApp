using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace RpgComponentLibrary.Components
{
    public partial class RpgItemsMenu<TItem>
    {
        private List<MenuItemTemplate> _indexImageItems = new();
        private string _draggable = "";
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
        public List<TItem> Items { get; set; }
        [Parameter]
        public Func<TItem, string> ImageMapperFunc { get; set; }

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
            _width = $"{(MenuColumns * 64) + 35}px";
            _draggable = IsDraggable ? "rpgui-draggable" : "";
            var imagesWithIndex = new List<MenuItemTemplate>();
            var i = 0;
            foreach (var item in Items)
            {
                imagesWithIndex.Add(new MenuItemTemplate(i, ImageMapperFunc(item), false, item));
            }
            _indexImageItems = imagesWithIndex;
            return base.OnParametersSetAsync();
        }
        private void OpenTemplate(MenuItemTemplate itemTemplate)
        {
            foreach (var item in _indexImageItems)
            {
                item.IsOpen = false;
            }

            itemTemplate.IsOpen = !itemTemplate.IsOpen;
        }
    }

    public enum Frame
    {
        Framed, FramedGolden, FramedGolden2, FramedGrey
    }
}
