﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RpgApp.Server.Data;
using RpgApp.Server.Services;
using RpgApp.Shared.Types;

namespace RpgApp.Server.Controllers
{
    [Route("api/rpgData")]
    [ApiController]
    public class RpgDataController : ControllerBase
    {
        private readonly RpgDbContext _context;
        public RpgDataController(RpgDbContext context)
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
            return new OkObjectResult(players);
        }
        [HttpGet("GetEquipment/{goldMax}")]
        public async Task<List<Equipment>> GetEquipmentAsync(int goldMax)
        {
            Expression<Func<Equipment, bool>> filterEquipExpression = equip => equip.GoldCost < goldMax;
            return await _context.Equipment.Where(filterEquipExpression).Include(x => x.Effects).AsSplitQuery().ToListAsync();
        }
        [HttpGet("GetSingleEquipment/{goldMax}")]
        public async Task<Equipment> GetSingleEquipmentAsync(int goldMax)
        {
            Expression<Func<Equipment, bool>> firstEquipExpression = equip => equip.GoldCost < goldMax;
            return await _context.Equipment.Where(firstEquipExpression).Include(x => x.Effects).AsSplitQuery().FirstOrDefaultAsync();
        }
        [HttpGet("GetEquipmentById/{equipId}")]
        public async Task<Equipment> GetEquipmentById(int equipId)
        {
            return await _context.Equipment.FindAsync(equipId);
        }
        [HttpGet("GetSkills/{goldMax}")]
        public async Task<List<Skill>> GetSkillsAsync(int goldMax)
        {
            Expression<Func<Skill, bool>> filterSkillsExpression = skill => skill.GoldCost < goldMax;
            return await _context.Skills.Where(filterSkillsExpression).Include(x => x.Effects).AsSplitQuery().ToListAsync();
        }
        [HttpGet("GetSingleSkill/{goldMax}")]
        public async Task<Skill> GetSingleSkillAsync(int goldMax)
        {
            Expression<Func<Skill, bool>> firstSkillExpression = skill => skill.GoldCost < goldMax;
            return await _context.Skills.Where(firstSkillExpression).Include(x => x.Effects).AsSplitQuery().FirstOrDefaultAsync();
        }
        [HttpGet("GetSkillById/{skillId}")]
        public async Task<Skill> GetSkillById(int skillId)
        {
            return await _context.Skills.FindAsync(skillId);
        }
        [HttpGet("GetMonsters/{maxLevel}")]
        public async Task<List<Monster>> GetMonstersAsync(int maxLevel)
        {
            Expression<Func<Monster, bool>> filterMonsters = skill => skill.DifficultyLevel < maxLevel;
            return await _context.Monsters.Where(filterMonsters).ToListAsync();
        }
        [HttpGet("GetSingleMonsters/{maxLevel}")]
        public async Task<Monster> GetSingleMonsterAsync(int maxLevel)
        {
            Expression<Func<Monster, bool>> firstMonsterExpression = skill => skill.DifficultyLevel < maxLevel;
            return await _context.Monsters.FirstOrDefaultAsync(firstMonsterExpression);
        }
        [HttpGet("GetSkillById{monsterId}")]
        public async Task<Monster> GetMonsterById(int monsterId)
        {
            return await _context.Monsters.FindAsync(monsterId);
        }
        [HttpPost("UpdateOrAddPlayer")]
        public async Task UpdateOrAddPlayer([FromBody] Player player)
        {
            _context.Players.Update(player);
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
}
