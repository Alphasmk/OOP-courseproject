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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using gamelauncher.ViewModels;

namespace gamelauncher.Views
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            int oldTh = 0;
            int newTh = 3;
            int time = 100;
            if (sender is TextBox tb && tb.Tag is Border border)
            {
                border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9319B8"));
                var Animation = new ThicknessAnimation
                {
                    From = new Thickness(oldTh),
                    To = new Thickness(newTh),
                    Duration = TimeSpan.FromMilliseconds(time)
                };
                border.BeginAnimation(Border.BorderThicknessProperty, Animation);
            }
            else if (sender is PasswordBox pb && pb.Tag is Border _border)
            {
                _border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9319B8"));
                var Animation = new ThicknessAnimation
                {
                    From = new Thickness(oldTh),
                    To = new Thickness(newTh),
                    Duration = TimeSpan.FromMilliseconds(time)
                };
                _border.BeginAnimation(Border.BorderThicknessProperty, Animation);
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int oldTh = 3;
            int newTh = 0;
            int time = 100;
            if (sender is TextBox tb && tb.Tag is Border border)
            {
                border.BorderBrush = Brushes.Purple;
                var Animation = new ThicknessAnimation
                {
                    From = new Thickness(oldTh),
                    To = new Thickness(newTh),
                    Duration = TimeSpan.FromMilliseconds(time)
                };
                border.BeginAnimation(Border.BorderThicknessProperty, Animation);
            }
            else if (sender is PasswordBox pb && pb.Tag is Border _border)
            {
                _border.BorderBrush = Brushes.Purple;
                var Animation = new ThicknessAnimation
                {
                    From = new Thickness(oldTh),
                    To = new Thickness(newTh),
                    Duration = TimeSpan.FromMilliseconds(time)
                };
                _border.BeginAnimation(Border.BorderThicknessProperty, Animation);
            }
        }
    }
}
