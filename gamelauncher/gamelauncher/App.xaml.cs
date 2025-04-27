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
using System.Collections.Generic; // Для DispatcherTimer

namespace gamelauncher
{
    public partial class App : Application
    {
        // Ресурсы, которые нужно освободить
        private CancellationTokenSource _cts;
        private List<DispatcherTimer> _activeTimers;
        private bool _hasBackgroundThreads;

        public App()
        {
            // Инициализация ресурсов
            _cts = new CancellationTokenSource();
            _activeTimers = new List<DispatcherTimer>();
            _hasBackgroundThreads = false;

            // Установка языка
            if (!Current.Properties.Contains("AppLanguage"))
            {
                Current.Properties["AppLanguage"] = "ru-RU";
            }
            SetAppLanguage(Current.Properties["AppLanguage"].ToString());

            // Обработчики событий приложения
            this.Exit += OnApplicationExit;
            this.DispatcherUnhandledException += OnUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Здесь можно инициализировать дополнительные ресурсы
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

        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            // 1. Отмена всех фоновых операций
            _cts?.Cancel();

            // 2. Остановка всех таймеров
            foreach (var timer in _activeTimers)
            {
                timer.Stop();
            }
            _activeTimers.Clear();

            // 3. Принудительное завершение, если есть фоновые потоки
            if (_hasBackgroundThreads)
            {
                Environment.Exit(0);
            }
        }

        private void OnUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Логирование ошибки
            string errorMessage = $"Произошла ошибка: {e.Exception.Message}";
            File.AppendAllText("error.log", $"{DateTime.Now}: {errorMessage}\n{e.Exception.StackTrace}\n");

            // Показ сообщения об ошибке
            MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            // Продолжаем работу приложения
            e.Handled = true;
        }

        // Методы для управления ресурсами из других частей приложения
        public void RegisterTimer(DispatcherTimer timer)
        {
            _activeTimers.Add(timer);
        }

        public CancellationToken GetAppCancellationToken()
        {
            return _cts.Token;
        }

        public void SetBackgroundThreadsFlag(bool hasThreads)
        {
            _hasBackgroundThreads = hasThreads;
        }
    }
}