using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using gamelauncher.MVVM;
using gamelauncher.Views;

namespace gamelauncher.ViewModels
{
    public class MainViewModel
    {
        public INavigation _navigation;


        public ICommand GoToShop { get; }
        public ICommand GoToLiked { get; }
        public ICommand GoToLibrary { get; }
        public ICommand GoToSettings { get; }
        public ICommand GoToInfo { get; }
        public ICommand LogoutCommand { get; }

        public MainViewModel(INavigation navigation)
        {
            _navigation = navigation;
            GoToShop = new RelayCommand(_ => _navigation.NavigateTo(typeof(Views.ShopPage)));
            GoToLiked = new RelayCommand(_ => _navigation.NavigateTo(typeof(Views.LikedPage)));
            GoToLibrary = new RelayCommand(_ => _navigation.NavigateTo(typeof(Views.LibraryPage)));
            GoToSettings = new RelayCommand(execute => _navigation.NavigateTo(typeof(Views.SettingsPage)));
            GoToInfo = new RelayCommand(execute => _navigation.NavigateTo(typeof(Views.InfoPage)));
            LogoutCommand = new RelayCommand(_ => Logout());
        }

        public event Action LogoutSuccessful;
        public void Logout()
        {
            CurrentUser.Logout();
            LogoutSuccessful?.Invoke();
        }
    }
}
