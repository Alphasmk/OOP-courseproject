using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using gamelauncher.Views;
using Newtonsoft.Json;

namespace gamelauncher.MVVM
{
    public static class ThemeManager
    {
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "user_theme.json");
        public static string CurrentTheme;
        public static event Action ThemeLoad;
        public static event Action<Type> ThemeLoaded;
        public static void SwitchTheme(string theme)
        {
            CurrentTheme = theme;
            var themeDict = new ResourceDictionary
            {
                Source = new Uri($"pack://application:,,,/Resources/{theme}Theme.xaml", UriKind.Absolute)
            };

            var existingTheme = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Theme"));

            if (existingTheme != null)
                Application.Current.Resources.MergedDictionaries.Remove(existingTheme);

            Application.Current.Resources.MergedDictionaries.Add(themeDict);
            SaveTheme();
            ThemeLoad?.Invoke();
        }

        public static void SwitchTheme(string theme, Type page)
        {
            CurrentTheme = theme;
            var themeDict = new ResourceDictionary
            {
                Source = new Uri($"pack://application:,,,/Resources/{theme}Theme.xaml", UriKind.Absolute)
            };

            var existingTheme = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Theme"));

            if (existingTheme != null)
                Application.Current.Resources.MergedDictionaries.Remove(existingTheme);

            Application.Current.Resources.MergedDictionaries.Add(themeDict);
            SaveTheme();
            ThemeLoaded?.Invoke(page);
        }

        public static void SaveTheme()
        {
            try
            {
                var data = new ThemeData
                {
                    Theme = CurrentTheme
                };

                File.WriteAllText(FilePath, JsonConvert.SerializeObject(data));
            }
            catch (Exception ex)
            {
                if (LanguageManager.CurrentLanguage == "ru-RU")
                {
                    RegisterError error = new RegisterError("Ошибка при сохранении");
                    error.ShowDialog();
                }
                else
                {
                    RegisterError error = new RegisterError("Error while saving");
                    error.ShowDialog();
                }
            }
        }

        public static void LoadTheme()
        {

            try
            {
                var json = File.ReadAllText(FilePath);
                var data = JsonConvert.DeserializeObject<ThemeData>(json);

                if (data != null)
                {
                   SwitchTheme(data.Theme);
                }
            }
            catch (Exception ex)
            {
                if (LanguageManager.CurrentLanguage == "ru-RU")
                {
                    RegisterError error = new RegisterError("Ошибка загрузки данных");
                    error.ShowDialog();
                }
                else
                {
                    RegisterError error = new RegisterError("Error loading data");
                    error.ShowDialog();
                }
            }
        }

        public class ThemeData
        {
            public string Theme { get; set; }
        }
    }
}
