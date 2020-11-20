using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Blazor.ModalDialog;
using Microsoft.AspNetCore.Components;
using RpgApp.Shared.StateManager;

namespace RpgApp.Client.Pages.Layouts
{
    public partial class PlayerLocation : IDisposable
    {
        [CascadingParameter(Name = "AppState")]
        public AppStateManager AppState { get; set; }
        [Inject]
        protected IModalDialogService ModalDialogService { get; set; }
        [Parameter]
        public int Col { get; set; }
        [Parameter]
        public int Row { get; set; }

        protected (int, int) PlayerLoc { get; set; }

        [Parameter]
        public EventCallback<(int, int)> PlayerLocChanged { get; set; }
        private string cssLocation = "";
        private bool isOccupied;

        protected override Task OnInitializedAsync()
        {

            AppState.PropertyChanged += UpdateLocation;
            return base.OnInitializedAsync();
        }

        private async void UpdateLocation(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "PlayerLocation") return;

            PlayerLoc = AppState.PlayerLocation;

            (int row, int col) = PlayerLoc;

            if (row == Row && col == Col)
            {
                cssLocation = "located";
                isOccupied = true;
                await PlayerLocChanged.InvokeAsync(PlayerLoc);
            }
            else
            {
                cssLocation = "";
                isOccupied = false;
            }

            await InvokeAsync(StateHasChanged);
        }

        private async Task TryModifyModal()
        {
            var result = await ModalDialogService.ShowDialogAsync<StyleTester>("this");
            if (result.Success)
                ModalDialogService.Close(true);
        }

        public void Dispose()
        {
            AppState.PropertyChanged -= UpdateLocation;
        }
    }
}
