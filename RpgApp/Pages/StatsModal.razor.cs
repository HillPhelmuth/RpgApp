using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop;
using TurnBasedRpg.StateManager;
using TurnBasedRpg.Types;
using Blazor.ModalDialog;

namespace TurnBasedRpg.Pages
{
    public partial class StatsModal
    {
        [Inject]
        public AppStateManager AppStateManager { get; set; }
        protected Player CurrentPlayer { get; set; }

        protected override Task OnInitializedAsync()
        {
            CurrentPlayer = AppStateManager.CurrentPlayer;
            AppStateManager.OnChange += UpdateState;
            return base.OnInitializedAsync();
        }
        private Task UpdateState()
        {
            CurrentPlayer = AppStateManager.CurrentPlayer;
            InvokeAsync(StateHasChanged);
            return Task.CompletedTask;
        }
    }
}
