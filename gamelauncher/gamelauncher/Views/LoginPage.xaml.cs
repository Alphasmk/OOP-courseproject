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

namespace gamelauncher.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            string lang = Application.Current.Properties["AppLanguage"].ToString();
            LoadLanguage(lang);
        }
        public LoginPage(string _lang)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            LoadLanguage(Application.Current.Properties["AppLanguage"].ToString());
        }

        public bool _isRussian = true;
        private void SwitchLanguage_Click(object sender, RoutedEventArgs e)
        {
            _isRussian = !_isRussian;
            string lang = _isRussian ? "ru-RU" : "en-EN";
            LoadLanguage(lang);
        }

        private void LoadLanguage(string lang)
        {
            Application.Current.Properties["AppLanguage"] = lang;
            Application.Current.Resources.MergedDictionaries.Clear();

            var dict = new ResourceDictionary
            {
                Source = new Uri($"/Resources/{lang}.xaml", UriKind.Relative)
            };

            Application.Current.Resources.MergedDictionaries.Add(dict);

            CultureInfo culture = new CultureInfo(lang);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            Language = XmlLanguage.GetLanguage(culture.IetfLanguageTag);
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
            LoadLanguage(lang);
        }

        private void ToggleCreateAccount_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RegisterPage registerPage = new RegisterPage();
            this.Close();
            registerPage.Show();
        }
    }
}
