using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        private UserData _userData;
        public UserData UserData
        {
            get => _userData;
            set
            {
                if (_userData == value) return;
                _userData = value;
                OnPropertyChanged();
            }
        }

        private (int, int) _playerLocation;
        public (int x, int y) PlayerLocation
        {
            get => _playerLocation;
            set
            {
                if (_playerLocation == value) return;
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
                if (_indexTab == value) return;
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
                if (_combatMonsters == value) return;
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
                if (_userId == value) return;
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
                if (_allMonsters == value) return;
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
                if (_allSkills == value) return;
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
                if (_allEquipment == value) return;
                _allEquipment = value;
                OnPropertyChanged();
            }
        }
        public void UpdateCurrentPlayer(Player player)
        {
            CurrentPlayer = player;
            OnPropertyChanged(nameof(CurrentPlayer));
            if (string.IsNullOrEmpty(UserId) || UserData == null) return;
            if (UserData.Players.Any(p => p.UserId == player.UserId && p.Name == player.Name)) return;
            UserData.Players.Add(player);
            OnPropertyChanged(nameof(UserData));
        }

        public void AddItemToInventory(Equipment equipment)
        {
            CurrentPlayer?.AddToInventory(equipment);
            OnPropertyChanged(nameof(CurrentPlayer));
        }
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Console.WriteLine($"OnPropertyChanged invoked for {propertyName}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
