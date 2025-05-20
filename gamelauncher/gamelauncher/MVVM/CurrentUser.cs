using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gamelauncher.Model;

namespace gamelauncher.MVVM
{
    public static class CurrentUser
    {
        public static User Instance { get; set; }
        public static bool IsLoggedIn => Instance != null;
        public static event Action UserChanged;
        public static bool IsAdmin;
        public static void Login(User user) 
        {
            Instance = user;
            IsAdmin = user.IsAdmin;
            AuthHelper.SaveCredentials(user.Email, user.Password);
        }

        public static void OnUserChanged()
        {
            UserChanged?.Invoke();
        }

        public static void Logout()
        {
            AuthHelper.DeleteCredentials();
            Instance = null;
        }

        public static bool TryAutoLogin()
        {
            var authData = AuthHelper.LoadCredentials();
            if (authData == null)
                return false;

            var user = DataWorker.GetUser(authData.Email);
            if (user != null)
            {
                Login(user);
                return true;
            }

            return false;
        }
    }
}
