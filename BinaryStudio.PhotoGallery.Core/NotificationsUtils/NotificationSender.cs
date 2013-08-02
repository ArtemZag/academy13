using System.Net;
using System.Net.Mail;

namespace BinaryStudio.PhotoGallery.Core.NotificationsUtils
{
    internal class NotificationSender : INotificationSender
    {
        public void Send(string fromHost, string fromEmail, string fromPassword, string toEmail, string text)
        {
            var message = new MailMessage
            {
                From = new MailAddress(fromEmail)
            };

            message.To.Add(toEmail);

            message.Subject = "Notification";

            message.Body = text;
            message.IsBodyHtml = true;

            //SMTP client
            var smtpClient = new SmtpClient(fromHost)
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true
            };

            smtpClient.Send(message);
        }
    }
}