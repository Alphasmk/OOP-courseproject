using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using gamelauncher.Model;
using System.Windows.Input;
using System.ComponentModel;
using gamelauncher.MVVM;
using System.Collections.ObjectModel;

namespace gamelauncher.ViewModels
{
    public class AddToGroupViewModel : INotifyPropertyChanged
    {
        private int _gameId;
        private List<UserGameGroup> _groups;
        private UserGameGroup _selectedGroup;
        private bool _isSelected;

        public UserGameGroup SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                _selectedGroup = value;
                if (_selectedGroup != null)
                {
                    IsSelected = true;
                }
                else
                {
                    IsSelected = false;
                }
                OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
        public List<UserGameGroup> Groups
        {
            get => _groups;
            set
            {
                _groups = value;
                OnPropertyChanged();
            }
        }

        public ICommand CloseCommand { get; }
        public ICommand AddToGroupCommand { get; }

        private readonly Action _closeAction;
        private readonly Action _refreshCallback;
        public AddToGroupViewModel(Action closeAction, Action refreshCallback, int id)
        {
            CloseCommand = new RelayCommand(_ => closeAction());
            AddToGroupCommand = new RelayCommand(_ => AddToGroup(), _ => !(SelectedGroup == null));
            _closeAction = closeAction;
            _refreshCallback = refreshCallback;
            _gameId = id;
            Groups = DataWorker.GetAllUserGroups();
        }

        private void AddToGroup()
        {
            DataWorker.AddGameToGroupById(SelectedGroup.Id, _gameId);
            _refreshCallback?.Invoke();
            _closeAction.Invoke();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
