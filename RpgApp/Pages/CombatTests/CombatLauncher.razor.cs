using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using RpgApp.Shared.StateManager;
using RpgApp.Shared.Types.Enums;

namespace RpgApp.Client.Pages.CombatTests
{
    public partial class CombatLauncher
    {
        [CascadingParameter(Name = "AppState")]
        public AppStateManager AppState { get; set; }
        private int _monsterCount = 1;
        [Parameter]
        public int MonsterCount
        {
            get => _monsterCount;
            set
            {
                if (value < 1)
                    _monsterCount = 1;
                else if (value > 3)
                    _monsterCount = 3;
                else
                    _monsterCount = value;
            }
        }
        [Parameter]
        public EventCallback<bool> OnCombatEnded { get; set; }

        private string videoSrc = "<iframe src = \"https://giphy.com/embed/UvQ2W4OYg3EKahV5Xe\" width = \"480\" height = \"270\" frameBorder = \"0\" class=\"giphy-embed\" allowFullScreen></iframe><p><a href =\"https://giphy.com/gifs/ff-sony-final-fantasy-UvQ2W4OYg3EKahV5Xe\"> via GIPHY</a></p>";
        private bool isCombatReady = false;
        protected override Task OnInitializedAsync()
        {
            
            videoSrc = AppState.CurrentPlayer.ClassType switch
            {
                ClassType.Mage => "<iframe src = \"https://giphy.com/embed/gLRTwEwvBlhLBYThOq\" width = \"960\" height = \"540\" frameBorder = \"0\" class=\"giphy-embed\" allowFullScreen></iframe><p><a href = \"https://giphy.com/gifs/xbox-xbox-one-ffx-x-gLRTwEwvBlhLBYThOq\"> via GIPHY</a></p>",
                ClassType.Ranger => "<iframe src = \"https://giphy.com/embed/26tncKS342OAhfGYU\" width = \"960\" height = \"540\" frameBorder = \"0\" class=\"giphy-embed\" allowFullScreen></iframe><p><a href =\"https://giphy.com/gifs/xbox-xbox-one-ffx-x-gLRTwEwvBlhLBYThOq\"> via GIPHY</a></p>",
                ClassType.Warrior => "<iframe src = \"https://giphy.com/embed/UvQ2W4OYg3EKahV5Xe\" width = \"960\" height = \"540\" frameBorder = \"0\" class=\"giphy-embed\" allowFullScreen></iframe><p><a href =\"https://giphy.com/gifs/ff-sony-final-fantasy-UvQ2W4OYg3EKahV5Xe\"> via GIPHY</a></p>",
                _ => ""
            };
           
            return base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                isCombatReady = false;
                await Task.Delay(2000);
                isCombatReady = true;
                StateHasChanged();
            }
            
        }

        private void HandleCombatEnded(bool isPlayerWon)
        {
            OnCombatEnded.InvokeAsync(isPlayerWon);
        }
    }
}
