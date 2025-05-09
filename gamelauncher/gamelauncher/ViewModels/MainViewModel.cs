using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;
using gamelauncher.Model;
using gamelauncher.MVVM;
using gamelauncher.Views;

namespace gamelauncher.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _currentPage;
        public string CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }

        public INavigation _navigation;
        public ICommand GoToShop { get; }
        public ICommand GoToLiked { get; }
        public ICommand GoToLibrary { get; }
        public ICommand GoToSettings { get; }
        public ICommand GoToInfo { get; }
        public ICommand LogoutCommand { get; }
        public ICommand GoToAdmin { get; }

        private string _selectedMenuItem;
        public string SelectedMenuItem
        {
            get => _selectedMenuItem;
            set
            {
                _selectedMenuItem = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel(INavigation navigation)
        {
            _navigation = navigation;
            GoToShop = new RelayCommand(_ =>
            {
                _navigation.NavigateTo(typeof(ShopPage), new Action<Game>(NavigateToGamePage));
                CurrentPage = "Shop";
                SelectedMenuItem = "Shop";
            });

            GoToLiked = new RelayCommand(_ =>
            {
                _navigation.NavigateTo(typeof(Views.LikedPage));
                CurrentPage = "Liked";
                SelectedMenuItem = "Liked";
            });

            GoToLibrary = new RelayCommand(_ =>
            {
                _navigation.NavigateTo(typeof(Views.LibraryPage));
                CurrentPage = "Library";
                SelectedMenuItem = "Library";
            });

            GoToSettings = new RelayCommand(_ =>
            {
                _navigation.NavigateTo(typeof(Views.SettingsPage));
                CurrentPage = "Settings";
                SelectedMenuItem = "Settings";
            });

            GoToInfo = new RelayCommand(_ =>
            {
                _navigation.NavigateTo(typeof(Views.InfoPage));
                CurrentPage = "Info";
                SelectedMenuItem = "Info";
            });

            GoToAdmin = new RelayCommand(_ =>
            {
                _navigation.NavigateTo(typeof(Views.AdminPanel));
                CurrentPage = "Admin";
                SelectedMenuItem = "Admin";
            });

            CurrentPage = "Shop";
            LogoutCommand = new RelayCommand(_ => Logout());
        }

        public event Action LogoutSuccessful;
        public event Action<Game> NavigateToGame;
        public void Logout()
        {
            CurrentUser.Logout();
            LogoutSuccessful?.Invoke();
        }

        public void NavigateToGamePage(Game game)
        {
            _navigation.NavigateTo(typeof(GamePage), game);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class MenuItemToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() == parameter?.ToString() ? "#B824E4" : "#3E3E3E";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
