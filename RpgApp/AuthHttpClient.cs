using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using RpgApp.Shared;
using RpgApp.Shared.Types;

namespace RpgApp.Client
{
    public class AuthHttpClient
    {
        private const string ApiAuthBaseUrl = "api/rpgAuthData";
        public HttpClient HttpClient { get; }

        public AuthHttpClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<string> AddOrUpdatePlayer(Player player)
        {
            try
            {
                var result = await HttpClient.PostAsJsonAsync($"{ApiAuthBaseUrl}/UpdateOrAddPlayer", player);
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
            try
            {
                return await HttpClient.GetFromJsonAsync<UserData>($"{ApiAuthBaseUrl}/GetUserPlayers/{user}");
            }
            catch (AccessTokenNotAvailableException ex)
            {
                ex.Redirect();
                throw;
            }
        }
    }
}
