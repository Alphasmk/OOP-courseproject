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
        public static bool IsAdmin = Instance != null ? Instance.IsAdmin : false;        
        public static void Login(User user) 
        {
            Instance = user;
        }

        public static void Logout()
        {
            Instance = null;
        }
    }
}
