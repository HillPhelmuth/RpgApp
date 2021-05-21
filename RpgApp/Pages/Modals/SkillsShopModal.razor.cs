using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using RpgApp.Shared;
using RpgApp.Shared.Types;
using RpgApp.Shared.Types.Enums;

namespace RpgApp.Client.Pages.Modals
{
    public partial class SkillsShopModal
    {
        private List<Skill> shopSkills = new List<Skill>();
        [Inject]
        public AppStateManager AppState { get; set; }
        [Inject]
        private HttpClient HttpClient { get; set; }
        [Inject]
        private ClientDataService ClientDataService { get; set; }
        private string PageTitle;

        protected override async Task OnInitializedAsync()
        {
            switch (AppState.CurrentPlayer.ClassType)
            {
                case ClassType.Mage:
                    shopSkills = AppState.AllSkills.Where(f => f.AllowedClasses.Contains("Mage")).ToList();
                    PageTitle = "Spells Shop";
                    break;
                case ClassType.Warrior:
                    shopSkills = AppState.AllSkills.Where(f => f.AllowedClasses.Contains("Warrior")).ToList();
                    PageTitle = "Warrior Skills";
                    break;
                case ClassType.Ranger:
                    shopSkills = AppState.AllSkills.Where(f => f.AllowedClasses.Contains("Ranger")).ToList();
                    PageTitle = "Ranger Skills";
                    break;
                default:
                    shopSkills = AppState.AllSkills;
                    PageTitle = "Skills";
                    break;
            }
        }
        public async Task BuySkills(Skill skill)
        {
            AppState.CurrentPlayer.Skills.Add(skill);
            await ClientDataService.AddOrUpdatePlayer(AppState.CurrentPlayer);
        }
    }
}
