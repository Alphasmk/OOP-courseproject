using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using gamelauncher.MVVM;
using gamelauncher.Model;
using Microsoft.EntityFrameworkCore.Diagnostics;
using gamelauncher.Views;
using System.Text.RegularExpressions;

namespace gamelauncher.ViewModels
{
    public class EditUserViewModel : INotifyPropertyChanged
    {
        private User _user;
        private string _userName;
        private string _email;
        private decimal _balance;
        private int _id;
        private DateTime _created;
        private bool _isAdmin;
        private bool _isBlocked;
        private string _balanceString;

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public string BalanceString
        {
            get => _balanceString;
            set
            {
                _balanceString = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
                (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public decimal Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                OnPropertyChanged();
            }
        }

        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                _isAdmin = value;
                OnPropertyChanged();
            }
        }

        public bool IsBlocked
        {
            get => _isBlocked;
            set
            {
                _isBlocked = value;
                OnPropertyChanged();
            }
        }

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
            }
        }

        public DateTime Created
        {
            get => _created;
            set
            {
                _created = value;
            }
        }

        private readonly Action _closeAction;
        public ICommand CloseCommand { get; }
        public ICommand CreateAdminCommand { get; }
        public ICommand BlockCommand { get; }
        public ICommand SaveCommand { get; }
        public EditUserViewModel(User user, Action closeAction)
        {
            _user = user;
            Email = _user.Email;
            IsAdmin = _user.IsAdmin;
            IsBlocked = _user.IsBlocked;
            Id = user.Id;
            Created = user.CreateTime;
            Balance = user.Balance;
            UserName = user.UserName;
            BalanceString = _user.Balance.ToString();
            _closeAction = closeAction;
            CloseCommand = new RelayCommand(_ => ExecuteClose());
            CreateAdminCommand = new RelayCommand(_ => ChangeAdminState());
            BlockCommand = new RelayCommand(_ => ChangeBlockState());
            SaveCommand = new RelayCommand(_ => Save(), _ => Validate());
        }

        private void ExecuteClose()
        {
            _closeAction?.Invoke();
        }
        private void ChangeAdminState()
        {
            _user.IsAdmin = !_user.IsAdmin;
            IsAdmin = _user.IsAdmin;
        }

        private void ChangeBlockState()
        {
            _user.IsBlocked = !_user.IsBlocked;
            IsBlocked = _user.IsBlocked;
        }

        private bool Validate()
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (string.IsNullOrWhiteSpace(Email))
            {
                return false;
            }
            else if (!Regex.IsMatch(Email, emailPattern))
            {
                return false;
            }
            if (!decimal.TryParse(BalanceString, out decimal parsedBalance))
            {
                return false;
            }
            return true;
        }

        private void Save()
        {
            if (Validate())
            {
                try
                {
                    if(DataWorker.IsUserExist(Email) && Email != _user.Email)
                    {
                        RegisterError success = new RegisterError("Пользователь с таким email уже существует");
                        success.ShowDialog();
                    }
                    else
                    {
                        decimal.TryParse(BalanceString, out decimal parsedBalance);
                        User newUser = _user;
                        newUser.UserName = UserName;
                        newUser.Email = Email;
                        newUser.Balance = parsedBalance;
                        DataWorker.UpdateUser(newUser);

                        RegisterError success = new RegisterError("Успешно сохранено");
                        success.ShowDialog();
                    }
                }
                catch
                {
                    RegisterError error = new RegisterError("Данные введены неверно");
                    error.ShowDialog();
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
