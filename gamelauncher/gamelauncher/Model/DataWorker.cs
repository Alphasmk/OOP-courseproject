using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using gamelauncher.Views;
using gamelauncher.ViewModels;
using System.Collections.ObjectModel;
using gamelauncher.MVVM;
using System.Runtime.Remoting.Contexts;

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

        public static bool DeleteGame(Game game)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                bool isExist = db.Games.Any(element => element == game);
                if (isExist)
                {
                    db.Games.Remove(game);
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

        public static ICollection<Game> GetAllGames()
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

        public static ObservableCollection<GenreViewModel> GetAllGenres()
        {
            var genresCollection = new ObservableCollection<GenreViewModel>();
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var genre in db.Genres)
                {
                    genresCollection.Add(new GenreViewModel
                    {
                        Id = genre.Id,
                        Name = genre.Name,
                        IsSelected = false
                    });
                }
                return genresCollection;
            }
        }

        public static ObservableCollection<PlatformViewModel> GetAllPlatforms()
        {
            var platformsCollection = new ObservableCollection<PlatformViewModel>();
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var platform in db.Platforms)
                {
                    platformsCollection.Add(new PlatformViewModel
                    {
                        Id = platform.Id,
                        Name = platform.Name,
                        IsSelected = false
                    });
                }
                return platformsCollection;
            }
        }

        public static ObservableCollection<ImageItemViewModel> GetAllImages(int id, Action<ImageItemViewModel> OnDeleteAction)
        {
            var imagesCollection = new ObservableCollection<ImageItemViewModel>();
            using (ApplicationContext db = new ApplicationContext())
            {
                var gameImages = db.GameImages.Where(w => w.GameId == id).ToList();
                if(gameImages.Any())
                {
                    foreach (var gameImage in gameImages)
                    {
                        imagesCollection.Add(new ImageItemViewModel(OnDeleteAction)
                        {
                            GameImage = gameImage,
                            ImagePath = gameImage.ImagePath
                        });
                    }
                }
            }
            return imagesCollection;
        }

        public static bool isGameLiked(int id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var user = db.Users
            .Include(u => u.Wishlists)
            .FirstOrDefault(u => u.Id == CurrentUser.Instance.Id);

                if (user == null || user.Wishlists == null)
                {
                    return false;
                }

                return user.Wishlists.Any(w => w.GameId == id);
            }
        }

        public static bool isGameBought(int id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var user = db.Users
                    .Include(u => u.Library)
                    .FirstOrDefault(u => u.Id == CurrentUser.Instance.Id);
                if (user == null || user.Library == null)
                {
                    return false;
                }
                return user.Library.Any(w => w.GameId == id);
            }
        }

        public static void ChangeLikeState(int id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                int userId = CurrentUser.Instance.Id;
                var existingWish = db.Wishlists
                    .FirstOrDefault(w => w.UserId == userId && w.GameId == id);

                if (existingWish != null)
                {
                    db.Wishlists.Remove(existingWish);
                }
                else
                {
                    var newWish = new Wishlist
                    {
                        UserId = userId,
                        GameId = id,
                        AddedDate = DateTime.Now
                    };
                    db.Wishlists.Add(newWish);
                }
                db.SaveChanges();
            }
        }

        public static void BuyGame(int id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                int userId = CurrentUser.Instance.Id;
                var boughtGame = new Library
                {
                    UserId = userId,
                    GameId = id,
                    DateOfPurchase = DateTime.Now
                };
                var gamePurchase = new Purchase
                {
                    UserId = userId,
                    GameId = id,
                    PricePaid = db.Games.FirstOrDefault(w => w.Id == id).Price,
                    PurchaseDate = DateTime.Now
                };
                db.Library.Add(boughtGame);
                db.Purchases.Add(gamePurchase);
                db.SaveChanges();
            }
        }
    }
}
