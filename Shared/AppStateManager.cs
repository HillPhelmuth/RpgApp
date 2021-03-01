using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RpgApp.Shared.Types;

namespace RpgApp.Shared
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

        private Dictionary<string, Monster> _combatMonsters;
        public Dictionary<string, Monster> CombatMonsters
        {
            get => _combatMonsters;
            set
            {
                _combatMonsters = value;
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

        private List<Monster> _allMonsters;

        public List<Monster> AllMonsters
        {
            get => _allMonsters;
            set
            {
                _allMonsters = value;
                OnPropertyChanged();
            }
        }

        private List<Skill> _allSkills;

        public List<Skill> AllSkills
        {
            get => _allSkills;
            set
            {
                _allSkills = value;
                OnPropertyChanged();
            }
        }

        private List<Equipment> _allEquipment;

        public List<Equipment> AllEquipment
        {
            get => _allEquipment;
            set
            {
                _allEquipment = value;
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
