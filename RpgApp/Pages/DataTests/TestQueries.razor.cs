using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
//using Newtonsoft.Json;
using RpgApp.Shared;
using RpgApp.Shared.Services;
using RpgApp.Shared.StateManager;
using RpgApp.Shared.Types;

namespace RpgApp.Client.Pages.DataTests
{
    public partial class TestQueries
    {
        [CascadingParameter(Name = "AppState")]
        private AppStateManager AppState { get; set; }
        
        [Inject]
        private HttpClient HttpClient { get; set; }

        #region Player Query Region
        private List<Player> UserPlayers { get; set; } = new List<Player>();
        private Player selectedPlayer;
        private string userInput;
        private bool isPlayerQuery = false;

        private async Task GetUserPlayers()
        {
            userInput ??= "admin";
            UserPlayers = await HttpClient.GetFromJsonAsync<List<Player>>($"{AppConstants.ApiUrl}/GetUserPlayers/{userInput}");
            isPlayerQuery = true;
            StateHasChanged();
        }

        private void SelectPlayer(object row)
        {
            if (row == null) return;
            selectedPlayer = UserPlayers.FirstOrDefault(x => x.ID == ((Player)row).ID);
            if (selectedPlayer != null)
            {
                selectedPlayer.Health = selectedPlayer.MaxHealth;
                selectedPlayer.AbilityPoints = selectedPlayer.MaxAbilityPoints;
                AppState.UpdateCurrentPlayer(selectedPlayer);
                //var jsonSetting = new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore};

                //Console.WriteLine(
                //    $"Selected Player: {JsonConvert.SerializeObject(selectedPlayer, Formatting.Indented, jsonSetting)}");
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
            //Expression<Func<Equipment, bool>> equipFilter = equipment => equipment.GoldCost <= value;
            EquipmentQueryResult = await HttpClient.GetFromJsonAsync<List<Equipment>>($"{AppConstants.ApiUrl}/GetEquipment/{value}");
            //var equipmentJson = await apiResponse.Content.ReadAsStringAsync();
            //EquipmentQueryResult = JsonSerializer.Deserialize<List<Equipment>>(equipmentJson);
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
            SkillQueryResult = await HttpClient.GetFromJsonAsync<List<Skill>>($"{AppConstants.ApiUrl}/GetSkills/{value}");
            //var skillJson = await apiResponse.Content.ReadAsStringAsync();
            //SkillQueryResult = JsonSerializer.Deserialize<List<Skill>>(skillJson);
            isSkillQuery = true;
            skillGold = value;
            StateHasChanged();
        }

        #endregion
    }
}
