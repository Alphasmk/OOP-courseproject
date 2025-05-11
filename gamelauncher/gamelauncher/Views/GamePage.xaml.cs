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
using System.Windows.Threading;
using gamelauncher.Model;
using gamelauncher.ViewModels;

namespace gamelauncher.Views
{
    public partial class GamePage : Page
    {
        private GamePageViewModel viewModel;
        public GamePage(Game game)
        {
            InitializeComponent();
            viewModel = new GamePageViewModel(game);
            DataContext = viewModel;
        }

        private void MainScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ExternalScrollBar.Value = e.VerticalOffset;
        }

        private void ExternalScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainScrollViewer.ScrollToVerticalOffset(e.NewValue);
        }
    }
}
