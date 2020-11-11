using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop;
using TurnBasedRpg.StateManager;
using TurnBasedRpg.Types;
using Blazor.ModalDialog;

namespace TurnBasedRpg.Pages
{
    public partial class MenuModal
    {
        [Inject]
        public AppStateManager AppStateManager { get; set; }
        [Inject]
        public IModalDialogService ModalService { get; set; }
        protected Player CurrentPlayer { get; set; }

        public async Task ShowStats()
        {
            ModalDialogResult result = await ModalService.ShowDialogAsync<StatsModal>("Character Sheet");
            StateHasChanged();
        }
        public async Task ShowInventory()
        {
            ModalDialogResult result = await ModalService.ShowDialogAsync<InventoryModal>("Shop");
            StateHasChanged();
        }
    }
}
