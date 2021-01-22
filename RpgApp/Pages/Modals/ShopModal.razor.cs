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
    public partial class ShopModal
    {
        List<Equipment> shopInventory = new List<Equipment>();
        [Inject]
        public AppStateManager AppState { get; set; }
        [Inject]
        private HttpClient HttpClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var apiResponse = await HttpClient.GetFromJsonAsync<List<Equipment>>($"{AppConstants.ApiUrl}/GetSomeEquipment?goldMax={30}");
            shopInventory = apiResponse;
            foreach (var item in shopInventory.Where(item => item.Effects == null))
            {
                item.Effects = new List<Effect> { new Effect { Type = EffectType.Status, Value = "none" } };
            }
        }
        public async Task BuyEquipment(Equipment equipment)
        {
            AppState.CurrentPlayer.Inventory.Add(equipment);
            await HttpClient.PostAsJsonAsync($"{AppConstants.ApiUrl}/UpdateOrAddPlayer", AppState.CurrentPlayer);
        }
    }
}
