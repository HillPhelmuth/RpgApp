using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using RpgApp.Shared;
using RpgApp.Shared.Services;
using RpgApp.Shared.Types;

namespace RpgApp.Client.Pages
{
    public partial class Index
    {
        [Inject]
        private HttpClient Http { get; set; }
        [Inject]
        private AuthenticationStateProvider AuthState { get; set; }
        [Inject]
        private ClientDataService ClientDataService { get; set; }
        [Inject]
        private RpgLocalStorageService RpgLocalStorage { get; set; }
       
        private string seedOutput;

        protected int indexTab;
        [CascadingParameter(Name = "AppState")]
        public AppStateManager AppState { get; set; }
        protected override Task OnInitializedAsync()
        {
            AppState.PropertyChanged += UpdateState;
            return base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                seedOutput = await Http.GetStringAsync($"{AppConstants.ApiUrl}/CreateAndSeed");
                Console.WriteLine(seedOutput);
                await InvokeAsync(StateHasChanged);
                
                var appData = new AllAppData();
                (bool hasData, var allAppData) = await RpgLocalStorage.GetAllAppData();
                if (hasData)
                {
                    appData = allAppData;
                    Console.WriteLine("appData retrieved from local storage");
                }
                else
                {
                    appData = await Http.GetFromJsonAsync<AllAppData>($"{AppConstants.ApiUrl}/AllAppData");
                    await RpgLocalStorage.SetAllAppData(appData);
                }

                AppState.AllEquipment = appData?.Equipment ?? new List<Equipment>();
                AppState.AllMonsters = appData?.Monsters ?? new List<Monster>();
                AppState.AllSkills = appData?.Skills ?? new List<Skill>();
                
                var authState = await AuthState.GetAuthenticationStateAsync();
                if (authState.User?.Identity?.IsAuthenticated == true)
                {
                    string user = authState.User?.Identity?.Name;
                    AppState.UserId = user;
                   
                    AppState.UserData = await ClientDataService.GetUserPlayers(user);
                    await InvokeAsync(StateHasChanged);
                }

            }
            await base.OnAfterRenderAsync(firstRender);
        }

        protected async void UpdateState(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(AppState.IndexTab)) return;
            Console.WriteLine($"{e.PropertyName} change handled by {nameof(Index)}");
            indexTab = AppState.IndexTab;
            await InvokeAsync(StateHasChanged);
        }
    }
}
