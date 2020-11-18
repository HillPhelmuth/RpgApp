using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RpgApp.Shared.CheatDevTools
{
    public class DnDApiService
    {
        private readonly IHttpClientFactory _clientFactory;
        private const string ApiBaseUrl = "https://www.dnd5eapi.co";
        public DnDApiService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<GeneralList> GetMonsterList()
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{ApiBaseUrl}/api/monsters");
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Api Error Code: {response.StatusCode} Response: {response.ReasonPhrase}");
                return new GeneralList {Count = 0, GeneralApiData = new List<GeneralApiData> {new GeneralApiData {Name = $"APi Fucked up. They say because: {response.ReasonPhrase}"}}};
            }

            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GeneralList>(responseString);

        }

        public async Task<MonsterData> GetMonster(string monsterUrl)
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{ApiBaseUrl}{monsterUrl}");
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Api Error Code: {response.StatusCode} Response: {response.ReasonPhrase}");
                return new MonsterData {Name = $"Api Error Code: {response.StatusCode} Response: {response.ReasonPhrase}" };
            }

            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Successful Response: {responseString}");
            return JsonConvert.DeserializeObject<MonsterData>(responseString);
        }

        public async Task<List<GeneralApiData>> GetEquipmentGategories()
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{ApiBaseUrl}/api/equipment-categories");
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Api Error Code: {response.StatusCode} Response: {response.ReasonPhrase}");
                return new List<GeneralApiData> { new GeneralApiData { Name = $"APi Fucked up. They say because: {response.ReasonPhrase}" } } ;
            }

            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API return equipment categories: {responseString}");
            var allObjects = JsonConvert.DeserializeObject<GeneralList>(responseString);
            return allObjects.GeneralApiData.Where(x => x.Name == "Weapon" || x.Name == "Armor").ToList();
            //return allObjects.GeneralApiData.Where(x => x.Name.Contains("weapon", StringComparison.InvariantCultureIgnoreCase) || x.Name.Contains("armor", StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public async Task<EquipmentList> GetSelectedList(string categoryUrl)
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{ApiBaseUrl}{categoryUrl}");
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Api Error Code: {response.StatusCode} Response: {response.ReasonPhrase}");
                return new EquipmentList { EquipmentsData = new List<GeneralApiData> { new GeneralApiData { Name = $"APi Fucked up. They say because: {response.ReasonPhrase}" } } };
            }

            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<EquipmentList>(responseString);

        }

        public async Task<WeaponData> GetWeaponData(string weaponUrl)
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{ApiBaseUrl}{weaponUrl}");
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Api Error Code: {response.StatusCode} Response: {response.ReasonPhrase}");
                return new WeaponData { Name = $"Api Error Code: {response.StatusCode} Response: {response.ReasonPhrase}" };
            }

            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Successful Response: {responseString}");
            return JsonConvert.DeserializeObject<WeaponData>(responseString);
        }
        public async Task<ArmorData> GetArmorData(string armorUrl)
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{ApiBaseUrl}{armorUrl}");
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Api Error Code: {response.StatusCode} Response: {response.ReasonPhrase}");
                return new ArmorData { Name = $"Api Error Code: {response.StatusCode} Response: {response.ReasonPhrase}" };
            }

            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Successful Response: {responseString}");
            return JsonConvert.DeserializeObject<ArmorData>(responseString);
        }
    }
}
