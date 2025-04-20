using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using gamelauncher.Views;

namespace gamelauncher.Model
{
    public static class DataWorker
    {
        public static bool CreateUser(string email, string password)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                RegisterError error;
                //Проверка, существует ли пользователь
                bool isExist = db.Users.Any(element => element.Email == email);
                if (!isExist)
                {
                    User newUser = new User
                    {
                        Email = email,
                        Password = HashPassword(password),
                        IsAdmin = false,
                        UserName = null,
                        CreateTime = DateTime.Now,
                        Balance = 0,
                        IsBlocked = false
                    };
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    error = new RegisterError("Успешная регистрация");
                    error.ShowDialog();
                    return true;
                }
                error = new RegisterError("Пользователь с таким email уже зарегистрирован");
                error.ShowDialog();
                return false;
            }
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
        public static bool DeleteUser(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                bool isExist = db.Users.Any(element => element == user);
                if(isExist)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public static bool UpdateUserName(User user, string newName)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                bool isExist = db.Users.Any(element => element == user);
                if(isExist)
                {
                    User _user = db.Users.FirstOrDefault(us => us.Id == user.Id);
                    _user.UserName = newName;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public static ICollection<User> GetAllUsers()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var users = db.Users.ToList();
                return users;
            }
        }

        public static bool IsUserExist(string email)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                bool isExist = db.Users.Any(u => u.Email == email);
                return isExist;
            }
        }

        public static User GetUser(string email)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Email == email);
                return user;
            }
        }
    }
}
