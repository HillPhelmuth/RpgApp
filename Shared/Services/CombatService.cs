using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;
using TurnBasedRpg.Services.ExtensionMethods;
using TurnBasedRpg.Types;
using TurnBasedRpg.Types.Enums;
using TurnBasedRpg.Types.PlayerExtensions;

namespace TurnBasedRpg.Services
{
    public class CombatService
    {
        public event Func<bool, Task> OnCombatEnded;
        public event Action<string> OnNewMessage;
        public event Action<bool, int> OnPlayerHit;
        
        private CombatPlayer _combatPlayer;
        public CombatPlayer CurrentPlayer { get; private set; }
        //private Monster _monster;
        private Dictionary<int, Monster> _allMonsters = new Dictionary<int, Monster>();

        private bool _isActiveCombat;
        
        public Task BeginCombat(ref CombatPlayer player, ref Monster monster)
        {
            _combatPlayer = null;
            CurrentPlayer = null;
           // _monster = null;
            _combatPlayer = player;
            //_monster = monster;
            //_monster.Health = _monster.MaxHealth;
            _isActiveCombat = true;
            NotifyNewMessage("Combat has begun");
            return EvaluatePlayerTurn();
        }

        #region BeginCombat Overloads

        // Overload for Two monsters
        public Task BeginCombat(ref CombatPlayer player, ref Dictionary<int, Monster> allMonsters)
        {
            _allMonsters = allMonsters;
            _combatPlayer = null;
            CurrentPlayer = null;
            //_monster = null;
            _combatPlayer = player;
            //_monster = monster;
            foreach ((int key, var _monster) in _allMonsters)
            {
                _monster.Health = _monster.MaxHealth;
                _monster.isDead = false;
                _monster.IsHit = false;
                _monster.Initiative = 0;
            }
            _isActiveCombat = true;
            NotifyNewMessage("Multi Combat has begun");
            return EvaluatePlayerTurn();
        }
       
        #endregion

        public async Task PlayerAttack(string damageDice = "1D6", int targetId = 0)
        {
            var random = new Random();
            if (targetId == 0) targetId = random.Next(1, _allMonsters.Count + 1);
            if (!_isActiveCombat/* || _monster == null*/) return;
            if (!_allMonsters.ContainsKey(targetId))
            {
                await NotifyNewMessage($"Monster {targetId} is Dead");
                return;
            }

            int key = targetId;
            
            _combatPlayer.Initiative = 0;
            
            var modifier = _combatPlayer.GetModifier();
            var damageDealt = damageDice.ToUpper().ToDiceValue() + (int)modifier - _allMonsters[targetId].Armor;
            _allMonsters[targetId].Health -= Math.Max(1, damageDealt);

            if (_allMonsters.Values.Any(x => x.Health >= 0))
            {
                await NotifyNewMessage($"You dealt {damageDealt} Damage");
                await NotifyPlayerHit(false, targetId);
                await Task.Delay(500);
                await EvaluatePlayerTurn();
                return;
            }
            else
            {
                _combatPlayer.Experience = _allMonsters.Values.Sum(x => x.ExpProvided);
                _combatPlayer.Gold = _allMonsters.Values.Sum(x => x.GoldProvided);
                await NotifyCombatEnded(false);
                await NotifyNewMessage(
                    $"monster {string.Join(", ", _allMonsters.Values.Select(x => x.Name))} have been killed");
                _isActiveCombat = false;
                return;
            }
        }

        public async Task PlayerUseSkill(Skill skill)
        {
            // check the skill for ability points and reduce ability points accordingly
            if (skill.AbilityCost > _combatPlayer.AbilityPoints)
            {
                await NotifyNewMessage("You do not have the required Ability points to use this ability");
                return;
            }

            _combatPlayer.Initiative = 0;
            _combatPlayer.AbilityPoints -= skill.AbilityCost;
            // check the skill Effect property and execute based on type of skill
            var skillType = skill.Effects.Select(x => x.Type).FirstOrDefault();
            var skillValue = skill.Effects.FirstOrDefault()?.Value;
            var skillAttrib = skill.Effects.FirstOrDefault()?.Attribute;
            // Creates task variable and assigns it to a Task method based on skill type
            Task skillTask = skillType switch
            {
                EffectType.Attack => PlayerAttack(skillValue),
                EffectType.Defend => Defend(skillValue),
                EffectType.Heal => Heal(skillValue),
                EffectType.ModifySelf => Modify(skillValue, skillAttrib, true),
                EffectType.Modify => Modify(skillValue, skillAttrib),
                _ => PlayerAttack()
            };
            
            
            await NotifyNewMessage($"You Attempt {skill.Name} is completed");
            await skillTask;
        }

        private Task Heal(string val)
        {
            _combatPlayer.Health += val.ToDiceValue();
            NotifyPlayerHit(true);
            return NotifyNewMessage($"Player healed");
        }

        private Task Defend(string val)
        {
            var value = int.TryParse(val, out int modifier);
            _combatPlayer.ArmorModifier = modifier;
            NotifyPlayerHit(true);
            return NotifyNewMessage($"Armor increased by {modifier}");
        }

        private Task Modify(string val, string attrib, bool isSelf = false)
        {
            if (isSelf)
            {
                _combatPlayer.ModifyAttribute(attrib, val);
                NotifyPlayerHit(true);
                NotifyNewMessage($"Your {attrib}+{val}");
                return EvaluatePlayerTurn();
            }
            var random = new Random();
            var keyVal = random.Next(1, 4);
            _allMonsters[keyVal].ModifyAttribute(attrib, val);
            NotifyPlayerHit(false);
            NotifyNewMessage($"Enemy {attrib}-{val}");
            return EvaluatePlayerTurn();

        }
        public Task PlayerFlee()
        {
            // Create Run await logic
            return NotifyNewMessage("You attempted to run away like a little girl, but you can't you pussy");
        }
        private async Task MonsterAttack(int monsterKey)
        {
            if (!_isActiveCombat || _combatPlayer == null) return;
            var monster = _allMonsters[monsterKey];
            monster.Initiative = 0;
            var damageTotal = monster.DamageDice.ToUpper().ToDiceValue() - _combatPlayer.ArmorValue;
            var damage = Math.Max(damageTotal, 1);
            _combatPlayer.Health -= damage;

            if (_combatPlayer.Health > 0)
            {
                await NotifyNewMessage($"Monster hits for {damage}");
                await NotifyPlayerHit(true);
                await Task.Delay(500);
                await EvaluatePlayerTurn();
                return;
            }

            _isActiveCombat = false;
            await NotifyCombatEnded(true);
        }
        private Task EvaluatePlayerTurn()
        {
            var isPlayerReady = false;
            if (_allMonsters.Values.All(x => x.Health <= 0))
                return NotifyCombatEnded(false);
            while (!isPlayerReady)
            {
                _combatPlayer.Initiative += _combatPlayer.Speed;
                foreach (var monster in _allMonsters.Where(x => x.Value.Health >= 0))
                {
                    monster.Value.Initiative += monster.Value.Speed;
                    
                }
                //_monster.Initiative += _monster.Speed;
                if (_combatPlayer.Initiative > 99) isPlayerReady = true;
                //if (_monster.Initiative > 99) isPlayerReady = true;
                if (_allMonsters.Values.Any(x => x.Initiative > 99)) isPlayerReady = true;
            }

            (int key, var topMonster) = _allMonsters.OrderByDescending(x => x.Value.Initiative).FirstOrDefault();
            return _combatPlayer.Initiative >= topMonster.Initiative ? NotifyNewMessage("Player Turn...") : MonsterAttack(key);
        }

        private async Task NotifyCombatEnded(bool isPlayer)
        {
            if (OnCombatEnded != null) await OnCombatEnded.Invoke(isPlayer);
        }

        private Task NotifyNewMessage(string str)
        {
            OnNewMessage?.Invoke(str);
            return Task.CompletedTask;
        }

        private Task NotifyPlayerHit(bool isPlayer, int monsterKey = 0)
        {
            OnPlayerHit?.Invoke(isPlayer, monsterKey);
            return Task.CompletedTask;
        }
    }
   
}
