using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Blazor.ModalDialog;
using Microsoft.AspNetCore.Components;
using RpgApp.Shared;
using RpgApp.Shared.Services;
using RpgApp.Shared.StateManager;
using RpgApp.Shared.Types;
using RpgApp.Shared.Types.Enums;
using RpgApp.Shared.Types.PlayerExtensions;

namespace RpgApp.Client.Pages.CombatTests
{
    public partial class MultiMonster : IDisposable
    {
        [Inject]
        public CombatService CombatService { get; set; }
        [Inject]
        public HttpClient HttpClient { get; set; }
        [Inject]
        public IModalDialogService ModalDialogService { get; set; }
        [CascadingParameter(Name = "AppState")]
        public AppStateManager AppState { get; set; }
        public Player CurrentPlayer { get; set; }

        private CombatPlayer _combatPlayer;
        
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
                if (value < 1)
                    _monsterCount = 1;
                else if (value > 3)
                    _monsterCount = 3;
                else
                    _monsterCount = value;

            }
        }

        public Dictionary<string, Monster> AllMonsters = new Dictionary<string, Monster>(3)
        {
            {"Monster 1", new Monster{isDead = true}},{"Monster 2",new Monster{isDead = true}},{"Monster 3",new Monster{isDead = true}}
        };
        private List<Skill> AllSkillsTemp = new List<Skill>();
        private bool isBeginCombat;
        private bool isMonsterDead;
        private bool isPlayerHit;
        private bool isSkillMenu;
        private bool isPlayerDefeated;
        [Parameter]
        public List<string> Messages { get; set; } = new List<string>();
        private string cssString = "";
        private string playerUrl;

        private Dictionary<Player, string> CssDictionary { get; set; }
        protected override async Task OnInitializedAsync()
        {
            CurrentPlayer = AppState.CurrentPlayer;
            // Temporary for testing

            _combatPlayer = CurrentPlayer.ConvertToCombatMode();
            cssString = CurrentPlayer.ClassType switch
            {
                ClassType.Warrior => "warrior-img",
                ClassType.Mage => "mage-img",
                ClassType.Ranger => "ranger-img",
                _ => ""
            };

            var max = 1;
            
            var monsters = AppState.AllMonsters.Where(x => x.DifficultyLevel <= 1).ToList();
           
            var count = Math.Min(monsters.Count, MonsterCount);
            AllMonsters = new Dictionary<string, Monster>();
            for (int i = 1; i <= MonsterCount; i++)
            {
                AllMonsters[$"Monster {i}"] = monsters[i - 1];
                //AllMonsters.Add($"", monsters[i-1]);
            }

            AllSkillsTemp = _combatPlayer.Skills;
            //var armor = await HttpClient.GetFromJsonAsync<Equipment>($"{AppConstants.ApiUrl}/GetEquipmentById/1");
            var armor = AppState.AllEquipment.FirstOrDefault(x => x.Id == 1);
            _combatPlayer.EquipArmor(armor);
            await BeginCombat();
            AppState.PropertyChanged += UpdateState;
            CombatService.OnCombatEnded += HandleCombatEnded;
            CombatService.OnNewMessage += HandleNewMessage;
            CombatService.OnPlayerHit += HandlePlayerHit;
        }

        private async Task BeginCombat()
        {
            isBeginCombat = true;
            var weapons = _combatPlayer?.Inventory ?? new List<Equipment> { new Equipment { EquipLocation = "TwoHands" } };
            var weapon = weapons.FirstOrDefault(x => x.Effects.Any(e => e.Type == EffectType.Attack));
            Console.WriteLine($"weapon: {JsonSerializer.Serialize(weapon)}");
            _combatPlayer.EquipWeapon(weapon);
            await CombatService.BeginCombat(ref _combatPlayer, ref AllMonsters);

        }
        private async Task Attack(string key)
        {
            Console.WriteLine("Attack!");

            var dice = _combatPlayer.DamageDice;
            await CombatService.PlayerAttack(dice, key);
            if (AllMonsters[key].Health <= 0) AllMonsters[key].isDead = true;
            CurrentPlayer.UpdateDuringCombat(_combatPlayer);
            await InvokeAsync(StateHasChanged);
        }
        private async Task UseSkill(Skill skill)
        {
            isSkillMenu = false;
            await InvokeAsync(StateHasChanged);
            await CombatService.PlayerUseSkill(skill);
            CurrentPlayer.UpdateDuringCombat(_combatPlayer);
            await InvokeAsync(StateHasChanged);
        }
        public void OpenMenu() => isSkillMenu = true;
        private async Task Flee()
        {
            //Not Implemented
            await CombatService.PlayerFlee();
        }
        private void UpdateState(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Monsters" && e.PropertyName != "CurrentPlayer")
            AllMonsters = AppState.CombatMonsters;
            CurrentPlayer = AppState.CurrentPlayer;
            InvokeAsync(StateHasChanged);
        }
        #region Event Handlers


        private async Task HandleCombatEnded(bool isPlayerDead)
        {
            isPlayerDefeated = _combatPlayer.Health <= 0;
            var totalExp = AllMonsters.Values.Sum(x => x.ExpProvided);
            var totalGold = AllMonsters.Values.Sum(x => x.GoldProvided);
            CurrentPlayer = CurrentPlayer.ApplyCombatResults(_combatPlayer);
            string alertTitle = isPlayerDefeated ? "You Lose!" : "You Win!";
            string alertMessage = isPlayerDefeated ? "You get nothing and start over"
                : $"You've received {totalGold}gp and earned {totalExp}xp";

            AppState.UpdateCurrentPlayer(CurrentPlayer);
            Console.WriteLine($"CurrentPlayer Stats: {_combatPlayer}");
            await ModalDialogService.ShowMessageBoxAsync(alertTitle, alertMessage);
          
            await OnCombatEnded.InvokeAsync(!isPlayerDefeated);
          
        }

        private async void HandlePlayerHit(bool isPlayer, string monsterId)
        {
            isPlayerHit = isPlayer;
            if (!monsterId.Contains("Monster"))
                return;
            AllMonsters[monsterId].IsHit = true;
            await Task.Delay(500);
            Console.WriteLine($"HandlePlayerHit \r\n monsterId: {monsterId}");
            await InvokeAsync(StateHasChanged);
        }

        private void OnMonsterHit(string monsterId) => AllMonsters[monsterId].IsHit = false;
        private void OnPlayerHit() => isPlayerHit = false;
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
            Console.WriteLine("TestPage.razor has been disposed");
        }
    }
}
