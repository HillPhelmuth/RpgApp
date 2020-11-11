using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using TurnBasedRpg.Services;
using TurnBasedRpg.StateManager;
using TurnBasedRpg.Types;

namespace TurnBasedRpg.Pages.DataTests
{
    public partial class TestQueries
    {
        [Inject]
        private AppStateManager AppStateManager { get; set; }
        [Inject]
        private RpgDataService RpgData { get; set; }


        #region Player Query Region
        private List<Player> UserPlayers { get; set; } = new List<Player>();
        private Player selectedPlayer;
        private string userInput;
        private bool isPlayerQuery = false;

        private async Task GetUserPlayers()
        {
            userInput ??= "admin";
            UserPlayers = await RpgData.GetUserPlayers(userInput);
            isPlayerQuery = true;
            StateHasChanged();
        }

        private async void SelectPlayer(object row)
        {
            if (row == null) return;
            selectedPlayer = UserPlayers.FirstOrDefault(x => x.ID == ((Player)row).ID);
            if (selectedPlayer != null)
            {
                selectedPlayer.Health = selectedPlayer.MaxHealth;
                selectedPlayer.AbilityPoints = selectedPlayer.MaxAbilityPoints;
                await AppStateManager.UpdateCurrentPlayer(selectedPlayer);
                var jsonSetting = new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore};

                Console.WriteLine(
                    $"Selected Player: {JsonConvert.SerializeObject(selectedPlayer, Formatting.Indented, jsonSetting)}");
            }

            StateHasChanged();
        }
        #endregion

        #region Equipment Queries
        private List<Equipment> EquipmentQueryResult { get; set; } = new List<Equipment>();
        private int equipGold;
        private bool isEquipQuery = false;
        public async void GetEquipFilterByGp(int value)
        {
            Expression<Func<Equipment, bool>> equipFilter = equipment => equipment.GoldCost <= value;
            EquipmentQueryResult = await RpgData.GetEquipmentAsync(equipFilter);
            isEquipQuery = true;
            equipGold = value;
            StateHasChanged();
        }

        #endregion

        #region Skills Query
        private List<Skill> SkillQueryResult { get; set; } = new List<Skill>();
        private int skillGold;
        private bool isSkillQuery = false;
        public async void GetSkillFilterByGp(int value)
        {
            Expression<Func<Skill, bool>> skillFilter = skill => skill.GoldCost <= value;
            SkillQueryResult = await RpgData.GetSkillsAsync(skillFilter);
            isSkillQuery = true;
            skillGold = value;
            StateHasChanged();
        }

        #endregion
    }
}
