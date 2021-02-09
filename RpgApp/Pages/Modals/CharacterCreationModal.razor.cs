using System.Threading.Tasks;
using Blazor.ModalDialog;
using Microsoft.AspNetCore.Components;
using RpgApp.Shared;
using RpgApp.Shared.Types;
using RpgApp.Shared.Types.Enums;

namespace RpgApp.Client.Pages.Modals
{
    public partial class CharacterCreationModal
    {
        [CascadingParameter(Name = "AppState")]
        public AppStateManager AppState { get; set; }
        [Inject]
        public IModalDialogService ModalDialogService { get; set; }
        private Player CurrentPlayer { get; set; }
        private ClassType CharClass { get; set; }
        private string name;
       
        public async Task CreateNewPlayer(ClassType classType)
        {
            var options = new ModalDialogOptions()
            {
                Style = ModalSyles.GoldenFramed2(ModalSize.Medium)
            };
            CurrentPlayer = await CreateCharacter.CreateNewCharacter(classType, AppState.AllSkills, AppState.AllEquipment);
            AppState.UpdateCurrentPlayer(CurrentPlayer);
            CurrentPlayer.Name = name;
            ModalDialogResult result = await ModalDialogService.ShowDialogAsync<CharacterResultModal>("Welcome, " + name + "!", options);
            StateHasChanged();
            ModalDialogService.Close(true);
        }
    }
}
