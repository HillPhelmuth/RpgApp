using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Blazor.ModalDialog;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RpgApp.Client.Shared;
using RpgApp.Shared;
using RpgApp.Shared.Services;
using RpgApp.Shared.Types;
using RpgApp.Shared.Types.Enums;
using RpgApp.Shared.Types.PlayerExtensions; //using Newtonsoft.Json;

namespace RpgApp.Client.Pages
{
    public class TestPageModel : RpgComponentBase, IDisposable
    {
        [Inject]
        public CombatService CombatService { get; set; }
        [Inject]
        private HttpClient Http { get; set; }
        [Inject]
        public IModalDialogService ModalDialogService { get; set; }
       
        protected CombatPlayer _combatPlayer;
        [Parameter]
        public Monster Monster { get; set; } = new Monster();
        [Parameter] 
        public int Difficulty { get; set; } = 1;
        
        protected List<Skill> AllSkillsTemp = new List<Skill>();
        protected Monster _monster;
        protected bool isBeginCombat;
        protected bool isMonsterDead;
        protected bool isPlayerHit;
        protected bool isMonsterHit;
        protected bool isSkillMenu;
        protected bool isPlayerDefeated;
        protected string combatMessages = "";
        protected List<string> messages = new List<string>();
        protected string cssString = "";
        private string playerUrl;

        protected Dictionary<Player, string> CssDictionary { get; set; }
        protected override async Task OnInitializedAsync()
        {
            //await UpdateState();
            // Temporary for testing
            
            _combatPlayer = CurrentPlayer.ConvertToCombatMode();
            cssString = CurrentPlayer.ClassType switch
            {
                ClassType.Warrior => "warrior-img",
                ClassType.Mage => "mage-img",
                ClassType.Ranger => "ranger-img",
                _ => ""
            };
            var dice = new DiceRoller();
            Difficulty = dice.RollD6();
            Expression<Func<Monster, bool>> monsterExpression = monster => monster.DifficultyLevel == Difficulty || monster.DifficultyLevel >= 1;
            var apiResponse = await Http.PostAsJsonAsync($"{AppConstants.ApiUrl}/GetSingleMonster", monsterExpression);
            var monsterJson = await apiResponse.Content.ReadAsStringAsync();
            Monster = JsonSerializer.Deserialize<Monster>(monsterJson);
            //Monster = await RpgData.GetSingleMonsterAsync(monsterExpression);
            _monster = Monster;
            AllSkillsTemp = _combatPlayer.Skills;
            var armor = await Http.GetFromJsonAsync<Equipment>($"{AppConstants.ApiUrl}/GetEquipmentById/1");
            _combatPlayer.EquipArmor(armor);
            await BeginCombat();
            AppState.PropertyChanged += UpdateState;
            CombatService.OnCombatEnded += AlertCombatEnded;
            CombatService.OnNewMessage += HandleNewMessage;
            //CombatService.OnPlayerHit += HandlePlayerHit;
        }

      

        protected async Task BeginCombat()
        {
            isBeginCombat = true;
            var weapons = _combatPlayer?.Inventory ?? new List<Equipment>
            {new Equipment
            {
                EquipLocation = "TwoHands",
                
            }};
            var weapon = weapons.Find(x => x.Effects.Any(e => e.Type == EffectType.Attack));
            _combatPlayer.EquipWeapon(weapon);
            await CombatService.BeginCombat(ref _combatPlayer, ref _monster);
            
            //return Task.CompletedTask;
        }
        protected async Task Attack()
        {
            Console.WriteLine("Attack!");
            
            var dice = _combatPlayer.DamageDice;
            await CombatService.PlayerAttack(dice);
            CurrentPlayer.UpdateDuringCombat(_combatPlayer);
            StateHasChanged();
        }
        protected async Task UseSkill(Skill skill)
        {
            isSkillMenu = false;
            await InvokeAsync(StateHasChanged);
            await CombatService.PlayerUseSkill(skill);
            CurrentPlayer.UpdateDuringCombat(_combatPlayer);
        }
        

        public void OpenMenu(MouseEventArgs e) => isSkillMenu = true;

        protected async Task Flee()
        {
            //Not Implemented
            await CombatService.PlayerFlee();
        }

        #region Event Handlers

        
        protected async Task AlertCombatEnded(bool isPlayerDead)
        {
            isPlayerDefeated = isPlayerDead;
            isMonsterDead = !isPlayerDefeated;
            CurrentPlayer = CurrentPlayer.ApplyCombatResults(_combatPlayer);
            var parameters = new ModalDialogParameters
            {
                {"CurrentPlayer", CurrentPlayer }
            };
            await Task.Delay(2100);
            //var jsonSetting = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            //Console.WriteLine($"CurrentPlayer Stats: {JsonConvert.SerializeObject(CurrentPlayer, Formatting.Indented, jsonSetting)}");
            if (isPlayerDead)
            {
                ModalDialogService.Close(false);
            }
            ModalDialogService.Close(true, parameters);
            
            
            StateHasChanged();

        }
        protected void HandlePlayerHit(bool isPlayer)
        {
            isPlayerHit = isPlayer;
            isMonsterHit = !isPlayerHit;
        }

        protected void OnMonsterHit() => isMonsterHit = false;
        protected void OnPlayerHit() => isPlayerHit = false;
        protected void HandleNewMessage(string message)
        {
            combatMessages += $"<p>{message}</p>";
            messages.Add($"<p>{message}</p>");
            if(messages.Count > 12)
                messages.RemoveAt(0);
            Monster.Health = _monster.Health;
            CurrentPlayer.Health = _combatPlayer.Health;
            CurrentPlayer.AbilityPoints = _combatPlayer.AbilityPoints;
            InvokeAsync(StateHasChanged);
        }
        #endregion


        public void Dispose()
        {
            AppState.PropertyChanged -= UpdateState;
            CombatService.OnCombatEnded -= AlertCombatEnded;
            //CombatService.OnPlayerHit -= HandlePlayerHit;
            CombatService.OnNewMessage -= HandleNewMessage;
            Console.WriteLine("TestPage.razor has been disposed");
        }
    }
}
