using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace RpgComponentLibrary.Animations
{
    public partial class RpgMonsterGifs
    {
        
        private bool isAttacking;
        [Parameter]
        public string MonsterGifSet { get; set; }
        [Parameter]
        public bool IsAttackGif { get; set; }
        [Parameter]
        public EventCallback<bool> IsAttackGifChanged { get; set; }
        [Parameter]
        public int Height { get; set; }
        [Parameter]
        public int Width { get; set; }
        

        protected override async Task OnParametersSetAsync()
        {
            var random = new Random();
            if (IsAttackGif && isAttacking)
                await UpdateGif();
            if (IsAttackGif)
                isAttacking = true;
            MonsterGifSet ??= $"Monster{random.Next(1,4)}";
            await base.OnParametersSetAsync();
        }

        private async Task UpdateGif()
        {
            await Task.Delay(1000);
            IsAttackGif = false;
            isAttacking = false;
            await IsAttackGifChanged.InvokeAsync(IsAttackGif);
        }

    }
}
