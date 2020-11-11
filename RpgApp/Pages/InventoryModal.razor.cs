using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TurnBasedRpg.Services;
using TurnBasedRpg.Types;
using TurnBasedRpg.Types.Enums;

namespace TurnBasedRpg.Pages
{
    public partial class InventoryModal
    {
        [Inject]
        public InventoryService InventoryService { get; set; }
        List<Equipment> playerInventory = new List<Equipment>();

        protected override async Task OnInitializedAsync()
        {
            playerInventory = await InventoryService.GetUnder30GpInventory();
            foreach (var item in playerInventory)
            {
                if (item.Effects == null)
                {
                    item.Effects = new List<Effect>();
                    item.Effects.Add(new Effect() {Type = EffectType.Status, Value = "none"});
                }
            }
            
        }
    }
}
