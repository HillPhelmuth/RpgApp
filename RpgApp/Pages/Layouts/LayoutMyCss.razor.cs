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
using RpgApp.Shared.Types.Enums;
using RpgComponentLibrary.Components;

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
        [Inject]
        private ClientDataService ClientDataService { get; set; }
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
        private AnimationModel moveAnimation = new(SpriteSets.OverheadSprites, "Right", 3, 4);
        private List<CollisionBlock> collisionsBlocks = new();
        private CanvasSpecs canvasSpecs = new(600, 1100);
        private KeyValuePair<string, string> background = new("village1", "/css/Images/Village1.png");
        private bool hasCollided;
        private bool stopTimer;
        private string currentMap = "Home";
        private TopViewAvatar avatarView;
        private void SetMapCollisionns(int monsterCount)
        {
            var rng = new Random();
            collisionsBlocks.Add(new CollisionBlock("Heal", "_content/RpgComponentLibrary/img/icons/foreign/heart.png", 64, 64, 400, 300));

            for (var i = 0; i < monsterCount; i++)
            {
                var x = rng.Next(0, 1100);
                var y = rng.Next(0, 600);
                collisionsBlocks.Add(new CollisionBlock($"Attack{i}", "_content/RpgComponentLibrary/img/icons/foreign/x.png", 64, 64, x, y));
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
                TopViewAvatar.Green => new AnimationModel(SpriteSets.OverheadSprites, "Right", 3, 4),
                TopViewAvatar.Boyle => new AnimationModel(SpriteSets.BoyleSprites, "Right", 3, 4),
                TopViewAvatar.Pink => new AnimationModel(SpriteSets.PinkSprites, "Right", 3, 4),
                TopViewAvatar.BurgerKing => new AnimationModel(SpriteSets.KingSprites, "Right", 4, 5),
                TopViewAvatar.ChearLeader => new AnimationModel(SpriteSets.CheerSprites, "Right", 3, 4),
                TopViewAvatar.Cop => new AnimationModel(SpriteSets.CopSprites, "Right", 3, 4),
                _ => new AnimationModel(SpriteSets.OverheadSprites, "Right", 3, 4),
            };
        }
        private void InitializeMap()
        {
            collisionsBlocks.Add(new CollisionBlock("Dungeon", "dungeon-54.png", 54, 54, 1000, 525));
            SetMapCollisionns(10);
        }


        private void HandleMove((double x, double y) pos)
        {
            moveUpdates.Add($"moved to {pos.x}-{pos.y}");

        }

        private List<string> collisionList = new();
        private async void HandleCollision(string name)
        {
            Console.WriteLine($"Collision at: {name} From: {string.Join(' ', collisionsBlocks.Where(x => x.Name == name).Select(x => x.ToString()).ToArray())}");
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
                var result = await ShowMessageModal("Collision!", $"You collided with a {name} collision Block! You have been HEALED");
                stopTimer = false;
            }
            else if (name.Contains("Dungeon", StringComparison.OrdinalIgnoreCase))
            {
                stopTimer = true;
                StateHasChanged();
                var result = await ShowMessageModal("Collision!", $"You collided with a {name} collision Block! You have been to the moved the Dungeon");
                stopTimer = false;

                ToggleCss();
                collisionsBlocks = new List<CollisionBlock>
                {
                    new("Home", "home-48.png", 54, 54, 50, 25)
                };
                SetMapCollisionns(20);
                currentMap = "Dungeon";
            }
            else if (name.Contains("Home", StringComparison.OrdinalIgnoreCase))
            {
                collisionsBlocks = new List<CollisionBlock>();
                InitializeMap();
            }

            if (name == "LeftEdge")
                moveAnimation.PosX = canvasSpecs.W - 48;
            else if (name == "RightEdge")
                moveAnimation.PosX = 0;

            var collision = collisionsBlocks.Find(x => x.Name == name);
            collisionsBlocks.Remove(collision);
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

        private List<string> _backgroundKeys => _backgrounds.Keys.ToList();
        private Dictionary<string, string> _backgrounds = new()
        {
            { "dungeon", "/css/Images/Dungeon1.png" },
            { "village", "/css/Images/Village1.png" },
            { "outdoorDark", "/css/Images/OutDark.png" },
            { "outDoor", "/css/Images/OutDoorMap.png" }
        };
        private void ToggleCss(string name = "village1")
        {
            background = _backgrounds.FirstOrDefault(x => x.Key == name);
            //currentMap = currentMap == "Home" ? "Dungeon" : "Home";
            //background = currentMap == "Dungeon"
            //    ? new("dungeon1", "/css/Images/OutDoorMap.png")
            //    : new("village1", "/css/Images/Village1.png");

            //gridCss = gridCss == "primary-grid" ? "primary-grid1" : "primary-grid";
        }
        private async void HandleCombatEnded(bool isVictory)
        {
            if (isVictory)
            {
                await ShowMessageModal("Victory!", "It's time get back to the road to continue your fucking quest you goddamn slacker");
                await ClientDataService.AddOrUpdatePlayer(AppState.CurrentPlayer);
                //await Http.PostAsJsonAsync($"{AppConstants.ApiUrl}/UpdateOrAddPlayer", AppState.CurrentPlayer);
            }
            else
            {
                AppState.CurrentPlayer.Health = AppState.CurrentPlayer.MaxHealth;
                AppState.PlayerLocation = (0, 0);
            }

            isCombatActive = false;
            await InvokeAsync(StateHasChanged);
        }
        private async Task<bool> ShowMessageModal(string alertTitle, string alertMessage)
        {
            var options = new ModalDialogOptions
            {
                Style = ModalStyles.Framed(ModalSize.Small),
                Position = ModalDialogPositionOptions.Center
            };
            var parameters = new ModalDialogParameters
            {
                {"HtmlMarkupContent", $"<h1>{alertMessage}</h1>"}
            };
            var result = await ModalService.ShowDialogAsync<MessageBoxModal>(alertTitle, options, parameters);
            return result.Success;
        }
        private void UpdateState(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "CurrentPlayer") return;
            Console.WriteLine($"{e.PropertyName} change handled by {nameof(LayoutMyCss)}");
            CurrentPlayer = AppState.CurrentPlayer;
            InvokeAsync(StateHasChanged);
        }
        public async Task ShowMenu()
        {
            var options = new ModalDialogOptions
            {
                Style = ModalStyles.Framed(ModalSize.Small)
            };
            var result = await ModalService.ShowDialogAsync<MenuModal>("Menu", options);
            StateHasChanged();
        }

        public void Dispose() => AppState.PropertyChanged -= UpdateState;
    }
}
