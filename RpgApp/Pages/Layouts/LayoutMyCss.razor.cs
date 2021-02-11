using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazor.ModalDialog;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using RpgApp.Client.Pages.Modals;
using RpgApp.Shared;
using RpgApp.Shared.Types;
using RpgApp.Shared.Types.Enums;

namespace RpgApp.Client.Pages.Layouts
{
    public partial class LayoutMyCss : ComponentBase, IDisposable
    {
        [CascadingParameter(Name = "AppState")]
        public AppStateManager AppState { get; set; }
        [Inject]
        protected IJSRuntime JsRuntime { get; set; }
        [Inject]
        public IModalDialogService ModalService { get; set; }
        [Inject]
        public HttpClient Http { get; set; }
        [Parameter]
        public (int x, int y) PlayerLoc { get; set; } = (0, 0);
        protected Player CurrentPlayer { get; set; }
        private ElementReference gameboardReference;
        private List<string> moveUpdates = new List<string>();
        private string gridCss = "primary-grid";
        private enum Direction { Blank, Up, Down, Left, Right }
        private Random random = new Random();
        private bool isCombatActive = false;

        private int monsterCount;
        protected override Task OnInitializedAsync()
        {
            CurrentPlayer = AppState.CurrentPlayer;
            AppState.PropertyChanged += UpdateState;
            return base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JsRuntime.InvokeVoidAsync("TurnBasedRpg.SetFocusToElement", gameboardReference);
                StateHasChanged();
            }
        }


        private void MovePlayer(Direction direction)
        {
            Console.Write($"Moved {Enum.GetName(direction)}");
            var playerLocation = AppState.PlayerLocation;
            if (playerLocation.x == 0 && direction == Direction.Left)
                return;
            if (playerLocation.y == 0 && direction == Direction.Up)
                return;
            if (playerLocation.x == 11 && direction == Direction.Right)
                return;
            if (playerLocation.y == 11 && direction == Direction.Down)
                return;

            switch (direction)
            {
                case Direction.Down:
                    playerLocation.y++;
                    break;
                case Direction.Up:
                    playerLocation.y--;
                    break;
                case Direction.Left:
                    playerLocation.x--;
                    break;
                case Direction.Right:
                    playerLocation.x++;
                    break;
            }

            var randomVal = random.Next(1, 8);
            if (randomVal == 1)
            {
                TriggerCombat();
            }
            AppState.PlayerLocation = playerLocation;
            //AppState.UpdatePlayerLocation(PlayerLoc);
            StateHasChanged();
        }

        protected void KeyboardMove(KeyboardEventArgs args)
        {
            Console.WriteLine($"Keyboard event triggered. {args.Key}/{args.Code} pressed");
            var direction = args.Key switch
            {
                "w" => Direction.Up,
                "ArrowUp" => Direction.Up,
                "s" => Direction.Down,
                "ArrowDown" => Direction.Down,
                "a" => Direction.Left,
                "ArrowLeft" => Direction.Left,
                "d" => Direction.Right,
                "ArrowRight" => Direction.Right,
                _ => Direction.Blank
            };
            MovePlayer(direction);
        }
        protected async Task HandlePlayerLocChanged((int x, int y) location)
        {
            var moveInfo = $"Player moved to Row-{location.y} and Column-{location.x}.";
            moveUpdates.Add(moveInfo);
            if (moveUpdates.Count > 5)
                moveUpdates.RemoveAt(0);
            
            await InvokeAsync(StateHasChanged);
        }

        private void TriggerCombat()
        {
            var monsterOdds = random.Next(1, 101);
            monsterCount = monsterOdds <= 50 ? 1 : monsterOdds <= 85 ? 2 : 3;
            isCombatActive = true;
            StateHasChanged();
        }

        private void ToggleCss()
        {
            gridCss = gridCss == "primary-grid" ? "primary-grid1" : "primary-grid";
        }
        private async void HandleCombatEnded(bool isVictory)
        {
            if (isVictory)
            {
                await ModalService.ShowMessageBoxAsync("Victory!", "It's time get back to the road to continue your fucking quest you goddamn slacker");
                await Http.PostAsJsonAsync($"{AppConstants.ApiUrl}/UpdateOrAddPlayer", CurrentPlayer);
            }
            else
            {
                AppState.CurrentPlayer.Health = AppState.CurrentPlayer.MaxHealth;
                AppState.PlayerLocation = (0, 0);
            }

            isCombatActive = false;
            await InvokeAsync(StateHasChanged);
        }
        private void UpdateState(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "CurrentPlayer") return;
            CurrentPlayer = AppState.CurrentPlayer;
            InvokeAsync(StateHasChanged);
        }
        public async Task ShowMenu()
        {
            var options = new ModalDialogOptions()
            {
                Style = ModalStyles.Framed(ModalSize.Small)
            };
            var result = await ModalService.ShowDialogAsync<MenuModal>("Menu", options);
            StateHasChanged();
        }

        public void Dispose() => AppState.PropertyChanged -= UpdateState;
    }
}
