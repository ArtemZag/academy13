using System.Net;
using System.Net.Mail;

namespace BinaryStudio.PhotoGallery.Core.NotificationsUtils
{
    public static class NotificationSender
    {
        private const string OWN_EMAIL = "binarystudio.gallery@outlook.com";
        private const string HOST = "smtp.live.com";

        // todo: maybe it's wrong
        private const string PASSWORD = "s5PCqNQn";

        private const string SUBJECT = "Notification";

        public static void Send(string email, string text)
        {
            var message = new MailMessage
                {
                    From = new MailAddress(OWN_EMAIL)
                };

            message.To.Add(email);

            message.Subject = SUBJECT;

            message.Body = text;
            message.IsBodyHtml = true;

            //SMTP client
            var smtpClient = new SmtpClient(HOST)
                {
                    Port = 587,
                    Credentials = new NetworkCredential(OWN_EMAIL, PASSWORD),
                    EnableSsl = true
                };

            smtpClient.Send(message);
        }
    }
}