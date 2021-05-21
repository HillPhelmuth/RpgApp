using System;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazor.ModalDialog;
using Microsoft.AspNetCore.Components;
using RpgApp.Shared;
using RpgApp.Shared.Types;

namespace RpgApp.Client.Pages.Modals
{
    public partial class CharacterSelectModal:IDisposable
    {
        [Inject]
        private AppStateManager AppState { get; set; }
        [Inject]
        public IModalDialogService ModalService { get; set; }
        [Inject]
        public HttpClient Http { get; set; }

        protected override Task OnInitializedAsync()
        {
            AppState.PropertyChanged += UpdateState;
            return base.OnInitializedAsync();
        }

        private void HandlePlayerSelect(Player player)
        {
            player.Refresh();
            AppState.UpdateCurrentPlayer(player);
            ModalService.Close(true);

        }
        protected void UpdateState(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(AppState.UserData))
                return;
            Console.WriteLine($"{e.PropertyName} change handled by {nameof(CharacterSelectModal)}");
            InvokeAsync(StateHasChanged);
        }

        public async Task Refresh()
        {
            AppState.UserData = await Http.GetFromJsonAsync<UserData>($"{AppConstants.ApiUrl}/GetUserPlayers/{AppState.UserId}");
            await InvokeAsync(StateHasChanged);
        }
        public void Dispose()
        {
            AppState.PropertyChanged -= UpdateState;
        }
    }
}
