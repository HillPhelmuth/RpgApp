//using Microsoft.AspNetCore.Components;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using TurnBasedRpg.Services;
//using TurnBasedRpg.Types;
//using TurnBasedRpg.Types.Enums;

//namespace TurnBasedRpg.Pages
//{
//    public partial class ShopModal
//    {
//        [Inject]
//        public InventoryService ShopService { get; set; }
//        List<Equipment> shopEquipment = new List<Equipment>();

//        protected override async Task OnInitializedAsync()
//        {
//            shopEquipment = await ShopService.GetUnder30GpInventory();
//            foreach (var equip in shopEquipment)
//            {
//                if (equip.Effects == null)
//                {
//                    equip.Effects = new List<Effect>();
//                    equip.Effects.Add(new Effect() { Type = EffectType.Status, Value = "none" });
//                }
//            }

//        }
//    }
//}
