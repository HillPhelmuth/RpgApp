using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpgApp.Server.Data;
using RpgApp.Shared.Types; //using RpgApp.Server.Services;

namespace RpgApp.Server.Controllers
{
    [Route("api/rpgData")]
    [ApiController]
    //[Authorize]
    public class RpgDataController : ControllerBase
    {
        private readonly RpgAppDbContext _context;
        public RpgDataController(RpgAppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetUserPlayers/{userId}")]
        public async Task<IActionResult> GetUserPlayers(string userId)
        {
            var players = new List<Player>();
            var playersFromDb = await _context.Players.Where(x => x.UserId == userId).Include(x => x.Inventory).ThenInclude(z => z.Effects).Include(x => x.Skills).ThenInclude(z => z.Effects).ToListAsync();
            players.AddRange(playersFromDb);
            //return players;
            return new OkObjectResult(new UserData { UserName = userId, Players = players });
        }

        [HttpGet("AllAppData")]
        public async Task<AllAppData> GetAllAppData()
        {
            return new AllAppData
            {
                Equipment = await _context.Equipment.Include(x => x.Effects).AsSplitQuery().ToListAsync(),
                Skills = await _context.Skills.Include(x => x.Effects).AsSplitQuery().ToListAsync(),
                Monsters = await _context.Monsters.ToListAsync()
            };
        }
        [HttpGet("GetEquipment")]
        public async Task<List<Equipment>> GetEquipmentAsync()
        {
            return await _context.Equipment.Include(x => x.Effects).AsSplitQuery().ToListAsync();
        }
        [HttpGet("GetSomeEquipment")]
        public async Task<List<Equipment>> GetSomeEquipmentAsync([FromQuery] int goldMax = 0)
        {
            Expression<Func<Equipment, bool>> firstEquipExpression = equip => equip.GoldCost < goldMax;
            return await _context.Equipment.Where(firstEquipExpression).Include(x => x.Effects).AsSplitQuery().ToListAsync();
        }
        [HttpGet("GetEquipmentById/{equipId}")]
        public async Task<Equipment> GetEquipmentById(int equipId)
        {
            return await _context.Equipment.FindAsync(equipId);
        }
        [HttpGet("GetSkills")]
        public async Task<List<Skill>> GetSkillsAsync()
        {
            return await _context.Skills.Include(x => x.Effects).AsSplitQuery().ToListAsync();
        }
        [HttpGet("GetSomeSkills")]
        public async Task<List<Skill>> GetSomeSkillsAsync([FromQuery] int goldMax = 0)
        {
            Expression<Func<Skill, bool>> firstEquipExpression = equip => equip.GoldCost < goldMax;
            return await _context.Skills.Where(firstEquipExpression).Include(x => x.Effects).AsSplitQuery().ToListAsync();
        }

        [HttpGet("GetSkillById/{skillId}")]
        public async Task<Skill> GetSkillById(int skillId)
        {
            return await _context.Skills.FindAsync(skillId);
        }
        [HttpGet("GetMonsters")]
        public async Task<List<Monster>> GetMonstersAsync()
        {
            return await _context.Monsters.ToListAsync();
        }
        [HttpGet("GetSingleMonsters/{maxLevel}")]
        public async Task<Monster> GetSingleMonsterAsync(int maxLevel)
        {
            Expression<Func<Monster, bool>> firstMonsterExpression = skill => skill.DifficultyLevel < maxLevel;
            return await _context.Monsters.FirstOrDefaultAsync(firstMonsterExpression);
        }
        [HttpGet("GetMonsterById{monsterId}")]
        public async Task<Monster> GetMonsterById(int monsterId)
        {
            return await _context.Monsters.FindAsync(monsterId);
        }
        [HttpPost("UpdateOrAddPlayer")]
        public async Task UpdateOrAddPlayer([FromBody] Player player)
        {
            // Make sure we don't accidently add duplicates to the Db
            var skills = player.Skills?.Distinct().ToList() ?? new List<Skill>();
            var inventory = player.Inventory?.Distinct().ToList() ?? new List<Equipment>();
            var exp = player.Experience;
            var gold = player.Gold;
            if (player.ID == 0)
            {
                
                player.Skills = new List<Skill>();
                player.Inventory = new List<Equipment>();
                await _context.Players.AddAsync(player);
                player.Skills.AddRange(skills.Select(x => _context.Skills.FirstOrDefault(y => y.Name == x.Name)));
                player.Inventory.AddRange(inventory.Select(x => _context.Equipment.FirstOrDefault(y => y.Name == x.Name)));
            }

            var untrackedPlayer = await _context.Players.FindAsync(player.ID);
            untrackedPlayer.Experience = exp;
            untrackedPlayer.Gold = gold;
            untrackedPlayer.Skills = skills;
            untrackedPlayer.Inventory = inventory;
            //_context.Players.Update(player);
            await _context.SaveChangesAsync();
        }
        [HttpPost("AddNewEquipment")]
        public async Task<bool> AddNewEquipment([FromBody] Equipment equipment)
        {
            var isMatch = await _context.Equipment.AnyAsync(s =>
                s.Name == equipment.Name);
            if (isMatch)
            {
                Console.WriteLine($"{equipment.Name} Already exists in DB");
                return false;
            }

            await _context.Equipment.AddAsync(equipment);
            await _context.SaveChangesAsync();
            return true;
        }
        [HttpPost("AddNewSkill")]
        public async Task<bool> AddNewSkill([FromBody] Skill skill)
        {
            var isMatch = await _context.Skills.AnyAsync(s =>
                s.Name == skill.Name || s.ID == skill.ID);
            if (isMatch)
                return false;
            await _context.Skills.AddAsync(skill);
            await _context.SaveChangesAsync();
            return true;
        }
        [HttpPost("AddMonster")]
        public async Task<bool> AddMonster([FromBody] Monster monster)
        {
            var isMatch = await _context.Monsters.AnyAsync(m =>
                m.Name == monster.Name && m.DifficultyLevel == monster.DifficultyLevel);
            if (isMatch)
                return false;
            await _context.Monsters.AddAsync(monster);
            await _context.SaveChangesAsync();
            return true;
        }
    }
    [Route("api/rpgAuthData")]
    [ApiController]
    [Authorize]
    public class RpgAuthDataController : ControllerBase
    {
        private readonly RpgAppDbContext _context;

        public RpgAuthDataController(RpgAppDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetUserPlayers/{userId}")]
        public async Task<IActionResult> GetUserPlayers(string userId)
        {
            var players = new List<Player>();
            var playersFromDb = await _context.Players.Where(x => x.UserId == userId).Include(x => x.Inventory).ThenInclude(z => z.Effects).Include(x => x.Skills).ThenInclude(z => z.Effects).ToListAsync();
            players.AddRange(playersFromDb);
            //return players;
            return new OkObjectResult(new UserData { UserName = userId, Players = players });
        }
        [HttpPost("UpdateOrAddPlayer")]
        public async Task UpdateOrAddPlayer([FromBody] Player player)
        {
            // Make sure we don't accidently add duplicates to the Db
            var skills = player.Skills?.Distinct().ToList() ?? new List<Skill>();
            var inventory = player.Inventory?.Distinct().ToList() ?? new List<Equipment>();
            int exp = player.Experience;
            int gold = player.Gold;
            if (player.ID == 0)
            {

                player.Skills = new List<Skill>();
                player.Inventory = new List<Equipment>();
                await _context.Players.AddAsync(player);
                player.Skills.AddRange(skills.Select(x => _context.Skills.FirstOrDefault(y => y.Name == x.Name)));
                player.Inventory.AddRange(inventory.Select(x => _context.Equipment.FirstOrDefault(y => y.Name == x.Name)));
            }

            var trackedPlayer = await _context.Players.FindAsync(player.ID);
            trackedPlayer.Experience = exp;
            trackedPlayer.Gold = gold;
            trackedPlayer.Skills = skills;
            trackedPlayer.Inventory = inventory;
            //_context.Players.Update(player);
            await _context.SaveChangesAsync();
        }
    }
}
