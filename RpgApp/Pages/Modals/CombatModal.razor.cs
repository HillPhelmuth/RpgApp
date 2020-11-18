using System.Threading.Tasks;

namespace RpgApp.Client.Pages.Modals
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
