using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using RpgApp.Shared;
using RpgApp.Shared.CheatDevTools;
using RpgApp.Shared.Services;
using RpgApp.Shared.Types;

namespace RpgApp.Client.Pages.DevTools
{
    public partial class CreateMonster
    {
        [Inject]
        public DnDApiService DnDApi { get; set; }
        //[Inject]
        //public RpgDataService RpgDataService { get; set; }
        [Inject]
        private HttpClient Http { get; set; }
        private List<GeneralApiData> AllMonsters { get; set; }
        private MonsterData MonsterData { get; set; } = new MonsterData();
        private CreateMonsterForm MonsterForm { get; set; } = new CreateMonsterForm();
        private bool isShowMontsters { get; set; }
        private string submitSucess = "";

        protected override async Task OnInitializedAsync()
        {
            var monsterList = await DnDApi.GetMonsterList();
            AllMonsters = monsterList.GeneralApiData;

        }

        private async void GetMonsterDetails(object dataRow)
        {
            var monster = (GeneralApiData)dataRow;
            MonsterData = await DnDApi.GetMonster(monster.Url);
            var newMonsterForm = new CreateMonsterForm
            {
                Name = MonsterData.Name,
                MaxHealth = MonsterData.HitPoints,
                Agility = MonsterData.Dexterity - 5,
                Strength = MonsterData.Strength - 5,
                Toughness = MonsterData.Constitution - 5,
                Intelligence = MonsterData.Intelligence - 5,
                Speed = (int)Math.Round(MonsterData.ChallengeRating, MidpointRounding.AwayFromZero) + 10,
                DifficultyLevel = (int)Math.Round(MonsterData.ChallengeRating, MidpointRounding.AwayFromZero)
            };
            MonsterForm = newMonsterForm;
            StateHasChanged();
        }

        private async Task SubmitMonsterForm()
        {
            var monster = new Monster
            {
                Name = MonsterForm.Name,
                MaxHealth = MonsterForm.MaxHealth,
                Health = MonsterForm.MaxHealth,
                DifficultyLevel = MonsterForm.DifficultyLevel,
                Description = MonsterForm.Description,
                Agility = MonsterForm.Agility,
                Intelligence = MonsterForm.Intelligence,
                Strength = MonsterForm.Strength,
                Speed = MonsterForm.Speed,
                Toughness = MonsterForm.Toughness,
                DamageDice = MonsterForm.DamageDice

            };
            var isSuccess = await Http.PostAsJsonAsync($"{AppConstants.ApiUrl}/AddMonster", monster); /*RpgDataService.AddMonster(monster);*/
            submitSucess = isSuccess.IsSuccessStatusCode ? "Monster successfully added to Db" : "Nope! a monster with this Name and Difficulty already exists in the Database";
        }

        private void ClearForm()
        {
            MonsterForm = new CreateMonsterForm();
            submitSucess = "";
        }
    }
}
