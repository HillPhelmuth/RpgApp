using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TurnBasedRpg.StateManager;
using TurnBasedRpg.Types;
using TurnBasedRpg.Types.Enums;
using Blazor.ModalDialog;

namespace TurnBasedRpg.Pages
{
    public partial class CharacterCreationModal
    {
        [Inject]
        public AppStateManager AppStateManager { get; set; }
        [Inject]
        public IModalDialogService ModalDialogService { get; set; }
        protected Player CurrentPlayer { get; set; }
        protected ClassType CharClass { get; set; }
        protected string name;

        public async Task CreateNewPlayer(ClassType classType)
        {
            var create = new CreateCharacter();
            CurrentPlayer = await create.CreateNewCharacter(classType);
            await AppStateManager.UpdateCurrentPlayer(CurrentPlayer);
            CurrentPlayer.Name = name;
            ModalDialogResult result = await ModalDialogService.ShowDialogAsync<CharacterResultModal>("Welcome, " + name + "!");
            StateHasChanged();
            ModalDialogService.Close(true);
        }
    }
}
