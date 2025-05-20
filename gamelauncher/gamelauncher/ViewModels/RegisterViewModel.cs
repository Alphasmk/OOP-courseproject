using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using gamelauncher.Model;
using gamelauncher.MVVM;
using gamelauncher.Validation;
using gamelauncher.Views;
using Newtonsoft.Json.Linq;

namespace gamelauncher.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private string _currentTheme = "Dark";
        private string _currentLanguage = "en-EN";
        private bool _isLightTheme;
        public ICommand ToggleThemeCommand { get; }
        public ICommand ChangeLanguageCommand { get; }
        public EmailViewModel EmailViewModel { get; }
        public PasswordViewModel PasswordViewModel { get; }

        private string _repeatPassword;
        public string RepeatPassword
        {
            get => _repeatPassword;
            set
            {
                _repeatPassword = value;
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

        public ICommand CreateAccountCommand { get; }

        public RegisterViewModel()
        {
            EmailViewModel = new EmailViewModel();
            PasswordViewModel = new PasswordViewModel();
            _currentTheme = ThemeManager.CurrentTheme;

            EmailViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(EmailViewModel.Email))
                    CommandManager.InvalidateRequerySuggested();
            };

            PasswordViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(PasswordViewModel.Password))
                    CommandManager.InvalidateRequerySuggested();
            };

            CreateAccountCommand = new RelayCommand(
                async _ => await CreateAccount(),
                _ => CanCreateAccount()
            );

            ToggleThemeCommand = new RelayCommand(_ => ToggleTheme());
            ChangeLanguageCommand = new RelayCommand(_ => ChangeLanguage());
            LanguageChanged();
            ThemeChanged();
            ThemeManager.ThemeLoad += ThemeChanged;
            LanguageManager.LanguageLoaded += LanguageChanged;
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

        private void ThemeChanged()
        {
            if (ThemeManager.CurrentTheme == "Light")
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

        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                return false;
            }
            return true;
        }

        public bool ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,20}$";
            if (!Regex.IsMatch(password, passwordPattern))
            {
                return false;
            }
            return true;
        }
        private bool CanCreateAccount()
        {
            return ValidateEmail(EmailViewModel.Email) &&
                   ValidatePassword(PasswordViewModel.Password) &&
                   PasswordViewModel.Password == RepeatPassword;
        }

        private async Task CreateAccount()
        {
            bool isOk;
            try
            {
                using (var ping = new Ping())
                {
                    var reply = ping.Send("8.8.8.8", 3000);
                    isOk = reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                isOk = false;
            }
            if (isOk)
            {
                await DataWorker.CreateUser(EmailViewModel.Email, PasswordViewModel.Password);
            }
            else
            {
                if(LanguageManager.CurrentLanguage == "ru-RU")
                {
                    RegisterError err = new RegisterError("Проверьте интернет-соединение");
                    err.ShowDialog();
                }
                else
                {
                    RegisterError err = new RegisterError("Check your internet connection");
                    err.ShowDialog();
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
