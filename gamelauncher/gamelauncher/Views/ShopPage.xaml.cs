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

namespace gamelauncher.Views
{
    /// <summary>
    /// Логика взаимодействия для ShopPage.xaml
    /// </summary>
    public partial class ShopPage : Page
    {
        public ShopPage()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Tag is Border border)
            {
                border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9319B8"));
                var thicknessAnimation = new ThicknessAnimation
                {
                    From = new Thickness(0),
                    To = new Thickness(2),
                    Duration = TimeSpan.FromMilliseconds(100)
                };
                border.BeginAnimation(Border.BorderThicknessProperty, thicknessAnimation);

                var widthAnimation = new DoubleAnimation
                {
                    From = 400,
                    To = 450,
                    Duration = TimeSpan.FromMilliseconds(100)
                };

                border.BeginAnimation(Border.WidthProperty, widthAnimation);
                var moveAnimation = new DoubleAnimation
                {
                    To = -40,
                    Duration = TimeSpan.FromSeconds(0.5),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
                    FillBehavior = FillBehavior.HoldEnd
                };
                LoopTransform.BeginAnimation(TranslateTransform.XProperty, moveAnimation);

                var moveTextAnimation = new DoubleAnimation
                {
                    To = -30,
                    Duration = TimeSpan.FromSeconds(0.5),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
                    FillBehavior = FillBehavior.HoldEnd
                };
                LoopTransform1.BeginAnimation(TranslateTransform.XProperty, moveTextAnimation);
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Tag is Border border)
            {
                border.BorderBrush = Brushes.Purple;
                var thicknessAnimation = new ThicknessAnimation
                {
                    From = new Thickness(2),
                    To = new Thickness(0),
                    Duration = TimeSpan.FromMilliseconds(100)
                };
                border.BeginAnimation(Border.BorderThicknessProperty, thicknessAnimation);

                var widthAnimation = new DoubleAnimation
                {
                    From = 450,
                    To = 400,
                    Duration = TimeSpan.FromMilliseconds(100)
                };

                border.BeginAnimation(Border.WidthProperty, widthAnimation);

                var moveBackAnimation = new DoubleAnimation
                {
                    To = 0,
                    Duration = TimeSpan.FromSeconds(0.5),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
                    FillBehavior = FillBehavior.HoldEnd
                };
                LoopTransform.BeginAnimation(TranslateTransform.XProperty, moveBackAnimation);

                var moveTextAnimation = new DoubleAnimation
                {
                    To = 0,
                    Duration = TimeSpan.FromSeconds(0.5),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
                    FillBehavior = FillBehavior.HoldEnd
                };
                LoopTransform1.BeginAnimation(TranslateTransform.XProperty, moveTextAnimation);
            }
        }
    }
}
