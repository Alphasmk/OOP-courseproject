using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
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
                if (isExist)
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
                if (isExist)
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

        public static bool CheckBlock(string email)
        {
            using (ApplicationContext context = new ApplicationContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Email == email);
                return user.IsBlocked;
            }
        }

        public static ICollection<Game> GetGames()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Games
                    .Include(g => g.GameGenres)
                    .ThenInclude(gg => gg.Genre)
                    .Include(g => g.GamePlatforms)
                    .ThenInclude(gp => gp.Platform)
                    .ToList();
            }
        }

        public static bool UpdateUser(User updatedUser)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User existingUser = db.Users.FirstOrDefault(u => u.Id == updatedUser.Id);
                if (existingUser != null)
                {
                    existingUser.Email = updatedUser.Email;
                    existingUser.IsAdmin = updatedUser.IsAdmin;
                    existingUser.UserName = updatedUser.UserName;
                    existingUser.Balance = updatedUser.Balance;
                    existingUser.IsBlocked = updatedUser.IsBlocked;

                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
