using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace gamelauncher.MVVM
{
    public interface INavigation
    {
        void NavigateTo(Type sourcePageType);
        void SetFrame(Frame frame);
    }
    public class Navigation : INavigation
    {
        private Frame _mainFrame;
        public Navigation(Frame frame)
        {
            _mainFrame = frame;
        }
        public void NavigateTo(Type sourcePageType)
        {
            _mainFrame.Navigate(Activator.CreateInstance(sourcePageType));
        }

        public void SetFrame(Frame frame)
        {
            _mainFrame = frame;
        }
    }
}
