using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using gamelauncher.Model;
using gamelauncher.MVVM;
using gamelauncher.Views;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Net.NetworkInformation;

namespace gamelauncher.ViewModels
{
    public class RestoreViewModel : INotifyPropertyChanged
    {
        private string _inputText;
        private string _email;
        private bool _isRestoring;
        private string _code;
        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public bool IsRestoring
        {
            get => _isRestoring;
            set
            {
                _isRestoring = value;
                OnPropertyChanged();
            }
        }
        public ICommand CloseCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand RestoreCommand { get; }
        public RestoreViewModel(Action closeAction)
        {
            IsRestoring = false;
            CloseCommand = new RelayCommand(_ => closeAction());
            NextCommand = new RelayCommand(async _ => await Restore(), _ => Check());
            RestoreCommand = new RelayCommand(async _ => await RestorePassword());
        }

        private async Task RestorePassword()
        {
            if(InputText == _code)
            {
                Random random = new Random();
                string newPass;
                const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
                const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                const string digits = "0123456789";
                const string allChars = lowerChars + upperChars + digits;

                char[] code = new char[10];
                code[0] = upperChars[random.Next(upperChars.Length)];

                for (int i = 1; i < 10; i++)
                {
                    code[i] = allChars[random.Next(allChars.Length)];
                }

                newPass = new string(code.OrderBy(x => random.Next()).ToArray());
                string smtpServer = "smtp.mail.ru";
                int smtpPort = 587;
                string smtpUsername = "gamelauncher@internet.ru";
                string smtpPassword = "H9QKamTArRPi3A3cDE7A";

                var user = DataWorker.GetUser(Email);
                user.Password = DataWorker.HashPassword(newPass);
                DataWorker.UpdateUserPassword(user);

                try
                {
                    using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                    {
                        smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                        smtpClient.EnableSsl = true;

                        using (MailMessage mailMessage = new MailMessage())
                        {
                            mailMessage.From = new MailAddress(smtpUsername);
                            mailMessage.To.Add(Email);
                            mailMessage.Subject = "Восстановление пароля";
                            mailMessage.IsBodyHtml = true;

                            string htmlBody = $@"
<table width='100%' border='0' cellspacing='0' cellpadding='0'>
    <tr>
        <td align='center'>
            <table width='600' border='0' cellspacing='0' cellpadding='20' style='background: #f8f8f8;'>
                <tr>
                    <td align='center'>
                        <img src='cid:companyLogo' width='100' style='display: block;'/>
                        <h1 style='font-family: Arial; color: #333;'>Восстановление пароля от аккаунта</h1>
                        <h2 style='font-family: Arial; color: #333;'>Ваш новый пароль:</h2>
                        <p style='font-size: 18px;'>{newPass}</p>
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
                            if (LanguageManager.CurrentLanguage == "ru-RU")
                            {
                                RegisterError err = new RegisterError("Новый пароль выслан на Email");
                                err.ShowDialog();
                            }
                            else
                            {
                                RegisterError err = new RegisterError("New password sent to email");
                                err.ShowDialog();
                            }
                            CloseCommand.Execute(true);

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
            else
            {
                if (LanguageManager.CurrentLanguage == "ru-RU")
                {
                    RegisterError err = new RegisterError("Неверный проверочный код");
                    err.ShowDialog();
                }
                else
                {
                    RegisterError err = new RegisterError("Invalid verification code");
                    err.ShowDialog();
                }
            }
        }

        private async Task Restore()
        {
            bool isOk;
            try
            {
                using (var ping = new Ping())
                {
                    var reply = ping.Send("8.8.8.8", 3000);
                    isOk = reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                isOk = false;
            }
            if(!isOk)
            {
                RegisterError err = new RegisterError("Проверьте интернет-соединение");
                err.ShowDialog();
            }
            else if (DataWorker.IsUserExist(InputText))
            {
                IsRestoring = true;
                if (LanguageManager.CurrentLanguage == "ru-RU")
                {
                    RegisterError err = new RegisterError("На почту отправлен проверочный код");
                    err.ShowDialog();
                }
                else
                {
                    RegisterError err = new RegisterError("A verification code has been sent to your email");
                    err.ShowDialog();
                }
                Random random = new Random();
                string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string code = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
                _code = code;
                _email = InputText;
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
                            mailMessage.To.Add(InputText);
                            mailMessage.Subject = "Восстановление пароля";
                            mailMessage.IsBodyHtml = true;
                            InputText = string.Empty;

                            string htmlBody = $@"
<table width='100%' border='0' cellspacing='0' cellpadding='0'>
    <tr>
        <td align='center'>
            <table width='600' border='0' cellspacing='0' cellpadding='20' style='background: #f8f8f8;'>
                <tr>
                    <td align='center'>
                        <img src='cid:companyLogo' width='100' style='display: block;'/>
                        <h1 style='font-family: Arial; color: #333;'>Восстановление пароля от аккаунта</h1>
                        <h2 style='font-family: Arial; color: #333;'>Проверочный код:</h2>
                        <p style='font-size: 18px;'>{code}</p>
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
            else
            {
                IsRestoring = false;
                if (LanguageManager.CurrentLanguage == "ru-RU")
                {
                    RegisterError err = new RegisterError("Пользователя с таким Email не существует");
                    err.ShowDialog();
                }
                else
                {
                    RegisterError err = new RegisterError("User with such email does not exist");
                    err.ShowDialog();
                }
            }
        }

        private bool Check()
        {
            if(string.IsNullOrWhiteSpace(InputText))
            {
                return false;
            }
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
