using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TurnBasedRpg.Types;

namespace TurnBasedRpg.StateManager
{
    /// <summary>
    /// This with simplify the process of keeping all of our page components in sync.
    /// Every time an event occurs that changes any of our Models, we send the change here,
    /// and trigger the Notification method so our components will automatically update with the
    /// changes. (this will make more sense as we go).
    /// </summary>
    public class AppStateManager
    {
        public Player CurrentPlayer { get; set; }
        public (int,int) PlayerLocation { get; set; }
        public int IndexTab { get; private set; }
        // Event for model state changes
        public event Func<Task> OnChange;
        public Action OnMove;
        public Action OnTabChange;

        public void UpdateTabChanged(int tab)
        {
            IndexTab = tab;
        }
        public void UpdatePlayerLocation((int row, int col) location)
        {
            PlayerLocation = location;
            NotifyPlayerMoved();
        }
        public async Task UpdateCurrentPlayer(Player player)
        {
            CurrentPlayer = player;
            await NotifyStateChanged();
        }

        public async Task AddItemToInventory(Equipment equipment)
        {
            CurrentPlayer?.Inventory?.Add(new PlayerEquipment {Equipment = equipment});
            await NotifyStateChanged();
        }
        // Task to notify components when model states change
        private async Task NotifyStateChanged()
        {
            if (OnChange != null) await OnChange.Invoke();
        }

        private void NotifyPlayerMoved() => OnMove?.Invoke();
        private void NotifyUpdateTab() => OnTabChange?.Invoke();
    }
}
