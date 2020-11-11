using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TurnBasedRpg.Pages
{
    public partial class CombatModal
    {
        protected bool IsCombatReady;
        protected bool IsPlayerCreated;
        protected string name;
        public Task BeginCombat()
        {
            IsPlayerCreated = false;
            IsCombatReady = true;
            return Task.CompletedTask;
        }
        protected void Reset()
        {
            name = "";
            IsPlayerCreated = false;
        }
    }
}
