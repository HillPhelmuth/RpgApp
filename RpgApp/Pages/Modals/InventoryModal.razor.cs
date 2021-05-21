using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using RpgApp.Shared;
using RpgApp.Shared.Types;

namespace RpgApp.Client.Pages.Modals
{
    public partial class InventoryModal
    {
        List<Equipment> playerInventory = new List<Equipment>();
        [Inject]
        AppStateManager AppState { get; set; }
        [Inject]
        private HttpClient HttpClient { get; set; }

        protected override Task OnInitializedAsync()
        {
            playerInventory = AppState.CurrentPlayer.Inventory;
            return base.OnInitializedAsync();
        }
    }
}
