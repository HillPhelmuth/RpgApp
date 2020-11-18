using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using RpgApp.Shared.Types;

namespace RpgApp.Shared.StateManager
{
    /// <summary>
    /// This with simplify the process of keeping all of our page components in sync.
    /// Every time an event occurs that changes any of our Models, we send the change here,
    /// and trigger the Notification method so our components will automatically update with the
    /// changes. (this will make more sense as we go).
    /// </summary>
    public class AppStateManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Player CurrentPlayer { get; set; }

        private (int, int) _playerLocation;
        public (int x, int y) PlayerLocation
        {
            get => _playerLocation;
            set
            {
                _playerLocation = value;
                OnPropertyChanged();
            }
        }

        private int _indexTab;
        public int IndexTab
        {
            get => _indexTab;
            set
            {
                _indexTab = value;
                OnPropertyChanged();
            }
        }

        private Dictionary<int, Monster> _monsters;
        public Dictionary<int, Monster> Monsters
        {
            get => _monsters;
            set
            {
                _monsters = value;
                OnPropertyChanged();
            }
        }

        private string _userId;
        public string UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged();
            }
        }

        public void UpdateCurrentPlayer(Player player)
        {
            CurrentPlayer = player;
            OnPropertyChanged(nameof(CurrentPlayer));
        }

        public void AddItemToInventory(Equipment equipment)
        {
            CurrentPlayer?.AddToInventory(equipment);
            OnPropertyChanged(nameof(CurrentPlayer));
        }
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
