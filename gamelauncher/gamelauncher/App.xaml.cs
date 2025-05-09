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

            // Установка языка
            if (!Current.Properties.Contains("AppLanguage"))
            {
                Current.Properties["AppLanguage"] = "ru-RU";
            }
            SetAppLanguage(Current.Properties["AppLanguage"].ToString());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (CurrentUser.TryAutoLogin())
            {
                new MainTemplate().Show();
            }
            else
            {
                new LoginPage().Show();
            }
        }

        private void SetAppLanguage(string lang)
        {
            CultureInfo culture = new CultureInfo(lang);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(culture.IetfLanguageTag)));
        }
    }
}