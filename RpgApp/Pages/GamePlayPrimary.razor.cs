using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TurnBasedRpg.Services;
using TurnBasedRpg.Shared;
using TurnBasedRpg.Types;
using TurnBasedRpg.Types.Enums;
using Blazor.ModalDialog;

namespace TurnBasedRpg.Pages
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

        protected override async Task OnInitializedAsync()
        {
            altDisplay = string.IsNullOrEmpty(TabDemoParameter) ? "Navigation style presentation" : TabDemoParameter;
            await UpdateState();
            // Listens for OnChange event from AppStateManager and triggers UpdateState each time
            AppStateManager.OnChange += UpdateState;
        }
        public async Task CreateNewPlayer(ClassType classType)
        {
            var create = new CreateCharacter();
            CurrentPlayer = await create.CreateNewCharacter(classType);
            await AppStateManager.UpdateCurrentPlayer(CurrentPlayer);
            CurrentPlayer.Name = name;
            IsPlayerCreated = true;
            StateHasChanged();
        }
        
        public async Task CreateCharacter()
        {
            ModalDialogResult result = await ModalService.ShowDialogAsync<CharacterCreationModal>("Create Character");
            StateHasChanged();
        }

        // UnSubscribes from the OnChange event when component is not currently in use
        public void Dispose() => AppStateManager.OnChange -= UpdateState;
        #endregion

    }
}
