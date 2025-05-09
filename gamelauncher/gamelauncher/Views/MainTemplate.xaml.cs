using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using gamelauncher.Model;
using gamelauncher.MVVM;
using gamelauncher.ViewModels;
using MahApps.Metro.Controls;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace gamelauncher.Views
{
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
            if (CurrentUser.Instance.UserName == null)
            {
                MenuButtonText7.Text = CurrentUser.Instance.Email.Split('@')[0];
            }
            else
            {
                MenuButtonText7.Text = CurrentUser.Instance.UserName;
            }

            if (CurrentUser.IsAdmin)
            {
                MenuBorder8.Visibility = Visibility.Visible;
            }
            else
            {
                MenuBorder8.Visibility = Visibility.Collapsed;
            }
            _viewModel.LogoutSuccessful += OnLogoutSuccessful;
            navigation.PageNavigated += OnPageNavigated;
            navigation.NavigateTo(typeof(ShopPage), new Action<Game>(_viewModel.NavigateToGamePage));
        }

        private void OnPageNavigated(Type sourcePageType)
        {
            var menuBorders = new[] { MenuBorder1, MenuBorder2, MenuBorder3, MenuBorder4, MenuBorder5, MenuBorder8 };

            foreach (var border in menuBorders)
            {
                if (!(border.Background is SolidColorBrush brush))
                {
                    brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E3E3E"));
                    border.Background = brush;
                }

                var animation = new ColorAnimation
                {
                    To = (Color)ColorConverter.ConvertFromString("#3E3E3E"),
                    Duration = TimeSpan.FromSeconds(0.3),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };

                brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);

                if (border.RenderTransform is ScaleTransform scaleTransform)
                {
                    var scaleAnimation = new DoubleAnimation
                    {
                        To = 1.0, 
                        Duration = TimeSpan.FromSeconds(0.2),
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                    };
                    border.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
                    border.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
                }
            }

            switch (_viewModel.CurrentPage)
            {
                case "Shop":
                    ApplySelection(MenuBorder1);
                    break;
                case "Liked":
                    ApplySelection(MenuBorder2);
                    break;
                case "Library":
                    ApplySelection(MenuBorder3);
                    break;
                case "Settings":
                    ApplySelection(MenuBorder4);
                    break;
                case "Info":
                    ApplySelection(MenuBorder5);
                    break;
                case "Admin":
                    ApplySelection(MenuBorder8);
                    break;
            }
        }

        private void ApplySelection(Border border)
        {
            if (!(border.Background is SolidColorBrush brush))
            {
                brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9319B8"));
                border.Background = brush;
            }

            var animation = new ColorAnimation
            {
                To = (Color)ColorConverter.ConvertFromString("#9319B8"),
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            if (border.RenderTransform == null || !(border.RenderTransform is ScaleTransform))
            {
                border.RenderTransform = new ScaleTransform(1.075, 1.075);
                border.RenderTransformOrigin = new Point(0.5, 0.5);
            }
            else
            {
                border.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                border.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, null);
                ((ScaleTransform)border.RenderTransform).ScaleX = 1.075;
                ((ScaleTransform)border.RenderTransform).ScaleY = 1.075;
            }
        }

        private void OnLogoutSuccessful()
        {
            var loginWindow = new LoginPage();
            this.Close();
            loginWindow.Show();
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
        }

        private void MenuButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is Border border)
            {
                // Пропускаем анимацию, если это уже выбранный элемент
                if ((border.Name == "MenuBorder1" && _viewModel.CurrentPage == "Shop") ||
                    (border.Name == "MenuBorder2" && _viewModel.CurrentPage == "Liked") ||
                    (border.Name == "MenuBorder3" && _viewModel.CurrentPage == "Library") ||
                    (border.Name == "MenuBorder4" && _viewModel.CurrentPage == "Settings") ||
                    (border.Name == "MenuBorder5" && _viewModel.CurrentPage == "Info") ||
                    (border.Name == "MenuBorder8" && _viewModel.CurrentPage == "Admin"))
                {
                    return;
                }

                // Убедимся, что есть RenderTransform
                if (border.RenderTransform == null || !(border.RenderTransform is ScaleTransform))
                {
                    border.RenderTransform = new ScaleTransform(1, 1);
                    border.RenderTransformOrigin = new Point(0.5, 0.5); // Масштабирование из центра
                }

                // Анимация увеличения на 10%
                var scaleAnimation = new DoubleAnimation
                {
                    To = 1.05, // Увеличение на 10%
                    Duration = TimeSpan.FromSeconds(0.2),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };

                border.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
                border.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
            }
        }

        private void MenuButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is Border border)
            {
                // Не меняем размер, если это выбранный элемент
                if ((border.Name == "MenuBorder1" && _viewModel.CurrentPage == "Shop") ||
                    (border.Name == "MenuBorder2" && _viewModel.CurrentPage == "Liked") ||
                    (border.Name == "MenuBorder3" && _viewModel.CurrentPage == "Library") ||
                    (border.Name == "MenuBorder4" && _viewModel.CurrentPage == "Settings") ||
                    (border.Name == "MenuBorder5" && _viewModel.CurrentPage == "Info") ||
                    (border.Name == "MenuBorder8" && _viewModel.CurrentPage == "Admin"))
                {
                    return;
                }

                // Возвращаем к исходному размеру
                if (border.RenderTransform is ScaleTransform scaleTransform)
                {
                    var scaleAnimation = new DoubleAnimation
                    {
                        To = 1.0, // Исходный размер
                        Duration = TimeSpan.FromSeconds(0.2),
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                    };

                    scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
                    scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
                }
            }
        }

        private bool isMenuMaximized = false;
        private void MenuPanel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
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

            if (CurrentUser.IsAdmin)
            {
                MenuBorder8.BeginAnimation(WidthProperty, new DoubleAnimation
                {
                    From = 65,
                    To = 200,
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new SineEase()
                });
                MenuButton8Transform.BeginAnimation(TranslateTransform.XProperty, new DoubleAnimation
                {
                    From = 0,
                    To = -35, // влево
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new SineEase()
                });
                MenuButton8.BeginAnimation(WidthProperty, new DoubleAnimation
                {
                    From = 65,
                    To = 200,
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new SineEase()
                });
                MenuButtonText8.Visibility = Visibility.Visible;
                MenuButtonText8.BeginAnimation(OpacityProperty, new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new SineEase()
                });
            }
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

        private void MenuPanel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
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
            if (CurrentUser.IsAdmin)
            {
                MenuBorder8.BeginAnimation(WidthProperty, new DoubleAnimation
                {
                    From = 200,
                    To = 65,
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new SineEase()
                });
                MenuButton8Transform.BeginAnimation(TranslateTransform.XProperty, new DoubleAnimation
                {
                    From = -35,
                    To = 0, // влево
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new SineEase()
                });
                MenuButtonText8.BeginAnimation(OpacityProperty, new DoubleAnimation
                {
                    From = 1,
                    To = 0,
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new SineEase()
                });
                MenuButtonText8.Visibility = Visibility.Collapsed;
                MenuButton8.BeginAnimation(WidthProperty, new DoubleAnimation
                {
                    From = 200,
                    To = 65,
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new SineEase()
                });
            }

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
