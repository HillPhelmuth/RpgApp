using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using RpgApp.Shared;
using RpgApp.Shared.Types;

namespace RpgApp.Client.Shared
{
    public class RpgComponentBase : ComponentBase
    {
        #region SharedComponentProperties

        [Inject]
        public AppStateManager AppState { get; set; }
        //[Inject]
        //public DiceRoller DiceRoller { get; set; }
        public Player CurrentPlayer { get; set; }


        #endregion

        #region SharedComponentMethods
        // Task that's triggered when NotifyStateHasChanged() is called in the AppStateManager
        protected void UpdateState(object sender, PropertyChangedEventArgs e)
        {
            // get and assign values from AppStateManager to Shared Properties
            CurrentPlayer = AppState.CurrentPlayer;
            InvokeAsync(StateHasChanged);
        }

        #endregion
    }
}
