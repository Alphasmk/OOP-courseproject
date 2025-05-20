using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Collections.Generic;
using gamelauncher.Views;
using gamelauncher.MVVM;
using System.ComponentModel;

namespace gamelauncher
{
    public partial class App : Application
    {
        // Ресурсы, которые нужно освободить
        private CancellationTokenSource _cts;
        private List<DispatcherTimer> _activeTimers;
        private bool _hasBackgroundThreads;
        public bool isFirstTime = true;

        public App()
        {
            _cts = new CancellationTokenSource();
            _activeTimers = new List<DispatcherTimer>();
            _hasBackgroundThreads = false;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ThemeManager.LoadTheme();
            LanguageManager.LoadLanguage();
            ThemeManager.SwitchTheme(ThemeManager.CurrentTheme);
            LanguageManager.SwitchLanguage(LanguageManager.CurrentLanguage);
            if (CurrentUser.TryAutoLogin())
            {
                new MainTemplate().Show();
            }
            else
            {
                new LoginPage().Show();
            }

        }
    }
}