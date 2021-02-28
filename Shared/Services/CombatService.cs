using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpgApp.Shared.Services.ExtensionMethods;
using RpgApp.Shared.Types;
using RpgApp.Shared.Types.Enums;
using RpgApp.Shared.Types.PlayerExtensions;

namespace RpgApp.Shared.Services
{
    public class CombatService
    {
        public event Func<bool, Task> OnCombatEnded;
        public event Action<string> OnNewMessage;
        public event Action<bool, string> OnPlayerHit;
        public event Action<string> OnMonsterAttack;

        private CombatPlayer _combatPlayer = new();

        //private Monster _monster;
        private Dictionary<string, Monster> _allMonsters = new();

        private bool _isActiveCombat;

        #region BeginCombat Overloads

        public Task BeginCombat(CombatPlayer player, Dictionary<string, Monster> monsters)
        {
            _allMonsters = monsters;
            _combatPlayer = null;
            _combatPlayer = player;
            foreach ((string key, var monster) in _allMonsters)
            {
                monster.Health = monster.MaxHealth;
                monster.isDead = false;
                monster.IsHit = false;
                monster.Initiative = 0;
            }
            _isActiveCombat = true;
            NotifyNewMessage("Multi Combat has begun");
            return EvaluatePlayerTurn();
        }

        #endregion

        public async Task PlayerAttack(string damageDice = "1D4", string targetId = "")
        {
            var random = new Random();
            _combatPlayer.Initiative = 0;
            if (string.IsNullOrWhiteSpace(targetId))
            {
                targetId = $"Monster {random.Next(1, _allMonsters.Count(x => !x.Value.isDead) + 1)}";
                await NotifyNewMessage($"Attacking random monster: {targetId}");
            }
            if (!_isActiveCombat) return;
            if (!_allMonsters.ContainsKey(targetId))
            {
                await NotifyNewMessage($"{targetId} Does not appear to exist. Weird...");
                return;
            }

            if (_allMonsters[targetId].isDead)
            {
                await NotifyNewMessage($"{targetId} is dead already, dummy");
                return;
            }

            (decimal modifier, int damageDealt) = DealDamage(damageDice, targetId);
            Console.WriteLine($"modifier = {modifier}\r\ndamageDice = {damageDice}");
            Console.WriteLine($"damage: {damageDealt}, minus monster armor: {_allMonsters[targetId].Armor}");
            var messageTask = NotifyNewMessage($"You dealt {damageDealt} Damage");
            var playerHitTask = NotifyPlayerHit(false, targetId);
            await Task.WhenAll(messageTask, playerHitTask);

            if (_allMonsters.Values.All(x => x.isDead))
            {
                await NotifyNewMessage(
                    $"monster {string.Join(", ", _allMonsters.Values.Select(x => x.Name))} have been killed");
            }
            //await Task.Delay(500);
            await EvaluatePlayerTurn();

        }


        private (decimal modifier, int damageDealt) DealDamage(string damageDice, string targetId)
        {
            var modifier = _combatPlayer.GetModifier();

            var damageDealt = damageDice.ToUpper().ToDiceValue() + (int)modifier - _allMonsters[targetId].Armor;
            _allMonsters[targetId].Health -= Math.Max(1, damageDealt);

            foreach (var monster in _allMonsters.Where(monster => monster.Value.Health <= 0))
            {
                monster.Value.isDead = true;
            }

            return (modifier, damageDealt);
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
            var effect = skill.Effects.FirstOrDefault() ?? new Effect();
            var skillType = effect.Type;
            string skillValue = effect.Value;
            string skillAttrib = effect.Attribute;
            var notify = NotifyNewMessage($"Skill {skill.Name} is completed");
            // Creates task variable 'skillType' and assigns it to a Task method based on skill type Enum
            await Task.WhenAll(notify, skillType switch
            {
                EffectType.Attack => PlayerSkillAttack(effect, skillValue),
                EffectType.Defend => Defend(skillValue),
                EffectType.Heal => Heal(skillValue),
                EffectType.ModifySelf => Modify(skillValue, skillAttrib, true),
                EffectType.Modify => Modify(skillValue, skillAttrib),
                _ => PlayerAttack()
            });

        }

        #region Skill Tasks

        private async Task PlayerSkillAttack(Effect effect, string damageDice = "1D4")
        {
            if (_allMonsters.All(x => x.Value.isDead))
            {
                await NotifyNewMessage($"monsters {string.Join(", ", _allMonsters.Values.Select(x => x.Name))} have been killed");
                return;
            }

            var random = new Random();
            int targets = Math.Max(effect.Area ?? 0, 1);
            for (int i = 0; i < targets; i++)
            {
                string targetId = $"Monster {random.Next(1, _allMonsters.Count(x => !x.Value.isDead) + 1)}";
                while (_allMonsters[targetId].isDead)
                {
                    targetId = $"Monster {random.Next(1, _allMonsters.Count(x => !x.Value.isDead) + 1)}";
                }
                await NotifyNewMessage($"Attacking random monster: {targetId}");

                (decimal modifier, int damageDealt) = DealDamage(damageDice, targetId);
                Console.WriteLine($"modifier = {modifier}\r\ndamageDice = {damageDice}");
                Console.WriteLine($"damage: {damageDealt}, minus monster armor: {_allMonsters[targetId].Armor}");
                var messageTask = NotifyNewMessage($"You dealt {damageDealt} Damage");
                var playerHitTask = NotifyPlayerHit(false, targetId);
                await Task.WhenAll(messageTask, playerHitTask);
            }
            
            if (_allMonsters.Values.All(x => x.isDead))
            {
                await NotifyNewMessage(
                    $"monster {string.Join(", ", _allMonsters.Values.Select(x => x.Name))} have been killed");
            }
            await EvaluatePlayerTurn();
        }
        private async Task Heal(string val)
        {
            _combatPlayer.Health += val.ToDiceValue();
            var hitTask = NotifyPlayerHit(true);
            var messageTask = NotifyNewMessage("Player healed");
            await Task.WhenAll(hitTask, messageTask);
            await EvaluatePlayerTurn();
        }

        private async Task Defend(string val)
        {
            var value = int.TryParse(val, out int modifier);
            _combatPlayer.ArmorModifier = modifier;
            var hitTask = NotifyPlayerHit(true);
            var messageTask = NotifyNewMessage($"Armor increased by {modifier}");
            await Task.WhenAll(hitTask, messageTask);
            await EvaluatePlayerTurn();
        }

        private async Task Modify(string val, string attrib, bool isSelf = false)
        {
            if (isSelf)
            {
                _combatPlayer.ModifyAttribute(attrib, val);
                var isHit = NotifyPlayerHit(true);
                var message = NotifyNewMessage($"Your {attrib}+{val}");
                await Task.WhenAll(isHit, message);
                await EvaluatePlayerTurn();
                return;
            }
            var random = new Random();
            var keyVal = random.Next(1, 4);
            _allMonsters[$"Monster {keyVal}"].ModifyAttribute(attrib, val);
            var hitTask = NotifyPlayerHit(false);
            var messageTask = NotifyNewMessage($"Enemy {attrib}-{val}");
            await Task.WhenAll(hitTask, messageTask);
            await EvaluatePlayerTurn();

        }

        #endregion

        public Task PlayerFlee()
        {
            // Create Run await logic
            return NotifyNewMessage("You attempted to run away like a little girl, but you can't you pussy");
        }
        private async Task MonsterAttack(string monsterKey)
        {
            if (!_isActiveCombat || _combatPlayer == null) return;
            var monster = _allMonsters[monsterKey];
            monster.Initiative = 0;
            var damageTotal = monster.DamageDice.ToUpper().ToDiceValue() - _combatPlayer.ArmorValue;
            var damage = Math.Max(damageTotal, 1);
            _combatPlayer.Health -= damage;
            var message = NotifyNewMessage($"Monster hits for {damage}");
            var hit = NotifyPlayerHit(true);
            var notifyAttack = NotifyMonsterAttack(monsterKey);
            await Task.WhenAll(notifyAttack, message, hit);
            //await Task.Delay(500);
            await EvaluatePlayerTurn();

        }
        private Task EvaluatePlayerTurn()
        {
            bool isPlayerReady = false;
            foreach (var monster in _allMonsters.Where(m => m.Value.Health <= 0))
            {
                monster.Value.isDead = true;
            }
            if (_allMonsters.Values.All(x => x.isDead))
            {

                return NotifyCombatEnded(false);
            }
            if (_combatPlayer.Health <= 0)
            {
                //_isActiveCombat = false;
                return NotifyCombatEnded(true);
            }

            while (!isPlayerReady)
            {
                _combatPlayer.Initiative += _combatPlayer.Speed;
                foreach (var monster in _allMonsters.Where(x => !x.Value.isDead))
                {
                    monster.Value.Initiative += monster.Value.Speed;

                }
                //_monster.Initiative += _monster.Speed;
                if (_combatPlayer.Initiative > 99) isPlayerReady = true;

                if (_allMonsters.Values.Any(x => x.Initiative > 99)) isPlayerReady = true;
            }

            (string key, var topMonster) = _allMonsters.OrderByDescending(x => x.Value.Initiative).FirstOrDefault();
            return _combatPlayer.Initiative >= topMonster.Initiative ? NotifyNewMessage("Player Turn...") : MonsterAttack(key);
        }

        private async Task NotifyCombatEnded(bool isPlayer)
        {
            if (OnCombatEnded != null && _isActiveCombat)
                await OnCombatEnded.Invoke(isPlayer);
            Console.WriteLine($"OnCombatEnded Invoked, isActiveCombat == {_isActiveCombat}");
            _isActiveCombat = false;
        }

        private Task NotifyNewMessage(string str)
        {
            OnNewMessage?.Invoke(str);
            return Task.CompletedTask;
        }

        private Task NotifyPlayerHit(bool isPlayer, string monsterKey = "")
        {
            if (!_isActiveCombat) return Task.CompletedTask;
            OnPlayerHit?.Invoke(isPlayer, monsterKey);
            Console.WriteLine("OnPlayerHit Invoked");
            return Task.CompletedTask;
        }

        private Task NotifyMonsterAttack(string monsterKey)
        {
            if (!_isActiveCombat) return Task.CompletedTask;
            OnMonsterAttack?.Invoke(monsterKey);
            Console.WriteLine($"OnMonsterAttack({monsterKey}) Invoked");
            return Task.CompletedTask;
        }
        #region old BeginCombat Signiture
        [Obsolete(message:"This is the old method. Use the overload BeginCombat(CombatPlayer player, Dictionary<string, Monster> monsters)")]
        public Task BeginCombat(ref CombatPlayer player, ref Monster monster)
        {
            _combatPlayer = null;
            // _monster = null;
            _combatPlayer = player;
            //_monster = monster;
            //_monster.Health = _monster.MaxHealth;
            _isActiveCombat = true;
            NotifyNewMessage("Combat has begun");
            return EvaluatePlayerTurn();
        }
        #endregion

    }

}
