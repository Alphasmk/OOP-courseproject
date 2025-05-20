using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using gamelauncher.Views;
using Newtonsoft.Json;
using static gamelauncher.MVVM.ThemeManager;

namespace gamelauncher.MVVM
{
    public static class LanguageManager
    {
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "user_language.json");
        public static string CurrentLanguage;
        public static event Action LanguageLoaded;
        public static void SwitchLanguage(string language)
        {
            CurrentLanguage = language;
            var langDict = new ResourceDictionary
            {
                Source = new Uri($"pack://application:,,,/Resources/{language}.xaml", UriKind.Absolute)
            };

            var existingLangDict = Application.Current.Resources.MergedDictionaries
                    .FirstOrDefault(d => d.Source != null &&
                        (d.Source.OriginalString.Contains("en-EN.xaml") ||
                         d.Source.OriginalString.Contains("ru-RU.xaml")));

            if (existingLangDict != null)
                Application.Current.Resources.MergedDictionaries.Remove(existingLangDict);

            Application.Current.Resources.MergedDictionaries.Add(langDict);
            SaveLanguage();
            LanguageLoaded?.Invoke();
        }

        public static void SaveLanguage()
        {
            try
            {
                var data = new LanguageData
                {
                    Language = CurrentLanguage
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
                    RegisterError good = new RegisterError("Error while saving");
                    good.Show();
                }
            }
        }

        public static void LoadLanguage()
        {
            try
            {
                if (!File.Exists(FilePath)) return;

                var json = File.ReadAllText(FilePath);
                var data = JsonConvert.DeserializeObject<LanguageData>(json);

                if (data != null && !string.IsNullOrEmpty(data.Language))
                {
                    CurrentLanguage = data.Language;
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

        public class LanguageData
        {
            public string Language { get; set; }
        }
    }

}
