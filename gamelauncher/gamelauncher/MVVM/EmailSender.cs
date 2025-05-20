using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace gamelauncher.MVVM
{
    public static class EmailSender
    {
        public static void SendMessage(string userEmail, string title, string text)
        {
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
                        mailMessage.To.Add(userEmail);
                        mailMessage.Subject = title;
                        mailMessage.IsBodyHtml = true;

                        string htmlBody = text;

                        AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");

                        string logoPath = @"H:\Уник\ООП\Курсач\gamelauncher\gamelauncher\img\logo.png";

                        LinkedResource logo = new LinkedResource(logoPath, "image/png");
                        logo.ContentId = "gamelauncherlogo";
                        htmlView.LinkedResources.Add(logo);
                        mailMessage.AlternateViews.Add(htmlView);
                        smtpClient.Send(mailMessage);
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
}
