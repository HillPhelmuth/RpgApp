using System.Threading.Tasks;
using Blazor.ModalDialog;
using Microsoft.AspNetCore.Components;
using RpgApp.Shared;
using RpgApp.Shared.Types;
using RpgApp.Shared.Types.Enums;

namespace RpgApp.Client.Pages.Modals
{
    public partial class MenuModal
    {
        [CascadingParameter(Name = "AppState")]
        public AppStateManager AppState { get; set; }
        [Inject]
        public IModalDialogService ModalService { get; set; }
        protected Player CurrentPlayer { get; set; }
        ModalDialogOptions DialogOptions = new ModalDialogOptions()
        {
            Style = ModalStyles.Framed(ModalSize.Medium)
        };

        public async Task ShowStats()
        {
            ModalDialogResult result = await ModalService.ShowDialogAsync<StatsModal>("Character Sheet", DialogOptions);
            StateHasChanged();
        }
        public async Task ShowShop()
        {
            ModalDialogResult result = await ModalService.ShowDialogAsync<ShopModal>("Shop", DialogOptions);
        }
        public async Task ShowInventory()
        {
            ModalDialogResult result = await ModalService.ShowDialogAsync<InventoryModal>("Inventory", DialogOptions);
            StateHasChanged();
        }
    }
}
