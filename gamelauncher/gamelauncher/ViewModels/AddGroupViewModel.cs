using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using gamelauncher.Model;
using gamelauncher.MVVM;

namespace gamelauncher.ViewModels
{
    public class AddGroupViewModel : INotifyPropertyChanged
    {
        public event Action close;
        public event Action GroupAdded;
        private string _input;

        public string Input
        {
            get => _input;
            set
            {
                _input = value;
                OnPropertyChanged();
            }
        }

        public ICommand CloseCommand { get; }
        public ICommand AddGroupCommand { get; }
        public AddGroupViewModel(Action closeAction, Action AddGroup)
        {
            CloseCommand = new RelayCommand(_ => closeAction());
            AddGroupCommand = new RelayCommand(_ => AddNewGroup(), _ => CanAddGroup());
            GroupAdded += AddGroup;
            close = closeAction;
        }

        private void AddNewGroup()
        {
            if (!string.IsNullOrWhiteSpace(Input))
            {
                DataWorker.CreateGroupForUser(Input);
                GroupAdded?.Invoke();
                close.Invoke();
            }
        }

        private bool CanAddGroup()
        {
            return !string.IsNullOrWhiteSpace(Input);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
