using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using gamelauncher.MVVM;

namespace gamelauncher.ViewModels
{
    public class AdminPanelViewModel
    {
        public INavigation _navigation;

        public ICommand GoToUsers { get; }
        public ICommand GoToGames { get; }

        public AdminPanelViewModel(INavigation navigation)
        {
            _navigation = navigation;
            GoToUsers = new RelayCommand(_ => _navigation.NavigateTo(typeof(Views.AdminPanelUsers)));
            GoToGames = new RelayCommand(_ => _navigation.NavigateTo(typeof(Views.AdminPanelGames)));
        }
    }
}
