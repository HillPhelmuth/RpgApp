using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
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
using RpgComponentLibrary.Animations;

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
            InitializeMap();
            return base.OnInitializedAsync();
        }

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        //await JsRuntime.InvokeVoidAsync("TurnBasedRpg.SetFocusToElement", gameboardReference);
        //        StateHasChanged();
        //    }
        //}

        #region Global animation controls
        private AnimationModel moveAnimation = new(SpriteSets.OverheadSprites, "Right", 4, 5);
        private List<CollisionBlock> collitionsBlocks = new();
        private CanvasSpecs canvasSpecs = new(600, 1100);
        private KeyValuePair<string, string> background = new("village1", "/css/Images/Village1.png");
        private bool hasCollided;
        private bool stopTimer;
        private string currentMap = "Home";
        private TopViewAvatar avatarView;
        private void SetMapCollisionns(int monsterCount)
        {
            var rng = new Random();
            collitionsBlocks.Add(new CollisionBlock("Heal", "_content/RpgComponentLibrary/img/icons/foreign/heart.png", 64, 64, 400, 300));
            
            for (var i = 0; i < monsterCount; i++)
            {
                var x = rng.Next(0, 1100);
                var y = rng.Next(0, 600);
                collitionsBlocks.Add(new CollisionBlock($"Attack{i}", "_content/RpgComponentLibrary/img/icons/foreign/x.png", 64, 64, x, y));
            }
        }

        private void HandleSprites(TopViewAvatar avatar)
        {
            moveAnimation = ChangeSprites(avatar);
        }
        private AnimationModel ChangeSprites(TopViewAvatar avatar)
        {
            return avatar switch
            {
                TopViewAvatar.Green => new AnimationModel(SpriteSets.OverheadSprites, "Right", 4, 5),
                TopViewAvatar.Boyle => new AnimationModel(SpriteSets.BoyleSprites, "Right", 4, 5),
                TopViewAvatar.Pink => new AnimationModel(SpriteSets.PinkSprites, "Right", 4, 5),
                TopViewAvatar.BurgerKing => new AnimationModel(SpriteSets.KingSprites, "Right", 4,5),
                TopViewAvatar.ChearLeader => new AnimationModel(SpriteSets.CheerSprites, "Right", 4, 5),
                TopViewAvatar.Cop => new AnimationModel(SpriteSets.CopSprites, "Right", 4, 5),
                _ => new AnimationModel(SpriteSets.OverheadSprites, "Right", 4, 5),
            };
        }
        private void InitializeMap()
        {
            collitionsBlocks.Add(new CollisionBlock("Dungeon", "dungeon-54.png", 54, 54, 1000, 525));
            SetMapCollisionns(10);
        }
        
        
        private void HandleMove((double x, double y) pos)
        {
            moveUpdates.Add($"moved to {pos.x}-{pos.y}");

        }

        private List<string> collisionList = new();
        private async void HandleCollision(string name)
        {
            Console.WriteLine($"Collision at: {name} From: {string.Join(' ', collitionsBlocks.Select(x => x.ToString()).ToArray())}");
            if (collisionList.Contains(name)) return;
            collisionList.Add(name);
            if (name.Contains("Attack", StringComparison.OrdinalIgnoreCase))
            {
                TriggerCombat();
            }
            else if (name.Contains("Heal", StringComparison.OrdinalIgnoreCase) || name == "Heal")
            {
                AppState.CurrentPlayer.Health = AppState.CurrentPlayer.MaxHealth;
                AppState.CurrentPlayer.AbilityPoints = AppState.CurrentPlayer.MaxAbilityPoints;
                stopTimer = true;
                StateHasChanged();
                 var result = await ModalService.ShowMessageBoxAsync("Collision!", $"You collided with a {name} collision Block! You have been HEALED", MessageBoxButtons.OK);
                 if (result == MessageBoxDialogResult.None || result != MessageBoxDialogResult.None)
                 {
                    stopTimer = false;
                 }
            }
            else if (name.Contains("Dungeon", StringComparison.OrdinalIgnoreCase))
            {
                stopTimer = true;
                StateHasChanged();
                var result = await ModalService.ShowMessageBoxAsync("Collision!", $"You collided with a {name} collision Block! You have been moved the Dungeon", MessageBoxButtons.OK);
                if (result == MessageBoxDialogResult.None || result != MessageBoxDialogResult.None)
                {
                    stopTimer = false;
                }
                ToggleCss();
                collitionsBlocks = new List<CollisionBlock>
                {
                    new CollisionBlock("Home", "home-48.png", 54, 54, 50, 25)
                };
                SetMapCollisionns(20);
                currentMap = "Dungeon";
            }
            else if (name.Contains("Home", StringComparison.OrdinalIgnoreCase))
            {
                collitionsBlocks = new List<CollisionBlock>();
                InitializeMap();
            }

            var collision = collitionsBlocks.Find(x => x.Name == name);
            collitionsBlocks.Remove(collision);
            await InvokeAsync(StateHasChanged);
          

        }
        #endregion

        #region Old Grid Shit

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

        #endregion


        private void TriggerCombat()
        {
            var monsterOdds = random.Next(1, 101);
            monsterCount = monsterOdds <= 50 ? 1 : monsterOdds <= 85 ? 2 : 3;
            isCombatActive = true;
            StateHasChanged();
        }

        private void ToggleCss(string name = "Dungeon")
        {
            currentMap = currentMap == "Home" ? "Dungeon" : "Home";
            background = currentMap == "Dungeon"
                ? new("dungeon1", "/css/Images/Dungeon1.png")
                : new("village1", "/css/Images/Village1.png");
            
            //gridCss = gridCss == "primary-grid" ? "primary-grid1" : "primary-grid";
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
            var result = await ModalService.ShowDialogAsync<MenuModal>("Menu");
            StateHasChanged();
        }

        public void Dispose() => AppState.PropertyChanged -= UpdateState;
    }
}
