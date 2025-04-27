using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using gamelauncher.ViewModels;

namespace gamelauncher.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Page
    {
        private readonly AdminPanelViewModel _viewModel;
        public AdminPanel()
        {
            InitializeComponent();
            var navigation = new MVVM.Navigation(AdminNavigate);
            _viewModel = new AdminPanelViewModel(navigation);
            this.DataContext = _viewModel;
            navigation.NavigateTo(typeof(AdminPanelGames));
        }
    }
}
