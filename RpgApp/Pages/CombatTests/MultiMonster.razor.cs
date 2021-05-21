using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Blazor.ModalDialog;
using Microsoft.AspNetCore.Components;
using RpgApp.Client.Pages.Modals;
using RpgApp.Shared;
using RpgApp.Shared.Services;
using RpgApp.Shared.Types;
using RpgApp.Shared.Types.Enums;
using RpgApp.Shared.Types.PlayerExtensions;
using RpgComponentLibrary.Animations;
using RpgComponentLibrary.Components;

namespace RpgApp.Client.Pages.CombatTests
{
    public partial class MultiMonster : ComponentBase, IDisposable
    {
        [Inject]
        public CombatService CombatService { get; set; }
       
        [Inject]
        public IModalDialogService ModalDialogService { get; set; }
        [CascadingParameter(Name = "AppState")]
        public AppStateManager AppState { get; set; }
        public Player CurrentPlayer { get; set; }

        private CombatPlayer _combatPlayer = new();

        [Parameter]
        public int Difficulty { get; set; }
        [Parameter]
        public EventCallback<bool> OnCombatEnded { get; set; }

        private int _monsterCount = 1;
        [Parameter]
        public int MonsterCount
        {
            get => _monsterCount;
            set
            {
                _monsterCount = value switch
                {
                    < 1 => 1,
                    > 3 => 3,
                    _ => value
                };
            }
        }

        public Dictionary<string, Monster> AllMonsters = new(3)
        {
            {"Monster 1", new Monster{isDead = true}},{"Monster 2",new Monster{isDead = true}},{"Monster 3",new Monster{isDead = true}}
        };
       
        //private bool isPlayerHit;
        //private bool isSkillMenu;
        private bool isPlayerDefeated;
        [Parameter]
        public List<string> Messages { get; set; } = new();
        
        #region Animation

        private AnimationCombatActions combatActions = new();
        
        private AnimationModel combatAnimation;
        
        private CanvasSpecs canvasSpecs = new (300, 325);
        
        private void SetCombatAnimationData()
        {
            var spriteSet = AppState.CurrentPlayer?.ClassType switch
            {
                ClassType.Mage => SpriteSets.WizardSprites,
                ClassType.Warrior => SpriteSets.WarriorSprites,
                ClassType.Ranger => SpriteSets.ArcherSprites,
                _ => SpriteSets.WarriorSprites
            };
            combatAnimation = new AnimationModel(spriteSet, "Idle", 3);
        }
        private Task RunAnimation(string animation)
        {
           combatActions.TriggerAnimation(animation);
           return Task.CompletedTask;
        }

        #endregion

        protected override async Task OnInitializedAsync()
        {
            CurrentPlayer = AppState.CurrentPlayer;
            SetCombatAnimationData();
            
            var monsters = AppState.AllMonsters.Where(x => x.DifficultyLevel <= 1).ToList();

            var count = Math.Min(monsters.Count, MonsterCount);
            AllMonsters = new Dictionary<string, Monster>();
            for (int i = 1; i <= count; i++)
            {
                AllMonsters[$"Monster {i}"] = monsters[i - 1];
            }

            await BeginCombat();
            AppState.PropertyChanged += UpdateState;
            CombatService.OnCombatEnded += HandleCombatEnded;
            CombatService.OnNewMessage += HandleNewMessage;
            CombatService.OnPlayerHit += HandlePlayerHit;
            CombatService.OnMonsterAttack += HandleMonsterAttack;
        }

        private async Task BeginCombat()
        {
            _combatPlayer = CurrentPlayer.ConvertToCombatMode();
            var armor = CurrentPlayer.Inventory.Find(x => x.Name == "Leather armor") ?? AppState.AllEquipment.Find(x => x.Name == "Leather armor");
            
            var weapons = _combatPlayer.Inventory ?? new List<Equipment> { new Equipment { EquipLocation = "TwoHands" } };
            var weapon = weapons.Find(x => x.Effects.Any(e => e.Type == EffectType.Attack));
            Console.WriteLine($"weapon: {JsonSerializer.Serialize(weapon)}");
            _combatPlayer.EquipArmor(armor);
            _combatPlayer.EquipWeapon(weapon);
            await CombatService.BeginCombat(_combatPlayer, AllMonsters);

        }
        private async Task Attack(string key)
        {
            if (string.IsNullOrEmpty(key)) return;
            targetMonster = key;
            Console.WriteLine("Attack!");
            var rng = new Random();
            await RunAnimation($"Attack{rng.Next(1, 3)}");
            //await TriggerAttack(key);
            //CurrentPlayer.UpdateDuringCombat(_combatPlayer);
            //await InvokeAsync(StateHasChanged);
        }

        private string targetMonster;
        private async void TriggerAttack(string animArgs)
        {
            if (string.IsNullOrEmpty(targetMonster)) return;
            if (!animArgs.Contains("Attack", StringComparison.OrdinalIgnoreCase)) return;
            var key = targetMonster;
            var dice = _combatPlayer.DamageDice;
            await CombatService.PlayerAttack(dice, key);
            if (AllMonsters[key].Health <= 0) AllMonsters[key].isDead = true;
        }

        private Skill selectedSkill = new();
        private async Task UseSkill(Skill skill)
        {
            //isSkillMenu = false;
            await InvokeAsync(StateHasChanged);
            await CombatService.PlayerUseSkill(skill);
            CurrentPlayer.UpdateDuringCombat(_combatPlayer);
            await InvokeAsync(StateHasChanged);
        }
        //public void OpenMenu() => isSkillMenu = true;
        private async Task Flee()
        {
            //Not Implemented
            await CombatService.PlayerFlee();
        }
        private void UpdateState(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Monsters" && e.PropertyName != "CurrentPlayer") return;
            Console.WriteLine($"{e.PropertyName} change handled by {nameof(MultiMonster)}");
            AllMonsters = AppState.CombatMonsters;
            CurrentPlayer = AppState.CurrentPlayer;
            InvokeAsync(StateHasChanged);
        }
        #region Event Handlers


        private async Task HandleCombatEnded(bool isPlayerDead)
        {
            isPlayerDefeated = _combatPlayer?.Health <= 0;
            if (isPlayerDefeated)
                await RunAnimation("Dead");
            int totalExp = AllMonsters.Values.Sum(x => x.ExpProvided);
            int totalGold = AllMonsters.Values.Sum(x => x.GoldProvided);
            AppState.CurrentPlayer.ApplyCombatResults(_combatPlayer);
            AppState.CurrentPlayer.Experience += totalExp;
            AppState.CurrentPlayer.Gold += totalGold;
            string alertTitle = isPlayerDefeated ? "You Lose!" : "You Win!";
            string alertMessage = isPlayerDefeated ? "You get nothing and start over"
                : $"You've received {totalGold}gp and earned {totalExp}xp";

            AppState.UpdateCurrentPlayer(AppState.CurrentPlayer);
            Console.WriteLine($"CurrentPlayer Stats: {_combatPlayer}");
            
            AllMonsters = new Dictionary<string, Monster>(3)
            {
                {"Monster 1", new Monster{isDead = true}},{"Monster 2",new Monster{isDead = true}},{"Monster 3",new Monster{isDead = true}}
            };
            await ShowMessageModal(alertTitle, alertMessage);

            await OnCombatEnded.InvokeAsync(!isPlayerDefeated);

        }

        private async Task ShowMessageModal(string alertTitle, string alertMessage)
        {
            var options = new ModalDialogOptions()
            {
                Style = ModalStyles.Framed(ModalSize.Small), Position = ModalDialogPositionOptions.Center
            };
            var parameters = new ModalDialogParameters
            {
                {"HtmlMarkupContent", $"<h1>{alertMessage}</h1>"}
            };
            await ModalDialogService.ShowDialogAsync<MessageBoxModal>(alertTitle, options, parameters);
        }

        private List<Task> AnimationEventList = new();
        private async void HandlePlayerHit(bool isPlayer, string monsterId)
        {
            //isPlayerHit = isPlayer;
            if (isPlayer)
            {
                await RunAnimation("Hit");
            }

            if (!monsterId.Contains("Monster"))
                return;
            AllMonsters[monsterId].IsHit = true;
            CurrentPlayer.UpdateDuringCombat(_combatPlayer);
            //await Task.Delay(500);
            Console.WriteLine($"HandlePlayerHit \r\n monsterId: {monsterId}");
            await InvokeAsync(StateHasChanged);
        }

        private void HandleMonsterAttack(string monster)
        {
            AllMonsters[monster].isAttack = true;
            StateHasChanged();
            //AllMonsters[monster].isAttack = false;
        }
        private void OnMonsterHit(string monsterId) => AllMonsters[monsterId].IsHit = false;
        //private void OnPlayerHit() => isPlayerHit = false;
        private void HandleNewMessage(string message)
        {
            Messages.Add($"<p>{message}</p>");
            if (Messages.Count > 10)
                Messages.RemoveAt(0);
            foreach (var monsterEntry in AllMonsters.Where(x => x.Value.Health <= 0 && !x.Value.isDead))
            {
                monsterEntry.Value.isDead = true;
            }
            CurrentPlayer.Health = _combatPlayer.Health;
            CurrentPlayer.AbilityPoints = _combatPlayer.AbilityPoints;
            InvokeAsync(StateHasChanged);
        }
        #endregion

        public void Dispose()
        {
            AppState.PropertyChanged -= UpdateState;
            CombatService.OnCombatEnded -= HandleCombatEnded;
            CombatService.OnPlayerHit -= HandlePlayerHit;
            CombatService.OnNewMessage -= HandleNewMessage;
            CombatService.OnMonsterAttack -= HandleMonsterAttack;
            Console.WriteLine("MultiMonster.razor has been disposed");
        }
    }
}
