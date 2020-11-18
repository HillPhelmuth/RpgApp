using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RpgApp.Server.Data;
using RpgApp.Shared.Types;

namespace RpgApp.Server.Services
{
    public class RpgDataService
    {
        private readonly RpgDbContext _context;
       
        public RpgDataService(RpgDbContext context)
        {
            _context = context;
            
        }
       
       
        public async Task<List<Player>> GetUserPlayers(string userId)
        {
            var players = new List<Player>();
            var playersFromDb = await _context.Players.Where(x => x.UserId == userId).Include(x => x.Inventory).ThenInclude(z => z.Effects).Include(x => x.Skills).ThenInclude(z => z.Effects).ToListAsync();
           players.AddRange(playersFromDb);
           return players;
        }
        // Programmatically set the .Where linq expression. See TestQueries.razor.cs for example
        // We can set up component specific query methods where they're required instead of adding a bunch
        // of queries to the service class.
        public async Task<List<Equipment>> GetEquipmentAsync(Expression<Func<Equipment, bool>> filterEquipExpression)
        {
            return await _context.Equipment.Where(filterEquipExpression).Include(x => x.Effects).ToListAsync();
        }

        public async Task<Equipment> GetSingleEquipmentAsync(Expression<Func<Equipment, bool>> firstEquipExpression)
        {
            return await _context.Equipment.Where(firstEquipExpression).Include(x => x.Effects).FirstOrDefaultAsync();
        }

        public async Task<Equipment> GetEquipmentById(int equipId)
        {
            return await _context.Equipment.FindAsync(equipId);
        }
        public async Task<List<Skill>> GetSkillsAsync(Expression<Func<Skill, bool>> filterSkillsExpression)
        {
            return await _context.Skills.Where(filterSkillsExpression).Include(x => x.Effects).ToListAsync();
        }

        public async Task<Skill> GetSingleSkillAsync(Expression<Func<Skill, bool>> firstSkillExpression)
        {
            return await _context.Skills.Where(firstSkillExpression).Include(x => x.Effects).FirstOrDefaultAsync();
        }

        public async Task<Skill> GetSkillById(int skillId)
        {
            return await _context.Skills.FindAsync(skillId);
        }
        public async Task<List<Monster>> GetMonstersAsync(Expression<Func<Monster, bool>> filterMonsters)
        {
            return await _context.Monsters.Where(filterMonsters).ToListAsync();
        }

        public async Task<Monster> GetSingleMonsterAsync(Expression<Func<Monster, bool>> firstMonsterExpression)
        {
            return await _context.Monsters.FirstOrDefaultAsync(firstMonsterExpression);
        }

        public async Task<Monster> GetMonsterById(int monsterId)
        {
            return await _context.Monsters.FindAsync(monsterId);
        }
        public async Task UpdateOrAddPlayer(Player player)
        {
            _context.Players.Update(player);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AddNewEquipment(Equipment equipment)
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

        public async Task<bool> AddNewSkill(Skill skill)
        {
            var isMatch = await _context.Skills.AnyAsync(s =>
                s.Name == skill.Name || s.ID == skill.ID );
            if (isMatch)
                return false;
            await _context.Skills.AddAsync(skill);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddMonster(Monster monster)
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
