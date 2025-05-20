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
using CommonWin32.API;
using System.Net.Mail;
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Net.NetworkInformation;

namespace gamelauncher.Model
{
    public static class DataWorker
    {
        public static async Task CreateUser(string email, string password)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                RegisterError error;
                bool isExist = db.Users.Any(element => element.Email == email);
                if (!isExist)
                {
                    if(LanguageManager.CurrentLanguage == "ru-RU")
                    {
                        error = new RegisterError("Успешная регистрация\nДанные отправлены на почту");
                        error.ShowDialog();
                    }
                    else
                    {
                        error = new RegisterError("Successful registration\nData sent to email");
                        error.ShowDialog();
                    }
                    User newUser = new User
                    {
                        Email = email,
                        Password = HashPassword(password),
                        IsAdmin = false,
                        UserName = null,
                        CreateTime = DateTime.Now,
                        Balance = 0,
                        IsBlocked = false,
                        SnakeRecord = 0,
                    };
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    string smtpServer = "smtp.mail.ru";
                    int smtpPort = 587;
                    string smtpUsername = "gamelauncher@internet.ru";
                    string smtpPassword = "H9QKamTArRPi3A3cDE7A";

                    try
                    {
                        using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                        {
                            smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                            smtpClient.EnableSsl = true;

                            using (MailMessage mailMessage = new MailMessage())
                            {
                                mailMessage.From = new MailAddress(smtpUsername);
                                mailMessage.To.Add(email);
                                mailMessage.Subject = "Создание аккаунта";
                                mailMessage.IsBodyHtml = true;

                                string htmlBody = $@"
<table width='100%' border='0' cellspacing='0' cellpadding='0'>
    <tr>
        <td align='center'>
            <table width='600' border='0' cellspacing='0' cellpadding='20' style='background: #f8f8f8;'>
                <tr>
                    <td align='center'>
                        <img src='cid:companyLogo' width='100' style='display: block;'/>
                        <h1 style='font-family: Arial; color: #333;'>Добро пожаловать!</h1>
                        <h2 style='font-family: Arial; color: #333;'>Данные для входа:</h2>
                        <p style='font-size: 18px;'>Логин: {email}</p>
                        <p style='font-size: 18px;'>Пароль: {password}</p>
                        <p style='font-size: 16px;'>Спасибо за регистрацию!</p>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
";

                                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");

                                string logoPath = @"H:\Уник\ООП\Курсач\gamelauncher\gamelauncher\img\logo.png";

                                LinkedResource logo = new LinkedResource(logoPath, "image/png");
                                logo.ContentId = "companyLogo";
                                htmlView.LinkedResources.Add(logo);

                                mailMessage.AlternateViews.Add(htmlView);

                                await smtpClient.SendMailAsync(mailMessage);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Ошибка: {ex.Message}");
                        if (ex.InnerException != null)
                        {
                            Console.WriteLine($"🔧 Подробности: {ex.InnerException.Message}");
                        }
                    }
                }
                if(LanguageManager.CurrentLanguage == "ru-RU")
                {
                    error = new RegisterError("Пользователь с таким email уже зарегистрирован");
                    error.ShowDialog();
                }
                else
                {
                    error = new RegisterError("A user with this email is already registered");
                    error.ShowDialog();
                }
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

        public static bool UpdateUserPassword(string newPassword)
        {

            using (ApplicationContext db = new ApplicationContext())
            {
                int userId = CurrentUser.Instance.Id;
                User user = db.Users.FirstOrDefault(u => u.Id == userId);

                if (user == null)
                {
                    return false;
                }

                string newHashedPassword = HashPassword(newPassword);
                user.Password = newHashedPassword;
                db.SaveChanges();
                return true;
            }
        }

        public static bool UpdateUserName(string newName)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                int userId = CurrentUser.Instance.Id;
                User _user = db.Users.FirstOrDefault(u => u.Id == userId);
                if (_user != null)
                {
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
                    existingUser.SnakeRecord = updatedUser.SnakeRecord;

                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public static bool UpdateUserPassword(User updatedUser)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User existingUser = db.Users.FirstOrDefault(u => u.Id == updatedUser.Id);
                if (existingUser != null)
                {
                    existingUser.Password = updatedUser.Password;
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
                if (gameImages.Any())
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

        public static async Task BuyGame(int id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                int userId = CurrentUser.Instance.Id;
                var wishlistItem = db.Wishlists
                    .FirstOrDefault(w => w.UserId == userId && w.GameId == id);

                if (wishlistItem != null)
                {
                    db.Wishlists.Remove(wishlistItem);
                }

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
                string smtpServer = "smtp.mail.ru";
                int smtpPort = 587;
                string smtpUsername = "gamelauncher@internet.ru";
                string smtpPassword = "H9QKamTArRPi3A3cDE7A";
                string gameTitle = db.Games.FirstOrDefault(g => g.Id == id).Title;
                string gameLogo = db.Games.FirstOrDefault(g => g.Id == id).LogoImagePath;
                try
                {
                    using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                    {
                        smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                        smtpClient.EnableSsl = true;

                        using (MailMessage mailMessage = new MailMessage())
                        {
                            mailMessage.From = new MailAddress(smtpUsername);
                            mailMessage.To.Add(CurrentUser.Instance.Email);
                            mailMessage.Subject = "Покупка игры";
                            mailMessage.IsBodyHtml = true;

                            string htmlBody = $@"
<table width='100%' border='0' cellspacing='0' cellpadding='0'>
    <tr>
        <td align='center'>
            <table width='600' border='0' cellspacing='0' cellpadding='20' style='background: #f8f8f8;'>
                <tr>
                    <td align='center'>
                        <img src='cid:companyLogo' width='100' style='display: block'; margin-bottom='20'/>
                        <img src='cid:gameLogo' height='80' style='display: block;'/>
                        <h1 style='font-family: Arial; color: #333;'>Спасибо за покупку!</h1>
                        <h3 style='font-family: Arial; color: #333;'>Вы приобрели игру «{gameTitle}»</h3>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
";

                            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");

                            string logoPath = @"H:\Уник\ООП\Курсач\gamelauncher\gamelauncher\img\logo.png";
                            string gameLogoPath = gameLogo;

                            LinkedResource logo = new LinkedResource(logoPath, "image/png");
                            logo.ContentId = "companyLogo";
                            htmlView.LinkedResources.Add(logo);

                            LinkedResource gameLogoRes = new LinkedResource(gameLogoPath, "image/png");
                            gameLogoRes.ContentId = "gameLogo";
                            htmlView.LinkedResources.Add(gameLogoRes);
                            mailMessage.AlternateViews.Add(htmlView);

                            await smtpClient.SendMailAsync(mailMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Ошибка: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"🔧 Подробности: {ex.InnerException.Message}");
                    }
                }
            }
        }

        public static List<Purchase> GetUserPurchases()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = db.Purchases.Where(p => p.UserId == CurrentUser.Instance.Id).ToList();
                return list;
            }
        }

        public static string GetGameNameById(int Id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Games.FirstOrDefault(g => g.Id == Id).Title;
            }
        }

        public static ObservableCollection<GameImage> LoadImages(int id)
        {
            var imagesCollection = new ObservableCollection<GameImage>();
            using (ApplicationContext db = new ApplicationContext())
            {
                var gameImages = db.GameImages.Where(w => w.GameId == id).ToList();
                if (gameImages.Any())
                {
                    foreach (var image in gameImages)
                    {
                        imagesCollection.Add(image);
                    }
                }
            }
            return imagesCollection;
        }

        public static List<Platform> GetPlatformsByGameId(int gameId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.GamePlatforms
                    .Where(gp => gp.GameId == gameId)
                    .Include(gp => gp.Platform)
                    .Select(gp => gp.Platform)
                    .ToList();
            }
        }

        public static List<Genre> GetGenresByGameId(int gameId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.GameGenres
                    .Where(gg => gg.GameId == gameId)
                    .Include(gg => gg.Genre)
                    .Select(gg => gg.Genre)
                    .ToList();
            }
        }

        public static bool CreateGroupForUser(string groupName)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                int userId = CurrentUser.Instance.Id;

                bool groupExists = db.UserGameGroups
                    .Any(g => g.UserId == userId && g.Name.ToLower() == groupName.ToLower());

                if (groupExists || string.IsNullOrWhiteSpace(groupName))
                    return false;

                var newGroup = new UserGameGroup
                {
                    Name = groupName,
                    UserId = userId
                };

                db.UserGameGroups.Add(newGroup);
                db.SaveChanges();
                return true;
            }
        }

        public static List<UserGameGroup> GetAllUserGroups()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                int userId = CurrentUser.Instance.Id;

                return db.UserGameGroups
                         .Where(g => g.UserId == userId)
                         .ToList();
            }
        }

        public static ObservableCollection<UserGameGroup> GetAllGameGroups(int gameId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                int userId = CurrentUser.Instance.Id;

                var groups = db.UserGameGroupGames
                    .Where(gg => gg.GameId == gameId && gg.UserGameGroup.UserId == userId)
                    .Select(gg => gg.UserGameGroup)
                    .Distinct()
                    .ToList();

                return new ObservableCollection<UserGameGroup>(groups);
            }
        }

        public static void AddGameToGroup(int userId, int gameId, string groupName)
        {
            using (var db = new ApplicationContext())
            {
                var group = db.UserGameGroups
                    .FirstOrDefault(g => g.UserId == userId && g.Name == groupName);

                var gameExists = db.UserGameGroupGames
                    .Any(gg => gg.UserGameGroupId == group.Id && gg.GameId == gameId);

                if (!gameExists)
                {
                    var groupGame = new UserGameGroupGame
                    {
                        UserGameGroupId = group.Id,
                        GameId = gameId
                    };
                    db.UserGameGroupGames.Add(groupGame);
                }

                db.SaveChanges();
            }
        }
        public static bool AddGameToGroupById(int groupId, int gameId)
        {
            using (var db = new ApplicationContext())
            {
                var group = db.UserGameGroups
                    .FirstOrDefault(g => g.Id == groupId && g.UserId == CurrentUser.Instance.Id);

                if (group == null)
                {
                    if(LanguageManager.CurrentLanguage == "ru-RU")
                    {
                        RegisterError notgood = new RegisterError("Ошибка при добавлении");
                        notgood.Show();
                    }
                    else
                    {
                        RegisterError notgood = new RegisterError("Error adding");
                        notgood.Show();
                    }
                    return false;
                }

                var gameExists = db.Games.Any(g => g.Id == gameId);
                if (!gameExists)
                {
                    if (LanguageManager.CurrentLanguage == "ru-RU")
                    {
                        RegisterError notgood = new RegisterError("Ошибка при добавлении");
                        notgood.Show();
                    }
                    else
                    {
                        RegisterError notgood = new RegisterError("Error adding");
                        notgood.Show();
                    }
                    return false;
                }

                var alreadyInGroup = db.UserGameGroupGames
                    .Any(gg => gg.UserGameGroupId == groupId && gg.GameId == gameId);

                if (alreadyInGroup)
                {
                    if (LanguageManager.CurrentLanguage == "ru-RU")
                    {
                        RegisterError notgood = new RegisterError("Игра уже в группе");
                        notgood.Show();
                    }
                    else
                    {
                        RegisterError notgood = new RegisterError("The game is already in the group");
                        notgood.Show();
                    }
                    return false;
                }

                db.UserGameGroupGames.Add(new UserGameGroupGame
                {
                    UserGameGroupId = groupId,
                    GameId = gameId
                });

                db.SaveChanges();
                if (LanguageManager.CurrentLanguage == "ru-RU")
                {
                    RegisterError good = new RegisterError("Успешно добавлена");
                    good.Show();
                }
                else
                {
                    RegisterError good = new RegisterError("Successfully added");
                    good.Show();
                }
                return true;
            }
        }

    }
}
