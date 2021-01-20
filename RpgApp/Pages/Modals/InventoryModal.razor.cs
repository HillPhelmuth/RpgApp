using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using RpgApp.Shared;
using RpgApp.Shared.Types;
using RpgApp.Shared.Types.Enums;

namespace RpgApp.Client.Pages.Modals
{
    public partial class InventoryModal
    {
        List<Equipment> playerInventory = new List<Equipment>();
        [Inject]
        private HttpClient HttpClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Expression<Func<Equipment, bool>> equipFilter = equipment => equipment.GoldCost <= 30;
            var apiResponse = await HttpClient.PostAsJsonAsync($"{AppConstants.ApiUrl}/GetEquipment", equipFilter);
            var equipmentJson = await apiResponse.Content.ReadAsStringAsync();
            playerInventory = JsonSerializer.Deserialize<List<Equipment>>(equipmentJson) ?? new List<Equipment>();
            foreach (var item in playerInventory.Where(item => item.Effects == null))
            {
                item.Effects = new List<Effect> { new Effect { Type = EffectType.Status, Value = "none" } };
            }

        }
    }
}
