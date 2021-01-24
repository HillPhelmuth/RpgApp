using System.Threading.Tasks;
using Blazor.ModalDialog;
using Microsoft.AspNetCore.Components;
using RpgApp.Shared;
using RpgApp.Shared.Types;

namespace RpgApp.Client.Pages.Modals
{
    public partial class MenuModal
    {
        [CascadingParameter(Name = "AppState")]
        public AppStateManager AppState { get; set; }
        [Inject]
        public IModalDialogService ModalService { get; set; }
        protected Player CurrentPlayer { get; set; }

        public async Task ShowStats()
        {
            ModalDialogResult result = await ModalService.ShowDialogAsync<StatsModal>("Character Sheet");
            StateHasChanged();
        }
        public async Task ShowShop()
        {
            ModalDialogResult result = await ModalService.ShowDialogAsync<ShopModal>("Shop");
        }
        public async Task ShowInventory()
        {
            ModalDialogResult result = await ModalService.ShowDialogAsync<InventoryModal>("Inventory");
            StateHasChanged();
        }
    }
}
