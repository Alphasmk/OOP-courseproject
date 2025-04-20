using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace gamelauncher
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // Устанавливаем язык по умолчанию, если он не задан
            if (!Current.Properties.Contains("AppLanguage"))
            {
                Current.Properties["AppLanguage"] = "ru-RU"; // Дефолтный язык
            }

            SetAppLanguage(Current.Properties["AppLanguage"].ToString());
            
        }

        private void SetAppLanguage(string lang)
        {
            CultureInfo culture = new CultureInfo(lang);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(culture.IetfLanguageTag)));
        }
        protected override void OnStartup(StartupEventArgs e)
        {

        }
    }
}
