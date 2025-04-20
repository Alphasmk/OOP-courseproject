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
using gamelauncher.Model;
using gamelauncher.MVVM;
using gamelauncher.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace gamelauncher.Views
{
    /// <summary>
    /// Логика взаимодействия для MainTemplate.xaml
    /// </summary>
    public partial class MainTemplate : Window
    {
        private readonly MainViewModel _viewModel;
        public MainTemplate()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            var navigation = new MVVM.Navigation(MainFrame);
            _viewModel = new MainViewModel(navigation);
            this.DataContext = _viewModel;     
            if(CurrentUser.Instance.UserName == null)
            {
                MenuButtonText7.Text = CurrentUser.Instance.Email.Split('@')[0];
            }
            else
            {
                MenuButtonText7.Text = CurrentUser.Instance.UserName;
            }
            _viewModel.LogoutSuccessful += OnLogoutSuccessful;
        }

        private void OnLogoutSuccessful()
        {
            var loginWindow = new LoginPage();
            loginWindow.Show();
            this.Close();
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

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MaximizeMenu();
        }

        private void MaximizeMenu()
        {
            VisualStateManager.GoToState(MenuPanel, isMenuMaximized ? "Collapsed" : "Expanded", true);
            isMenuMaximized = !isMenuMaximized;
            //MenuColumn.Width = new GridLength(isMenuMaximized ? 300 : 120);
        }

        private bool isMenuMaximized = false;
        private void MenuPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            MenuWrapper.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 150,
                To = 250,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuBorder1.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 65,
                To = 200,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButton1Transform.BeginAnimation(TranslateTransform.XProperty, new DoubleAnimation
            {
                From = 0,
                To = -30, // влево
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButton1.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 65,
                To = 200,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButtonText1.Visibility = Visibility.Visible;
            MenuButtonText1.BeginAnimation(OpacityProperty, new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            isMenuMaximized = true;
            MenuBorder2.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 65,
                To = 200,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButton2Transform.BeginAnimation(TranslateTransform.XProperty, new DoubleAnimation
            {
                From = 0,
                To = -18, // влево
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButton2.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 65,
                To = 200,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButtonText2.Visibility = Visibility.Visible;
            MenuButtonText2.BeginAnimation(OpacityProperty, new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuBorder3.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 65,
                To = 200,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButton3Transform.BeginAnimation(TranslateTransform.XProperty, new DoubleAnimation
            {
                From = 0,
                To = -10, // влево
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButton3.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 65,
                To = 200,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButtonText3.Visibility = Visibility.Visible;
            MenuButtonText3.BeginAnimation(OpacityProperty, new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuBorder4.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 65,
                To = 200,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButton4Transform.BeginAnimation(TranslateTransform.XProperty, new DoubleAnimation
            {
                From = 0,
                To = -16, // влево
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButton4.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 65,
                To = 200,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButtonText4.Visibility = Visibility.Visible;
            MenuButtonText4.BeginAnimation(OpacityProperty, new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuBorder5.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 65,
                To = 200,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButton5Transform.BeginAnimation(TranslateTransform.XProperty, new DoubleAnimation
            {
                From = 0,
                To = -5, // влево
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButton5.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 65,
                To = 200,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButtonText5.Visibility = Visibility.Visible;
            MenuButtonText5.BeginAnimation(OpacityProperty, new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuBorder6.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 65,
                To = 200,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButton6Transform.BeginAnimation(TranslateTransform.XProperty, new DoubleAnimation
            {
                From = 0,
                To = -35, // влево
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButton6.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 65,
                To = 200,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButtonText6.Visibility = Visibility.Visible;
            MenuButtonText6.BeginAnimation(OpacityProperty, new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuBorder7.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 65,
                To = 200,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButtonText7.Visibility = Visibility.Visible;
            MenuButtonText7.BeginAnimation(OpacityProperty, new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(600),
                EasingFunction = new SineEase()
            });
            MainFrame.BeginAnimation(OpacityProperty, new DoubleAnimation
            {
                From = 1,
                To = 0.35,
                Duration = TimeSpan.FromMilliseconds(200),
                EasingFunction = new SineEase()
            });
        }

        private void MenuPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            MenuWrapper.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 250,
                To = 150,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuBorder1.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 200,
                To = 65,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButton1Transform.BeginAnimation(TranslateTransform.XProperty, new DoubleAnimation
            {
                From = -30,
                To = 0, // влево
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            isMenuMaximized = false;
            MenuButtonText1.BeginAnimation(OpacityProperty, new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButtonText1.Visibility = Visibility.Collapsed;
            MenuButton1.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 200,
                To = 65,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuBorder2.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 200,
                To = 65,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButton2Transform.BeginAnimation(TranslateTransform.XProperty, new DoubleAnimation
            {
                From = -10,
                To = 0, // влево
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButtonText2.BeginAnimation(OpacityProperty, new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButtonText2.Visibility = Visibility.Collapsed;
            MenuButton2.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 200,
                To = 65,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuBorder3.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 200,
                To = 65,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButton3Transform.BeginAnimation(TranslateTransform.XProperty, new DoubleAnimation
            {
                From = -18,
                To = 0, // влево
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButtonText3.BeginAnimation(OpacityProperty, new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButtonText3.Visibility = Visibility.Collapsed;
            MenuButton3.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 200,
                To = 65,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuBorder4.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 200,
                To = 65,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButton4Transform.BeginAnimation(TranslateTransform.XProperty, new DoubleAnimation
            {
                From = -16,
                To = 0, // влево
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButtonText4.BeginAnimation(OpacityProperty, new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButtonText4.Visibility = Visibility.Collapsed;
            MenuButton4.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 200,
                To = 65,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuBorder5.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 200,
                To = 65,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButton5Transform.BeginAnimation(TranslateTransform.XProperty, new DoubleAnimation
            {
                From = -5,
                To = 0, // влево
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButtonText5.BeginAnimation(OpacityProperty, new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButtonText5.Visibility = Visibility.Collapsed;
            MenuButton5.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 200,
                To = 65,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuBorder6.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 200,
                To = 65,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButton6Transform.BeginAnimation(TranslateTransform.XProperty, new DoubleAnimation
            {
                From = -35,
                To = 0, // влево
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButtonText6.BeginAnimation(OpacityProperty, new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButtonText6.Visibility = Visibility.Collapsed;
            MenuButton6.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 200,
                To = 65,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuBorder7.BeginAnimation(WidthProperty, new DoubleAnimation
            {
                From = 200,
                To = 65,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new SineEase()
            });
            MenuButtonText7.BeginAnimation(OpacityProperty, new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(600),
                EasingFunction = new SineEase(),
            });
            MenuButtonText7.Visibility = Visibility.Collapsed;
            MainFrame.BeginAnimation(OpacityProperty, new DoubleAnimation
            {
                From = 0.35,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(200),
                EasingFunction = new SineEase()
            });
        }
    }
}
