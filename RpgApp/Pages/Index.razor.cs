using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
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
        private AuthHttpClient AuthClient { get; set; }
        [Inject]
        private RpgLocalStorageService rpgLocalStorage { get; set; }


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
                var appData = new AllAppData();
                (bool hasData, var allAppData) = await rpgLocalStorage.GetAllAppData();
                if (hasData)
                {
                    appData = allAppData;
                    Console.WriteLine("appData retrieved from local storage");
                }
                else
                {
                    appData = await Http.GetFromJsonAsync<AllAppData>($"{AppConstants.ApiUrl}/AllAppData");
                    await rpgLocalStorage.SetAllAppData(appData);
                }

                AppState.AllEquipment = appData?.Equipment ?? new List<Equipment>();
                AppState.AllMonsters = appData?.Monsters ?? new List<Monster>();
                AppState.AllSkills = appData?.Skills ?? new List<Skill>();
                
                var authState = await AuthState.GetAuthenticationStateAsync();
                if (authState.User?.Identity?.IsAuthenticated == true)
                {
                    string user = authState.User?.Identity?.Name;
                    AppState.UserId = user;
                    
                    //(bool hasUserData, var userData) = await rpgLocalStorage.GetUserData(user);
                    //if (hasUserData)
                    //{
                    //    data = userData;
                    //    Console.WriteLine("userData retrieved from local storage");
                    //}
                    //else
                    //{
                    //    data = await Http.GetFromJsonAsync<UserData>($"{AppConstants.ApiUrl}/GetUserPlayers/{user}");
                    //    await rpgLocalStorage.SetUserData(data ?? new UserData {UserName = user});
                    //}
                    AppState.UserData = await AuthClient.GetUserPlayers(user);
                    await InvokeAsync(StateHasChanged);
                }

            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private List<string> authCallMessages = new();
        private async Task TestAuthCalls()
        {
            authCallMessages.Add($"Starting Api calls using the named httpclient ({nameof(AuthClient)}) calls to auth controller");
            await InvokeAsync(StateHasChanged);
            string addOrUpdate = await AuthClient.AddOrUpdatePlayer(AppState.CurrentPlayer);
            authCallMessages.Add($"AddOrUpdatePlayer returned {addOrUpdate}");
            await InvokeAsync(StateHasChanged);
            var getPlayers = await AuthClient.GetUserPlayers(AppState.UserId ?? "");
            authCallMessages.Add($"GetUserPlayers call for {getPlayers.UserName}-- {string.Join(';', getPlayers.Players.Select(p => p.Name))}");
            await InvokeAsync(StateHasChanged);
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
