using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using gamelauncher.Model;
using gamelauncher.MVVM;
using gamelauncher.Views;
using Syncfusion.Windows.Shared;

namespace gamelauncher.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {

        private string _currentTheme = "Dark";
        private string _currentLanguage = "en-EN";
        private bool _isLightTheme;
        public ICommand ToggleThemeCommand { get; }
        public ICommand ChangeLanguageCommand { get; }
        public struct UserPurchase
        {
            public string Name { get; set; }
            public DateTime? Date { get; set; }
            public decimal Price { get; set; }
        }
        private string _userName;
        private string _userPassword;
        private string _userPasswordRepeat;
        private List<Purchase> _userPurchases;
        private List<UserPurchase> _userPurchasesList;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public string UserPassword
        {
            get => _userPassword;
            set
            {
                _userPassword = value;
                OnPropertyChanged();
            }
        }

        public List<UserPurchase> UserPurchasesList
        {
            get => _userPurchasesList;
            set
            {
                _userPurchasesList = value;
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

        public string UserPasswordRepeat
        {
            get => _userPasswordRepeat;
            set
            {
                _userPasswordRepeat = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand {  get; }
        Type type = typeof(SettingsPage);
        public SettingsViewModel()
        {
            
            UserName = CurrentUser.Instance.UserName;
            SaveCommand = new RelayCommand(_ => Save(), _ => CanSave());
            _currentTheme = ThemeManager.CurrentTheme;
            _userPurchasesList = new List<UserPurchase>();
            _userPurchases = DataWorker.GetUserPurchases();
            if (_userPurchases != null)
            {
                foreach(var purchase in _userPurchases)
                {
                    UserPurchasesList.Add(new UserPurchase{
                        Name = DataWorker.GetGameNameById(purchase.GameId),
                        Date = purchase.PurchaseDate,
                        Price = purchase.PricePaid
                    });
                }
            }

            ToggleThemeCommand = new RelayCommand(_ => ToggleTheme());
            ChangeLanguageCommand = new RelayCommand(_ => ChangeLanguage());
            LanguageChanged();
            ThemeChanged(type);
            ThemeManager.ThemeLoaded += ThemeChanged;
            LanguageManager.LanguageLoaded += LanguageChanged;
        }

        private void ToggleTheme()
        {
            _currentTheme = _currentTheme == "Light" ? "Dark" : "Light";
            ThemeManager.SwitchTheme(_currentTheme, typeof(SettingsPage));
        }

        private void ChangeLanguage()
        {
            _currentLanguage = _currentLanguage == "en-EN" ? "ru-RU" : "en-EN";
            LanguageManager.SwitchLanguage(_currentLanguage);
        }

        private void ThemeChanged(Type type)
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

        private bool CanSave()
        {
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,20}$";
            if (string.IsNullOrWhiteSpace(UserName) || (UserPasswordRepeat != UserPassword))
            {
                return false;
            }
            else if(!string.IsNullOrEmpty(UserPassword) && Regex.IsMatch(passwordPattern, UserPassword))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(UserPasswordRepeat) && string.IsNullOrWhiteSpace(UserPassword) && !string.IsNullOrWhiteSpace(UserName))
            {
                DataWorker.UpdateUserName(UserName);
            }
            else
            {
                DataWorker.UpdateUserName(UserName);
                DataWorker.UpdateUserPassword(UserPassword);
            }
            CurrentUser.Instance.UserName = UserName;
            CurrentUser.OnUserChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
