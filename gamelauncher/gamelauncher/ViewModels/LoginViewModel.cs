using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using gamelauncher.Model;
using gamelauncher.MVVM;
using gamelauncher.Views;

namespace gamelauncher.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _currentTheme = "Dark";
        private string _currentLanguage = "en-EN";
        private bool _isLightTheme;
        public ICommand ToggleThemeCommand { get; }
        public ICommand ChangeLanguageCommand { get; }
        public event Action LoginSuccessful;
        public string _email;
        public string _password;
        public ICommand LoginCommand { get; }
        public ICommand RestoreCommand { get; }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                _currentLanguage = value;
                OnPropertyChanged();
            }
        }

        public bool IsLightTheme
        {
            get => _isLightTheme;
            set
            {
                _isLightTheme = value;
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
            RestoreCommand = new RelayCommand(_ => Restore());
            _currentTheme = ThemeManager.CurrentTheme;
            ToggleThemeCommand = new RelayCommand(_ => ToggleTheme());
            ChangeLanguageCommand = new RelayCommand(_ => ChangeLanguage());
            LanguageChanged();
            ThemeChanged();
            ThemeManager.ThemeLoad += ThemeChanged;
            LanguageManager.LanguageLoaded += LanguageChanged;
        }

        private void Restore()
        {
            RestorePasswordPage page = new RestorePasswordPage();
            page.ShowDialog();
        }

        private void ToggleTheme()
        {
            _currentTheme = _currentTheme == "Light" ? "Dark" : "Light";
            ThemeManager.SwitchTheme(_currentTheme);
        }

        private void ChangeLanguage()
        {
            _currentLanguage = _currentLanguage == "en-EN" ? "ru-RU" : "en-EN";
            LanguageManager.SwitchLanguage(_currentLanguage);
        }

        private void LoginAccount(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                if(LanguageManager.CurrentLanguage == "ru-RU")
                {
                    RegisterError error = new RegisterError("Неверный email или пароль");
                    error.ShowDialog();
                }
                else
                {
                    RegisterError error = new RegisterError("Incorrect email or password");
                    error.ShowDialog();
                }
            }
            else if(string.IsNullOrWhiteSpace(email))
            {
                if (LanguageManager.CurrentLanguage == "ru-RU")
                {
                    RegisterError error = new RegisterError("Пользователь с таким email не существует");
                    error.ShowDialog();
                }
                else
                {
                    RegisterError error = new RegisterError("User with such email does not exist");
                    error.ShowDialog();
                }
            }
            else
            {
                if (!DataWorker.IsUserExist(email))
                {
                    if (LanguageManager.CurrentLanguage == "ru-RU")
                    {
                        RegisterError error = new RegisterError("Пользователь с таким email не существует");
                        error.ShowDialog();
                    }
                    else
                    {
                        RegisterError error = new RegisterError("User with such email does not exist");
                        error.ShowDialog();
                    }
                }
                else if(DataWorker.CheckBlock(email))
                {
                    if (LanguageManager.CurrentLanguage == "ru-RU")
                    {
                        RegisterError error = new RegisterError("Пользователь заблокирован");
                        error.ShowDialog();
                    }
                    else
                    {
                        RegisterError error = new RegisterError("User is blocked");
                        error.ShowDialog();
                    }
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
                        if (LanguageManager.CurrentLanguage == "ru-RU")
                        {
                            RegisterError error = new RegisterError("Неверный email или пароль");
                            error.ShowDialog();
                        }
                        else
                        {
                            RegisterError error = new RegisterError("Incorrect email or password");
                            error.ShowDialog();
                        }
                    }
                }
            }
        }

        private void ThemeChanged()
        {
            if(ThemeManager.CurrentTheme == "Light")
            {
                IsLightTheme = true;
            }
            else
            {
                IsLightTheme = false;
            }
        }

        private void LanguageChanged()
        {
            if (LanguageManager.CurrentLanguage == "ru-RU")
            {
                CurrentLanguage = "ru-RU";
            }
            else
            {
                CurrentLanguage = "en-EN";
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
