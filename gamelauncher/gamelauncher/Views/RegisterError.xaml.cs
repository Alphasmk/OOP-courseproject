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
using System.Windows.Shapes;
using gamelauncher.MVVM;

namespace gamelauncher.Views
{
    /// <summary>
    /// Логика взаимодействия для RegisterError.xaml
    /// </summary>
    public partial class RegisterError : Window
    {
        public ICommand CloseCommand { get; }
        public RegisterError(string _message)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = this;
            message.Text = _message;
            CloseCommand = new RelayCommand(_ => CloseWindow());
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
