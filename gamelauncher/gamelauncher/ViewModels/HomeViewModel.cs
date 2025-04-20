using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using gamelauncher.MVVM;
using gamelauncher.Views;
using Microsoft.Identity.Client;

namespace gamelauncher.ViewModels
{
    internal class HomeViewModel : ViewModelBase
    {

        public readonly NavigationService navigationService;

        public RelayCommand GoToShop => new RelayCommand(execute => { }, canExecute => { return true; });

        public HomeViewModel(NavigationService _navigationService)
        {
            navigationService = _navigationService;
        }
    }

    public class NavigationService
    {
        public Frame _frame;

        public NavigationService(Frame frame)
        {
            _frame = frame;
        }
        public void Navigate(Uri uri)
        {
            _frame.Navigate(uri);
        }
    }
}
