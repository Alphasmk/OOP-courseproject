using gamelauncher.ViewModels;
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
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace gamelauncher.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        private readonly LoginViewModel _viewModel;
        public LoginPage()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _viewModel = new LoginViewModel();
            this.DataContext = _viewModel;
            _viewModel.LoginSuccessful += OnLoginSuccessful;
        }
        private void OnLoginSuccessful()
        {
            var mainWindow = new MainTemplate();
            mainWindow.Show();
            this.Close();
        }
        public LoginPage(string _lang)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel._password = LoginPasswordBox.Password;
            }
        }

        public bool _isRussian = true;
        private void SwitchLanguage_Click(object sender, RoutedEventArgs e)
        {
            _isRussian = !_isRussian;
            string lang = _isRussian ? "ru-RU" : "en-EN";
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

        private void ToggleLanguage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string lang;
            if (_isRussian)
            {
                ToggleLanguage.Source = new BitmapImage(new Uri("pack://application:,,,/img/russia.png"));
                lang = "ru-RU";
            }
            else
            {
                ToggleLanguage.Source = new BitmapImage(new Uri("pack://application:,,,/img/usa.png"));
                lang = "en-EN";
            }

            _isRussian = !_isRussian;
        }

        private void ToggleCreateAccount_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RegisterPage registerPage = new RegisterPage();
            this.Close();
            registerPage.Show();
        }

        private void LoginEmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            int oldTh = 0;
            int newTh = 3;
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
