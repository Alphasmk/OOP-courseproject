using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using gamelauncher.Model;
using gamelauncher.ViewModels;

namespace gamelauncher.Views
{
    /// <summary>
    /// Логика взаимодействия для AddToGroup.xaml
    /// </summary>
    public partial class AddToGroup : Window
    {
        private AddToGroupViewModel viewModel;
        public AddToGroup(Action refreshCallback, int id)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            viewModel = new AddToGroupViewModel(CloseWindow, refreshCallback, id);
            DataContext = viewModel;
        }

        private async void CloseWindow()
        {
            var fadeOut = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2)
            };

            this.BeginAnimation(OpacityProperty, fadeOut);

            await Task.Delay(200);
            this.Close();
        }
    }
}
