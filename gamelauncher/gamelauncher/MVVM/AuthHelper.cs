using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gamelauncher.Views;
using Newtonsoft.Json;

namespace gamelauncher.MVVM
{
    public static class AuthHelper
    {
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "user_auth.json");

        public static void SaveCredentials(string email, string password)
        {
            try
            {
                var data = new AuthData
                {
                    Email = email,
                    Password = password
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

        public static AuthData LoadCredentials()
        {
            if (!File.Exists(FilePath))
                return null;

            try
            {
                var json = File.ReadAllText(FilePath);
                var data = JsonConvert.DeserializeObject<AuthData>(json);

                if (data != null)
                {
                    return data;
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
                    RegisterError good = new RegisterError("Error loading data");
                    good.Show();
                }
            }

            return null;
        }

        public static void DeleteCredentials()
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }
    }

    public class AuthData
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
