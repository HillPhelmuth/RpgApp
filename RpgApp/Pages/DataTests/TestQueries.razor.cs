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
        private UserData UserData { get; set; } = new() {UserName = "admin", Players = new List<Player>()};
        private Player selectedPlayer;
        private string userInput;
        private bool isPlayerQuery = false;

        private async Task GetUserPlayers()
        {
            userInput ??= string.IsNullOrEmpty(AppState.UserId) ? "admin": AppState.UserId;
            UserData = await HttpClient.GetFromJsonAsync<UserData>($"{AppConstants.ApiUrl}/GetUserPlayers/{userInput}");
            isPlayerQuery = true;
            StateHasChanged();
        }

        private void SelectPlayer(Player player)
        {
            if (player == null) return;
            selectedPlayer = UserData.Players.Find(x => x.ID == player.ID);
            if (selectedPlayer != null)
            {
                selectedPlayer.Health = selectedPlayer.MaxHealth;
                selectedPlayer.AbilityPoints = selectedPlayer.MaxAbilityPoints;
                AppState.UpdateCurrentPlayer(selectedPlayer);
                Console.WriteLine($"Player: Id: {selectedPlayer.UserId} Name: {selectedPlayer.Name}");
            }

            StateHasChanged();
        }
        #endregion

        #region Equipment Queries
        private List<Equipment> EquipmentQueryResult { get; set; } = new();
        private int equipGold;
        private bool isEquipQuery = false;
        public async void GetEquipFilterByGp(double value)
        {
            EquipmentQueryResult = await HttpClient.GetFromJsonAsync<List<Equipment>>($"{AppConstants.ApiUrl}/GetSomeEquipment?goldMax={(int)value}");
            isEquipQuery = true;
            equipGold = (int)value;
            StateHasChanged();
        }

        #endregion

        #region Skills Query
        private List<Skill> SkillQueryResult { get; set; } = new();
        private int skillGold;
        private bool isSkillQuery = false;
        public async void GetSkillFilterByGp(double value)
        {
            SkillQueryResult = await HttpClient.GetFromJsonAsync<List<Skill>>($"{AppConstants.ApiUrl}/GetSomeSkills?goldMax={(int)value}");
            isSkillQuery = true;
            skillGold = (int)value;
            StateHasChanged();
        }

        #endregion
    }
}
