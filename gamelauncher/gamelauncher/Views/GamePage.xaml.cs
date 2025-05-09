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
using gamelauncher.Model;

namespace gamelauncher.Views
{
    public partial class GamePage : Page
    {
        public GamePage(Game game)
        {
            InitializeComponent();
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            if (FlipView.SelectedIndex > 0)
                FlipView.SelectedIndex--;
            FlipView.HideControlButtons();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (FlipView.SelectedIndex < FlipView.Items.Count - 1)
                FlipView.SelectedIndex++;
            FlipView.HideControlButtons();
        }

        private void FlipView_Loaded(object sender, RoutedEventArgs e)
        {
            FlipView.HideControlButtons();
        }
    }
}
