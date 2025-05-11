using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;

namespace gamelauncher.Behaviors
{
    public static class ListBoxCenterOnSelectedBehavior
    {
        public static readonly DependencyProperty CenterOnSelectedProperty =
            DependencyProperty.RegisterAttached(
                "CenterOnSelected",
                typeof(bool),
                typeof(ListBoxCenterOnSelectedBehavior),
                new PropertyMetadata(false, OnCenterOnSelectedChanged));

        public static bool GetCenterOnSelected(DependencyObject obj) =>
            (bool)obj.GetValue(CenterOnSelectedProperty);

        public static void SetCenterOnSelected(DependencyObject obj, bool value) =>
            obj.SetValue(CenterOnSelectedProperty, value);

        private static void OnCenterOnSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListBox lb)
            {
                if ((bool)e.NewValue)
                    lb.SelectionChanged += ListBox_SelectionChanged;
                else
                    lb.SelectionChanged -= ListBox_SelectionChanged;
            }
        }

        private static void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lb = (ListBox)sender;
            lb.Dispatcher.BeginInvoke(new Action(() =>
            {
                // Находим ScrollViewer внутри ListBox-а
                if (VisualTreeHelper.GetChild(lb, 0) is Border border &&
                    VisualTreeHelper.GetChild(border, 0) is ScrollViewer sv)
                {
                    // Находим визуальный контейнер выбранного элемента
                    if (lb.ItemContainerGenerator.ContainerFromItem(lb.SelectedItem) is FrameworkElement item)
                    {
                        // Вычисляем смещение так, чтобы item оказался по центру
                        var transform = item.TransformToAncestor(sv);
                        var itemPos = transform.Transform(new Point(0, 0)).X;
                        double targetOffset = sv.HorizontalOffset + itemPos - (sv.ViewportWidth - item.ActualWidth) / 2;

                        // Плавная анимация
                        var anim = new DoubleAnimation(targetOffset, TimeSpan.FromMilliseconds(300))
                        {
                            AccelerationRatio = 0.2,
                            DecelerationRatio = 0.2
                        };
                        var storyboard = new Storyboard();
                        storyboard.Children.Add(anim);
                        Storyboard.SetTarget(anim, sv);
                        Storyboard.SetTargetProperty(anim, new PropertyPath("HorizontalOffset"));
                        storyboard.Begin();
                    }
                }
            }), DispatcherPriority.Loaded);
        }
    }
}
