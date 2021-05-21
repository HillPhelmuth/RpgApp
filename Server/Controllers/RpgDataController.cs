﻿using System;
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
        private readonly  RpgAppDbContext _context;
        public RpgDataController(IDbContextFactory<RpgAppDbContext> contextFactory)
        {
            _context = contextFactory.CreateDbContext();
        }

        [HttpGet("CreateAndSeed")]
        public async Task<string> CreateAndSeedCosmos()
        {
           var dbCreatetext = RpgDbInitializer.Initialize(_context);
           return dbCreatetext;

        }
        [HttpGet("GetUserPlayers/{userId}")]
        public async Task<IActionResult> GetUserPlayers(string userId)
        {
            var players = new List<Player>();
            var playersFromDb = await _context.Players.Where(x => x.UserId == userId).ToListAsync();
            players.AddRange(playersFromDb);
            //return players;
            return new OkObjectResult(new UserData { UserName = userId, Players = players });
        }

        [HttpGet("AllAppData")]
        public async Task<AllAppData> GetAllAppData()
        {
            return new AllAppData
            {
                Equipment = await _context.Equipment.ToListAsync(),
                Skills = await _context.Skills.ToListAsync(),
                Monsters = await _context.Monsters.ToListAsync()
            };
        }
        [HttpGet("GetEquipment")]
        public async Task<List<Equipment>> GetEquipmentAsync()
        {
            return await _context.Equipment.ToListAsync();
        }
        [HttpGet("GetSomeEquipment")]
        public async Task<List<Equipment>> GetSomeEquipmentAsync([FromQuery] int goldMax = 0)
        {
            Expression<Func<Equipment, bool>> firstEquipExpression = equip => equip.GoldCost < goldMax;
            return await _context.Equipment.Where(firstEquipExpression).ToListAsync();
        }
        [HttpGet("GetEquipmentById/{equipId}")]
        public async Task<Equipment> GetEquipmentById(int equipId)
        {
            return await _context.Equipment.FindAsync(equipId);
        }
        [HttpGet("GetSkills")]
        public async Task<List<Skill>> GetSkillsAsync()
        {
            return await _context.Skills.ToListAsync();
        }
        [HttpGet("GetSomeSkills")]
        public async Task<List<Skill>> GetSomeSkillsAsync([FromQuery] int goldMax = 0)
        {
            Expression<Func<Skill, bool>> firstEquipExpression = equip => equip.GoldCost < goldMax;
            return await _context.Skills.Where(firstEquipExpression).ToListAsync();
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
            var trackedPlayer = await _context.Players.WithPartitionKey(player.UserId).FirstOrDefaultAsync(x => x.Name == player.Name);
            if (trackedPlayer == null)
            {
                player.Skills = new List<Skill>();
                player.Inventory = new List<Equipment>();
                await _context.Players.AddAsync(player);
                player.Skills.AddRange(skills.Select(x => _context.Skills.FirstOrDefault(y => y.Name == x.Name)));
                player.Inventory.AddRange(inventory.Select(x => _context.Equipment.FirstOrDefault(y => y.Name == x.Name)));
            }
            else
            {
                var newSkills = skills.Where(s => !trackedPlayer.Skills.Contains(s)).ToList();
                trackedPlayer.Skills.AddRange(newSkills);
                var newEq = inventory.Where(e => !trackedPlayer.Inventory.Contains(e)).ToList();
                trackedPlayer.Inventory.AddRange(newEq);
                trackedPlayer.Experience = exp;
                trackedPlayer.Gold = gold;
            }

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
                s.Name == skill.Name || s.Index == skill.Index);
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
            var skills = player.Skills?.Distinct().ToList() ?? new List<Skill>();
            var dbSkills = _context.Skills.ToList().Where(x => skills.Any(y => x.Index == y.Index)).ToList();
            var inventory = player.Inventory?.Distinct().ToList() ?? new List<Equipment>();
            var dbInventory = _context.Equipment.ToList().Where(e1 => inventory.Any(e2 => e2.Index == e1.Index)).ToList();
            int exp = player.Experience;
            int gold = player.Gold;
            var trackedPlayer = await _context.Players.WithPartitionKey(player.UserId).FirstOrDefaultAsync(x => x.Name == player.Name);
            if (trackedPlayer == null)
            {
                player.Skills = new List<Skill>();
                player.Inventory = new List<Equipment>();
                await _context.Players.AddAsync(player);
                player.Skills.AddRange(skills.Select(x => _context.Skills.FirstOrDefault(y => y.Name == x.Name)));
                player.Inventory.AddRange(inventory.Select(x => _context.Equipment.FirstOrDefault(y => y.Name == x.Name)));
            }
            else
            {
                var newSkills = dbSkills.Where(s => !trackedPlayer.Skills.Contains(s)).ToList();
                trackedPlayer.Skills.AddRange(newSkills);
                var newEq = dbInventory.Where(e => !trackedPlayer.Inventory.Contains(e)).ToList();
                trackedPlayer.Inventory.AddRange(newEq);
                trackedPlayer.Experience = exp;
                trackedPlayer.Gold = gold;
            }

            await _context.SaveChangesAsync();
        }
    }
}
