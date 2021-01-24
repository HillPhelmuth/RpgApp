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
