using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using gamelauncher.Model;
using gamelauncher.MVVM;
using gamelauncher.Views;
using Microsoft.Identity.Client;

namespace gamelauncher.ViewModels
{
    public class AdminPanelUsersViewModel : INotifyPropertyChanged
    {
        private List<User> _users;
        private User _selectedUser;
        public List<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public AdminPanelUsersViewModel()
        {
            RefreshCommand = new RelayCommand(_ => LoadUsers());
            DeleteCommand = new RelayCommand(_ => DeleteUser(), _ => CanDeleteUser());
            EditCommand = new RelayCommand(_ => EditUser(), _ => CanDeleteUser());

            LoadUsers();
        }

        private void LoadUsers()
        {
            Users = DataWorker.GetAllUsers().ToList();
        }

        private bool CanDeleteUser()
        {
            if (SelectedUser != null)
            {
                if(CurrentUser.Instance == null || SelectedUser.Email == CurrentUser.Instance.Email)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        private void DeleteUser()
        {
            if (SelectedUser != null)
            {
                if (DataWorker.DeleteUser(SelectedUser))
                {
                    LoadUsers();
                }
            }
        }

        private void EditUser()
        {

            var editpage = new EditUser(SelectedUser);
            editpage.ShowDialog();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
