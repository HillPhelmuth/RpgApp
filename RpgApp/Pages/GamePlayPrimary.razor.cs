using System;
using System.ComponentModel;
using System.Text.Json;
using System.Threading.Tasks;
using Blazor.ModalDialog;
using Microsoft.AspNetCore.Components;
using RpgApp.Client.Pages.Modals;
using RpgApp.Client.Shared;
using RpgApp.Shared;
using RpgApp.Shared.Types;
using RpgApp.Shared.Types.Enums;

namespace RpgApp.Client.Pages
{
    public partial class GamePlayPrimary : IDisposable
    {
       
        #region Fields/Properties
        [Inject]
        public IModalDialogService ModalService { get; set; }
        [Inject]
        public AppStateManager AppState { get; set; }
        [Inject]
        private ClientDataService ClientDataService { get; set; }
       
        public Player CurrentPlayer { get; set; }
        private bool show;
        private string name;
        #endregion

        #region Methods

        protected override Task OnInitializedAsync()
        {
            AppState.PropertyChanged += UpdateState;
            return base.OnInitializedAsync();
        }
        public async Task CreateNewCharacter()
        {
            var options = new ModalDialogOptions
            {
                Style = ModalStyles.Framed(ModalSize.ExtraLarge)
            };
            var result = await ModalService.ShowDialogAsync<CharacterCreationModal>("Create Character", options);
            if (result.Success)
            {
                show = false;
                await ClientDataService.AddOrUpdatePlayer(AppState.CurrentPlayer);
            }

            StateHasChanged();
        }

        private void Selected(Player player) => name = player.Name;
        private async Task SelectUserCharacter()
        {
            Console.WriteLine($"UserData: {JsonSerializer.Serialize(AppState.UserData)}");
            if (AppState.UserData == null) return;
            var options = new ModalDialogOptions
            {
                Style = ModalStyles.Framed(ModalSize.Medium)
            };
            var result = await ModalService.ShowDialogAsync<CharacterSelectModal>("Double Click to Select Character", options);
            if (result.Success) show = false;
            StateHasChanged();
        }
        protected void UpdateState(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(AppState.UserId) && e.PropertyName != nameof(AppState.UserData))
                return;
            Console.WriteLine($"{e.PropertyName} change handled by {nameof(GamePlayPrimary)}");
            InvokeAsync(StateHasChanged);
        }

        public void Dispose() => AppState.PropertyChanged -= UpdateState;
        #endregion

    }
}
