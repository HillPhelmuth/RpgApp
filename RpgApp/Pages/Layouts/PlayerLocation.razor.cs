using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blazor.ModalDialog;
using Microsoft.AspNetCore.Components;
using TurnBasedRpg.StateManager;

namespace TurnBasedRpg.Pages.Layouts
{
    public partial class PlayerLocation
    {
        [Inject]
        public AppStateManager AppStateManager { get; set; }
        [Inject]
        protected IModalDialogService ModalDialogService { get; set; }
        [Parameter]
        public int Col { get; set; }
        [Parameter]
        public int Row { get; set; }
        
        protected (int, int) PlayerLoc { get; set; }

        [Parameter] 
        public EventCallback<(int, int)> PlayerLocChanged { get; set; }
        private string cssLocation = "";
        private bool isOccupied;

        protected override Task OnInitializedAsync()
        {
            
            AppStateManager.OnMove += UpdateLocation;
            return base.OnInitializedAsync();
        }

        private async void UpdateLocation()
        {
            PlayerLoc = AppStateManager.PlayerLocation;

            var sw = new Stopwatch();
            sw.Start();
            (int row, int col) = PlayerLoc;

            if (row == Row && col == Col)
            {
                cssLocation = "located";
                isOccupied = true;
                sw.Stop();
                Console.WriteLine($"Update Location took {sw.ElapsedMilliseconds}ms");
                await PlayerLocChanged.InvokeAsync(PlayerLoc);
            }
            else
            {
                cssLocation = "";
                isOccupied = false;
            }
            
            await InvokeAsync(StateHasChanged);
        }

        private async Task TryModifyModal()
        {
            var result = await ModalDialogService.ShowDialogAsync<StyleTester>("this");
            if (result.Success)
                ModalDialogService.Close(true);
        }
    }
}
