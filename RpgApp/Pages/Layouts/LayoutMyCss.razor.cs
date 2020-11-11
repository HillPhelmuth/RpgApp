using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop;
using TurnBasedRpg.StateManager;
using TurnBasedRpg.Types;
using Blazor.ModalDialog;
using Blazor.ModalDialog.Components;
using TurnBasedRpg.Pages.CombatTests;
using TurnBasedRpg.Services;

namespace TurnBasedRpg.Pages.Layouts
{
    public partial class LayoutMyCss:IDisposable
    {
        [Inject]
        public AppStateManager AppStateManager { get; set; }
        [Inject]
        protected IJSRuntime JsRuntime { get; set; }
        [Inject]
        public IModalDialogService ModalService { get; set; }
        [Inject]
        public RpgDataService RpgData { get; set; }
        [Parameter] 
        public (int x, int y) PlayerLoc { get; set; } = (0, 0);
        protected Player CurrentPlayer { get; set; }
        private ElementReference gameboardReference;
        private List<string> moveUpdates = new List<string>();
        private string gridCss = "primary-grid";
        private enum Direction { Blank, Up, Down, Left, Right }
        private Random random = new Random();
        protected override Task OnInitializedAsync()
        {
            CurrentPlayer = AppStateManager.CurrentPlayer;
            AppStateManager.OnChange += UpdateState;
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
            var playerLocation = PlayerLoc;
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

            PlayerLoc = playerLocation;
            AppStateManager.UpdatePlayerLocation(PlayerLoc);
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
            var randomVal = random.Next(1, 8);
            if (randomVal != 1)
            {
                await InvokeAsync(StateHasChanged);
                return;
            }
            await TriggerCombat();
            await InvokeAsync(StateHasChanged);
        }

        private async Task TriggerCombat()
        {
            var options = new ModalDialogOptions
            {
                ShowCloseButton = false,
                BackgroundClickToClose = false,
                Style = "liquid-modal-dialog-combat"
            };
            var difficulty = random.Next(1, CurrentPlayer.Level + 3);
            var monsterOdds = random.Next(1, 101);
            var monsterCount = monsterOdds <= 50 ? 1 : monsterOdds <= 85 ? 2 : 3;
            var parameters = new ModalDialogParameters {{"Difficulty", difficulty}, {"MonsterCount", monsterCount}};
            var result =
                await ModalService.ShowDialogAsync<MultiMonster>($"Holy Shit! You Were Attacked by {monsterCount} monsters!",
                    options, parameters);
            if (result.Success)
            {
                //await ModalService.ShowMessageBoxAsync("Victory!", "It's time get back to the road to continue your fucking quest you goddamn slacker");
                moveUpdates.Add("Victory!");
                moveUpdates.Add("It's time get back to the road to continue your fucking quest you goddamn slacker");
                //if (innerResult == MessageBoxDialogResult.Cancel && innerResult == MessageBoxDialogResult.OK)
                //    return;
                //var currentPlayer = result.ReturnParameters.Get<Player>("CurrentPlayer");
                //var messages = result.ReturnParameters.Get<List<string>>("Messages");
                //while (messages.Count > 5) messages.RemoveAt(0);
                //moveUpdates.AddRange(messages);
                //await AppStateManager.UpdateCurrentPlayer(currentPlayer);
            }
            else
            {
                CurrentPlayer.Health = CurrentPlayer.MaxHealth;
                PlayerLoc = (0, 0);
            }
            await RpgData.UpdateOrAddPlayer(CurrentPlayer);
        }

        private void ToggleCss()
        {
            gridCss = gridCss == "primary-grid" ? "primary-grid1" : "primary-grid";
        }

        private Task UpdateState()
        {
            CurrentPlayer = AppStateManager.CurrentPlayer;
            InvokeAsync(StateHasChanged);
            return Task.CompletedTask;
        }
        public async Task ShowMenu()
        {
            ModalDialogResult result = await ModalService.ShowDialogAsync<MenuModal>("Menu");
            StateHasChanged();
        }

        public void Dispose()
        {
            AppStateManager.OnChange -= UpdateState;
        }

    }
}
