using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using RpgApp.Shared.Types;

namespace RpgApp.Shared.Services
{
    public class RpgLocalStorageService
    {
        private readonly ILocalStorageService _localStorage;

        public RpgLocalStorageService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<(bool, AllAppData)> GetAllAppData()
        {
            bool hasData = await _localStorage.ContainKeyAsync("AllAppData");
            if (!hasData)
                return (false, new AllAppData());
            var allData = await _localStorage.GetItemAsync<AllAppData>("AllAppData");
            return (true, allData);
        }

        public async Task<(bool, UserData)> GetUserData(string userId)
        {
            bool hasData = await _localStorage.ContainKeyAsync(userId);
            if (!hasData)
                return (false, new UserData());
            var userData = await _localStorage.GetItemAsync<UserData>(userId);
            return (true, userData);
        }
        public async Task UpdateUserData(Player player)
        {
            var userData = await _localStorage.GetItemAsync<UserData>(player.UserId);
            var userPlayers = new List<Player>();
            foreach (var userPlayer in userData?.Players ?? new List<Player>())
            {
                if (userPlayer.ID == player.ID)
                {
                    userPlayers.Add(player);
                    continue;
                }
                userPlayers.Add(userPlayer);
            }

            await _localStorage.SetItemAsync(player.UserId, new UserData { UserName = player.UserId, Players = userPlayers });
        }

        public async Task SetUserData(UserData data)
        {
            await _localStorage.SetItemAsync(data.UserName, data);
        }
        public async Task SetAllAppData(AllAppData appData)
        {
            await _localStorage.SetItemAsync("AllAppData", appData);
        }

    }
}
