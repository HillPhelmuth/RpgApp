using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blazor.ModalDialog;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;
using TurnBasedRpg.Services;
using TurnBasedRpg.StateManager;
using TurnBasedRpg.Types;
using TurnBasedRpg.Types.Enums;
using TurnBasedRpg.Types.PlayerExtensions;

namespace TurnBasedRpg.Pages.CombatTests
{
    public partial class MultiMonster
    {
        [Inject]
        public CombatService CombatService { get; set; }
        [Inject]
        public RpgDataService RpgData { get; set; }
        [Inject]
        public IModalDialogService ModalDialogService { get; set; }
        [Inject]
        public AppStateManager AppStateManager { get; set; }
        public Player CurrentPlayer { get; set; }

        private CombatPlayer _combatPlayer;
        [Parameter]
        public Monster Monster { get; set; } = new Monster();
        [Parameter]
        public int Difficulty { get; set; }

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

            } }

        public Dictionary<int, Monster> AllMonsters = new Dictionary<int, Monster>();
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
            await UpdateState();
            // Temporary for testing

            _combatPlayer = CurrentPlayer.ConvertToCombatMode();
            cssString = CurrentPlayer.ClassType switch
            {
                ClassType.Warrior => "warrior-img",
                ClassType.Mage => "mage-img",
                ClassType.Ranger => "ranger-img",
                _ => ""
            };
            
            Expression<Func<Monster, bool>> monsterExpression = monster => monster.DifficultyLevel == 1;
            var monsters = await RpgData.GetMonstersAsync(monsterExpression);

            AllMonsters = new Dictionary<int, Monster>();
            for (int i = 1; i <= MonsterCount; i++)
            {
                AllMonsters.Add(i, monsters[i-1]);
            }
           
            AllSkillsTemp = _combatPlayer.Skills.Select(x => x.Skill).ToList();
            _combatPlayer.EquipArmor(await RpgData.GetEquipmentById(1));
            await BeginCombat();
            AppStateManager.OnChange += UpdateState;
            CombatService.OnCombatEnded += HandleCombatEnded;
            CombatService.OnNewMessage += HandleNewMessage;
            CombatService.OnPlayerHit += HandlePlayerHit;
        }

        private async Task BeginCombat()
        {
            isBeginCombat = true;
            var weapons = _combatPlayer?.Inventory?.Select(x => x.Equipment) ??
                          new List<Equipment> { new Equipment {EquipLocation = "TwoHands" }};
            var weapon = weapons.FirstOrDefault(x => x.Effects.Any(e => e.Type == EffectType.Attack));
            _combatPlayer.EquipWeapon(weapon);
            await CombatService.BeginCombat(ref _combatPlayer, ref AllMonsters);

        }
        private async Task Attack(int key)
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
        private Task UpdateState()
        {
            // get and assign values from AppStateManager to Shared Properties
            CurrentPlayer = AppStateManager.CurrentPlayer;
            return InvokeAsync(StateHasChanged);
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

            await AppStateManager.UpdateCurrentPlayer(CurrentPlayer);
            Console.WriteLine($"CurrentPlayer Stats: {_combatPlayer}");
           // await ModalDialogService.ShowMessageBoxAsync(alertTitle, alertMessage);
            if (isPlayerDefeated)
            {
                ModalDialogService.Close(false);
                return;
            }
            ModalDialogService.Close(true);

            await InvokeAsync(StateHasChanged);
        }

        private async void HandlePlayerHit(bool isPlayer, int monsterId)
        {
            isPlayerHit = isPlayer;
            if (monsterId == 0)
                return;
            AllMonsters[monsterId].IsHit = true;
            await Task.Delay(500);
            Console.WriteLine($"HandlePlayerHit \r\n monsterId: {monsterId}");
            await InvokeAsync(StateHasChanged);
        }

        private void OnMonsterHit(int monsterId) => AllMonsters[monsterId].IsHit = false;
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
            AppStateManager.OnChange -= UpdateState;
            CombatService.OnCombatEnded -= HandleCombatEnded;
            CombatService.OnPlayerHit -= HandlePlayerHit;
            CombatService.OnNewMessage -= HandleNewMessage;
            Console.WriteLine("TestPage.razor has been disposed");
        }
    }
}
