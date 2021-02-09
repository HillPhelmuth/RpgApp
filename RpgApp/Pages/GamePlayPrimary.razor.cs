using System;
using System.Threading.Tasks;
using Blazor.ModalDialog;
using Microsoft.AspNetCore.Components;
using RpgApp.Client.Pages.Modals;
using RpgApp.Client.Shared;
using RpgApp.Shared.Types;
using RpgApp.Shared.Types.Enums;

namespace RpgApp.Client.Pages
{
    public class GamePlayPrimaryModel : RpgComponentBase, IDisposable
    {
        #region TempRegion
        [CascadingParameter]
        public string TabDemoParameter { get; set; }
        protected string altDisplay { get; set; }

        #endregion
        #region Fields/Properties
        [Inject]
        public IModalDialogService ModalService { get; set; }
        protected string at = "@";
        protected Player Player { get; set; }
        protected ClassType CharClass { get; set; }
        protected string name;
        protected bool IsPlayerCreated;

        #endregion

        #region Methods

        protected override Task OnInitializedAsync()
        {
            altDisplay = string.IsNullOrEmpty(TabDemoParameter) ? "Navigation style presentation" : TabDemoParameter;
            //await UpdateState();
            // Listens for OnChange event from AppStateManager and triggers UpdateState each time
            AppState.PropertyChanged += UpdateState;
            return base.OnInitializedAsync();
        }
        //public async Task CreateNewPlayer(ClassType classType)
        //{
        //    CurrentPlayer = await CreateCharacter.CreateNewCharacter(classType);
        //    AppState.UpdateCurrentPlayer(CurrentPlayer);
        //    CurrentPlayer.Name = name;
        //    IsPlayerCreated = true;
        //    StateHasChanged();
        //}

        public async Task CreateNewCharacter()
        {
            var options = new ModalDialogOptions()
            {
                Style = ModalSyles.Framed(ModalSize.ExtraLarge)
            };
            var result = await ModalService.ShowDialogAsync<CharacterCreationModal>("Create Character", options);
            StateHasChanged();
        }

        // UnSubscribes from the OnChange event when component is not currently in use
        public void Dispose() => AppState.PropertyChanged -= UpdateState;
        #endregion

    }
}
