using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using TurnBasedRpg.Services;
using TurnBasedRpg.StateManager;
using TurnBasedRpg.Types;

namespace TurnBasedRpg.Shared
{
    public class RpgComponentBase : ComponentBase
    {
        #region SharedComponentProperties

        [Inject]
        public AppStateManager AppStateManager { get; set; }
        [Inject]
        public DiceRoller DiceRoller { get; set; }
        public Player CurrentPlayer { get; set; }
        

        #endregion

        #region SharedComponentMethods
        // Task that's triggered when NotifyStateHasChanged() is called in the AppStateManager
        protected Task UpdateState()
        {
            // get and assign values from AppStateManager to Shared Properties
            CurrentPlayer = AppStateManager.CurrentPlayer;
            return InvokeAsync(StateHasChanged);
        }

        #endregion
    }
}
