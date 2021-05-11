using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using RpgApp.Shared;
using RpgApp.Shared.Types;

namespace RpgApp.Client.Pages.Modals
{
    public partial class StatsModal : IDisposable
    {
        [CascadingParameter(Name = "AppState")]
        public AppStateManager AppState { get; set; }
        protected Player CurrentPlayer { get; set; }

        protected override Task OnInitializedAsync()
        {
            CurrentPlayer = AppState.CurrentPlayer;
            AppState.PropertyChanged += UpdateState;
            return base.OnInitializedAsync();
        }
        private void UpdateState(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(AppState.CurrentPlayer)) return;
            Console.WriteLine($"{e.PropertyName} change handled by {nameof(StatsModal)}");
            CurrentPlayer = AppState.CurrentPlayer;
            InvokeAsync(StateHasChanged);
        }

        public void Dispose() => AppState.PropertyChanged -= UpdateState;
    }
}
