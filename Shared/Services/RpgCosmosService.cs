using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Configuration;
using RpgApp.Shared.Types;

namespace RpgApp.Shared.Services
{
    public class RpgCosmosService
    {
       // private readonly HttpClient _client;
        private readonly CosmosClient _cosmosClient;
        //private Database _database;
        public Container PlayerContainer { get; private set; }
        public Container EquipmentContainer { get; private set; }
        public Container SkillContainer { get; private set; }
        public Container MonsterContainer { get; private set; }

        public RpgCosmosService(IConfiguration configuration)
        {
            _cosmosClient = new CosmosClient(configuration["Cosmos:ConnectionString"]);
        }

        public async Task<string> CreateCosmosService()
        {
            DatabaseResponse response = await _cosmosClient.CreateDatabaseIfNotExistsAsync(AppConstants.CosmosDbKey);
            var exists = response.StatusCode == HttpStatusCode.OK;
            
            if (exists)
            {
                EquipmentContainer = _cosmosClient.GetContainer(AppConstants.CosmosDbKey, AppConstants.EquipContainer);
                SkillContainer = _cosmosClient.GetContainer(AppConstants.CosmosDbKey, AppConstants.SkillContainer);
                MonsterContainer = _cosmosClient.GetContainer(AppConstants.CosmosDbKey, AppConstants.MonsterContainer);
                PlayerContainer = _cosmosClient.GetContainer(AppConstants.CosmosDbKey, AppConstants.PlayerContainer);
                return "CosmosDb Containers retrieved";
            }
            Database db = response;
            EquipmentContainer = await db.CreateContainerIfNotExistsAsync(AppConstants.EquipContainer, "/PartitionKey");
            SkillContainer = await db.CreateContainerIfNotExistsAsync(AppConstants.SkillContainer, "/PartitionKey");
            MonsterContainer = await db.CreateContainerIfNotExistsAsync(AppConstants.MonsterContainer, "/Name");
            PlayerContainer = await db.CreateContainerIfNotExistsAsync(AppConstants.PlayerContainer, "/UserId");
            await SeedDatabase();
            return "CosmosDb Containers Created and seeded";


        }

        public async Task SeedDatabase()
        {
           
            foreach (var monster in CosmosInit.CreateMonsters())
            {
                await MonsterContainer.CreateItemAsync(monster);
                Console.WriteLine($"Added monster: {monster.Name}");
            }

            var skills = CosmosInit.AllSkills();
            foreach (var skill in skills)
            {
                await SkillContainer.CreateItemAsync(skill);
                Console.WriteLine($"Added skill: {skill.Name}");
            }

            var equipments = CosmosInit.EquipmentList().Equipments;
            foreach (var item in equipments)
            {
                await EquipmentContainer.CreateItemAsync(item);
                Console.WriteLine($"Added item: {item.Name}");
            }
            var dagger = equipments.Find(x => x.Name == "Dagger");
            var armor = equipments.Find(x => x.Name == "Leather armor");
            var enrage = skills.Find(x => x.Name == "Enrage");
            var magicMissile = skills.Find(x => x.Name == "Magic missile");
            var doubleShot = skills.Find(x => x.Name == "Double shot");
            var warrior = CosmosInit.CreatePlayer("Seed Warrior", dagger, armor, enrage);
            await PlayerContainer.CreateItemAsync(warrior);
            var mage = CosmosInit.CreatePlayer("Seed Mage", dagger, armor, magicMissile);
            await PlayerContainer.CreateItemAsync(mage);
            var ranger = CosmosInit.CreatePlayer("Seed Ranger", dagger, armor, doubleShot);
            await PlayerContainer.CreateItemAsync(ranger);

        }
        public async Task<AllAppData> GetAllAppData()
        {
            var allAppData = new AllAppData();
            var skillIterator = SkillContainer.GetItemLinqQueryable<Skill>().ToFeedIterator();
            while (skillIterator.HasMoreResults)
            {
                var response = await skillIterator.ReadNextAsync();
                allAppData.Skills.AddRange(response.ToList());
            }
            var equipmentIterator = EquipmentContainer.GetItemLinqQueryable<Equipment>().ToFeedIterator();
            while (equipmentIterator.HasMoreResults)
            {
                var response = await equipmentIterator.ReadNextAsync();
                allAppData.Equipment.AddRange(response.ToList());
            }
            var monsterIterator = MonsterContainer.GetItemLinqQueryable<Monster>().ToFeedIterator();
            while (monsterIterator.HasMoreResults)
            {
                var response = await monsterIterator.ReadNextAsync();
                allAppData.Monsters.AddRange(response.ToList());
            }
            
            return allAppData;
        }

        public async Task AddPlayer(Player player)
        {
            Console.WriteLine($"{player.ToString()} added to Db");
            await PlayerContainer.CreateItemAsync<Player>(player);
        }

        public async Task UpdatePlayer(Player player)
        {
            await PlayerContainer.UpsertItemAsync<Player>(player);
        }

        public async Task<List<Player>> GetUserPlayers(string userId)
        {
            var playerIterator = PlayerContainer.GetItemLinqQueryable<Player>()
                .Where(p => p.UserId == userId)
                .ToFeedIterator();
            var players = new List<Player>();
            while (playerIterator.HasMoreResults)
            {
                var response = await playerIterator.ReadNextAsync();
                players.AddRange(response.ToList());
            }

            return players;
        }

        public async Task<List<Equipment>> GetSomeEquipment(int goldMax)
        {
            var equipmentIterator = EquipmentContainer.GetItemLinqQueryable<Equipment>()
                .Where(e => e.GoldCost <= goldMax)
                .ToFeedIterator();
            var equipment = new List<Equipment>();
            while (equipmentIterator.HasMoreResults)
            {
                var equipmentResponse = await equipmentIterator.ReadNextAsync();
                equipment.AddRange(equipmentResponse);
            }

            return equipment;
        }
        public async Task<List<Skill>> GetSomeSkills(int goldMax)
        {
            var skillIterator = EquipmentContainer.GetItemLinqQueryable<Skill>()
                .Where(e => e.GoldCost <= goldMax)
                .ToFeedIterator();
            var skills = new List<Skill>();
            while (skillIterator.HasMoreResults)
            {
                var equipmentResponse = await skillIterator.ReadNextAsync();
                skills.AddRange(equipmentResponse);
            }

            return skills;
        }
    }
}
