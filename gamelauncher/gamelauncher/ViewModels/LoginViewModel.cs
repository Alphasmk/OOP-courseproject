using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using gamelauncher.Model;
using gamelauncher.MVVM;
using gamelauncher.Views;

namespace gamelauncher.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event Action LoginSuccessful;
        public string _email;
        public string _password;
        public ICommand LoginCommand { get; }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(
                execute: _ => LoginAccount(_email, _password)
            );
        }

        private void LoginAccount(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                RegisterError error = new RegisterError("Неверный email или пароль");
                error.ShowDialog();
            }
            else if(string.IsNullOrWhiteSpace(email))
            {
                RegisterError error = new RegisterError("Пользователь с таким email не существует");
                error.ShowDialog();
            }
            else
            {
                if (!DataWorker.IsUserExist(email))
                {
                    RegisterError error = new RegisterError("Пользователь с таким email не существует");
                    error.ShowDialog();
                }
                else
                {
                    password = DataWorker.HashPassword(password);
                    User user = DataWorker.GetUser(email);
                    if (user.Password == password)
                    {
                        CurrentUser.Login(user);
                        LoginSuccessful?.Invoke();
                    }
                    else
                    {
                        RegisterError error = new RegisterError("Неверный email или пароль");
                        error.ShowDialog();
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
