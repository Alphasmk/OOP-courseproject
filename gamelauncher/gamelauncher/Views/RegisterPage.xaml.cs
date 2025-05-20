using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using gamelauncher.Model;
using gamelauncher.MVVM;
using gamelauncher.ViewModels;

namespace gamelauncher.Views
{
    /// <summary>
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Window
    {
        public RegisterPage()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = new RegisterViewModel();
        }

        public void CreateAccount()
        {
            DataWorker.CreateUser(LoginEmailTextBox.Text.ToString(), RegisterPasswordTextBox.Text.ToString()); 
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            this.Close();
            loginPage.Show();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Позволяет перетаскивать окно
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool _isRussian = true;
        //private void SwitchLanguage_Click(object sender, RoutedEventArgs e)
        //{
        //    _isRussian = !_isRussian;
        //    string lang = _isRussian ? "ru-RU" : "en-EN";
        //    LoadLanguage(lang);
        //}

        private void Password_TextChanged(object sender, EventArgs e)
        {
            string val1 = RepeatPassword.Text;
            string val2 = RegisterPasswordTextBox.Text;
            if (val1 != val2)
            {
                RepeatPasswordError.Visibility = Visibility.Visible;
            }
            else
            {
                RepeatPasswordError.Visibility = Visibility.Hidden;
            }
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
