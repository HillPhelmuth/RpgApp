using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using RpgApp.Shared;
using RpgApp.Shared.Types; //using Newtonsoft.Json;

namespace RpgApp.Client.Pages.DataTests
{
    public partial class TestQueries
    {
        [CascadingParameter(Name = "AppState")]
        private AppStateManager AppState { get; set; }

        [Inject]
        private HttpClient HttpClient { get; set; }

        #region Player Query Region
        private List<Player> UserPlayers { get; set; } = new();
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
            selectedPlayer = UserPlayers.Find(x => x.ID == ((Player)row).ID);
            if (selectedPlayer != null)
            {
                selectedPlayer.Health = selectedPlayer.MaxHealth;
                selectedPlayer.AbilityPoints = selectedPlayer.MaxAbilityPoints;
                AppState.UpdateCurrentPlayer(selectedPlayer);
                Console.WriteLine($"Player: {JsonSerializer.Serialize(selectedPlayer)}");
                //var jsonSetting = new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore};

                //Console.WriteLine(
                //    $"Selected Player: {JsonConvert.SerializeObject(selectedPlayer, Formatting.Indented, jsonSetting)}");
            }

            StateHasChanged();
        }
        #endregion

        #region Equipment Queries
        private List<Equipment> EquipmentQueryResult { get; set; } = new();
        private int equipGold;
        private bool isEquipQuery = false;
        public async void GetEquipFilterByGp(int value)
        {
            EquipmentQueryResult = await HttpClient.GetFromJsonAsync<List<Equipment>>($"{AppConstants.ApiUrl}/GetSomeEquipment?goldMax={value}");
            isEquipQuery = true;
            equipGold = value;
            StateHasChanged();
        }

        #endregion

        #region Skills Query
        private List<Skill> SkillQueryResult { get; set; } = new();
        private int skillGold;
        private bool isSkillQuery = false;
        public async void GetSkillFilterByGp(int value)
        {
            SkillQueryResult = await HttpClient.GetFromJsonAsync<List<Skill>>($"{AppConstants.ApiUrl}/GetSomeSkills?goldMax={value}");
            isSkillQuery = true;
            skillGold = value;
            StateHasChanged();
        }

        #endregion
    }
}
