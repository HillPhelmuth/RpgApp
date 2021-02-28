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
using RpgApp.Shared.Types.PlayerExtensions;
using RpgComponentLibrary.Services;

namespace RpgApp.Client.Pages.Modals
{
    public partial class ShopModal
    {
        private List<Equipment> shopInventory = new List<Equipment>();
        private List<KeyValuePair<string, Equipment>> _imagesEquipPairs = new();
        private List<KeyValuePair<string, Equipment>> AddImages(List<Equipment> equipment)
        {
            int id = 0;
            List<KeyValuePair<string, Equipment>> ImageItems = new List<KeyValuePair<string, Equipment>>();
            foreach (var item in equipment)
            {
                if (item.Name == "Dagger")
                {
                    id = 9;
                }
                if (item.Name == "Short sword")
                {
                    id = 1;
                }
                else { id = 5; }
                ImageItems.Add(TestImageAdd(item, id));
            }
            return ImageItems.ToList();
        }
        [Inject]
        public AppStateManager AppState { get; set; }
        [Inject]
        private HttpClient HttpClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var apiResponse = await HttpClient.GetFromJsonAsync<List<Equipment>>($"{AppConstants.ApiUrl}/GetSomeEquipment?goldMax={30}");
            shopInventory = apiResponse;
            _imagesEquipPairs = AddImages(shopInventory);
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
        private KeyValuePair<string, Equipment> TestImageAdd(Equipment equipment, int id)
        {
            return new KeyValuePair<string, Equipment>(ImageData.IndexedImages[id], equipment);
        }
    }
}
