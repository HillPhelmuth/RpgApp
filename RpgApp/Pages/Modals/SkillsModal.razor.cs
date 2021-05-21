using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using RpgApp.Shared;
using RpgApp.Shared.Types;
using RpgApp.Shared.Types.Enums;

namespace RpgApp.Client.Pages.Modals
{
    public partial class SkillsModal
    {
        List<Skill> playerSkills = new List<Skill>();
        [Inject]
        AppStateManager AppState { get; set; }
        [Inject]
        private HttpClient HttpClient { get; set; }
        private string PageTitle;

        protected override Task OnInitializedAsync()
        {
            playerSkills = AppState.CurrentPlayer.Skills;
            switch (AppState.CurrentPlayer.ClassType)
            {
                case ClassType.Mage:
                    PageTitle = "Spellbook";
                    break;
                case ClassType.Warrior:
                    PageTitle = "Techniques";
                    break;
                case ClassType.Ranger:
                    PageTitle = "Tricks";
                    break;
                default:
                    PageTitle = "Skills";
                    break;
            }
            return base.OnInitializedAsync();
        }
    }
}
