using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using RpgApp.Shared;
using RpgApp.Shared.Services;
using RpgApp.Shared.Types;

namespace RpgApp.Client
{
    public class ClientDataService
    {
        private const string ApiAuthBaseUrl = AppConstants.ApiUrl;//"api/rpgData"; 
        private readonly RpgLocalStorageService _rpgLocalStorage;
        public HttpClient HttpClient { get; }
        public ClientDataService(HttpClient httpClient, RpgLocalStorageService rpgLocalStorage)
        {
            HttpClient = httpClient;
            _rpgLocalStorage = rpgLocalStorage;
        }

        public async Task<string> AddOrUpdatePlayer(Player player)
        {
            try
            {
                var result = await HttpClient.PostAsJsonAsync($"{ApiAuthBaseUrl}/UpdateOrAddPlayer", player);
                await _rpgLocalStorage.UpdateUserData(player);
                Console.WriteLine($"{player} sent to server");
                return result.ToString();
            }
            catch (AccessTokenNotAvailableException ex)
            {
                ex.Redirect();
                return ex.Message;
            }
        }

        public async Task<UserData> GetUserPlayers(string user)
        {
            (bool hasData, var userData) = await _rpgLocalStorage.GetUserData(user);
            if (hasData)
            {
                Console.WriteLine("userData retrieved from local storage");
                return userData;
            }

            try
            {
                var userPlayerData = await HttpClient.GetFromJsonAsync<UserData>($"{ApiAuthBaseUrl}/GetUserPlayers/{user}");
                await _rpgLocalStorage.SetUserData(userPlayerData);
                return userPlayerData;
            }
            catch (AccessTokenNotAvailableException ex)
            {
                ex.Redirect();
                return null;
            }
        }
    }
}
