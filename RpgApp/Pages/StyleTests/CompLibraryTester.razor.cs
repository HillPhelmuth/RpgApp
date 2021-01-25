using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using RpgApp.Shared;
using RpgApp.Shared.Types;
using RpgApp.Shared.Types.PlayerExtensions;

namespace RpgApp.Client.Pages.StyleTests
{
    public partial class CompLibraryTester
    {
        [Inject]
        private AppStateManager AppState { get; set; }
        private double life;
        private double mana;
        private double stamina;
        private bool showProgress = true;
        private bool showMenu = true;
        private List<KeyValuePair<string, Equipment>> _imagesEquipPairs = new();
        private List<KeyValuePair<string, Skill>> _imagesSkillPairs = new();
        private List<string> TestLog = new();
        protected override Task OnInitializedAsync()
        {
            _imagesEquipPairs = AddImages(AppState.AllEquipment);
            _imagesSkillPairs = AddImages(AppState.AllSkills);
            return base.OnInitializedAsync();
        }

        private void AddToLog(string info)
        {
            TestLog.Add(info);
            StateHasChanged();
        }
        private List<KeyValuePair<string, Equipment>> AddImages(List<Equipment> equipment)
        {
            return equipment.Select(eq => eq.AddImagePath()).ToList();
        }

        private List<KeyValuePair<string, Skill>> AddImages(List<Skill> skills)
        {
            return skills.Select(sk => sk.AddImagePath()).ToList();
        }
        private async Task UpdateBar(string meterNmae)
        {
            var random = new Random();
            double newVal = random.Next(1, 101);
            switch (meterNmae)
            {
                case "Life":
                    life = newVal;
                    break;
                case "Mana":
                    mana = newVal;
                    break;
                case "Stamina":
                    stamina = newVal;
                    break;
            }

            await InvokeAsync(StateHasChanged);
        }

       
    }
}
