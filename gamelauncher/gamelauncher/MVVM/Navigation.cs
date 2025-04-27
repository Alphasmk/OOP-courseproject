using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using gamelauncher.ViewModels;
using gamelauncher.Views;

namespace gamelauncher.MVVM
{
    public interface INavigation
    {
        void NavigateTo(Type sourcePageType);
        void SetFrame(Frame frame);
        event Action<Type> PageNavigated;
    }
    public class Navigation : INavigation
    {
        private Frame _mainFrame;
        public event Action<Type> PageNavigated;
        public Navigation(Frame frame)
        {
            _mainFrame = frame;
        }
        public void NavigateTo(Type sourcePageType)
        {
            if(_mainFrame.Name == "MainFrame")
            {
                var fadeOut = new DoubleAnimation(0, TimeSpan.FromSeconds(0.1));
                fadeOut.Completed += (s, _) =>
                {
                    _mainFrame.Navigate(Activator.CreateInstance(sourcePageType));
                    var fadeIn = new DoubleAnimation(0.35, TimeSpan.FromSeconds(0.1));
                    _mainFrame.BeginAnimation(UIElement.OpacityProperty, fadeIn);
                    PageNavigated?.Invoke(sourcePageType);
                };

                _mainFrame.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            }
            else
            {
                var fadeOut = new DoubleAnimation(0, TimeSpan.FromSeconds(0.1));
                fadeOut.Completed += (s, _) =>
                {
                    _mainFrame.Navigate(Activator.CreateInstance(sourcePageType));
                    var fadeIn = new DoubleAnimation(1, TimeSpan.FromSeconds(0.1));
                    _mainFrame.BeginAnimation(UIElement.OpacityProperty, fadeIn);
                };

                _mainFrame.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            }
        }

        public void SetFrame(Frame frame)
        {
            _mainFrame = frame;
        }
    }
}
