using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RpgApp.Shared.Services;
using RpgApp.Shared.Types;

namespace RpgApp.Server.Controllers
{
    [Route("api/rpgCosmos")]
    [ApiController]
    public class RpgCosmosController : ControllerBase
    {
        private readonly RpgCosmosService cosmosService;

        public RpgCosmosController(RpgCosmosService cosmosService)
        {
            this.cosmosService = cosmosService;
        }

        [HttpGet("CreateAndSeed")]
        public async Task<string> CreateAndSeedCosmos()
        {
            try
            {
                string resultMessage = await cosmosService.CreateCosmosService();
               
                Console.WriteLine("Success");
                return resultMessage;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message}\r\n{ex.StackTrace}");
                return $"{ex.Message}\r\n{ex.StackTrace}";
            }
        }
        [HttpGet("GetUserPlayers/{userId}")]
        public async Task<IActionResult> GetUserPlayers(string userId)
        {
            var players = await cosmosService.GetUserPlayers(userId);
            return new OkObjectResult(new UserData{UserName = userId, Players = players});
        }

        [HttpGet("AllAppData")]
        public async Task<AllAppData> GetAllAppData()
        {
            return await cosmosService.GetAllAppData();
        }

        [HttpPost("UpdateOrAddPlayer")]
        public async Task UpdateOrAddPlayer([FromBody] Player player)
        {
            if (player.Index == 0)
            {
                player.Index = new Random().Next(1, 999999);
                await cosmosService.AddPlayer(player);
            }
            else
            {
                await cosmosService.UpdatePlayer(player);
            }
        }
        [HttpGet("GetSomeEquipment")]
        public async Task<List<Equipment>> GetSomeEquipmentAsync([FromQuery] int goldMax = 0)
        {
            return await cosmosService.GetSomeEquipment(goldMax);
        }
        [HttpGet("GetSomeSkills")]
        public async Task<List<Skill>> GetSomeSkillsAsync([FromQuery] int goldMax = 0)
        {
            return await cosmosService.GetSomeSkills(goldMax);
        }
    }
}
