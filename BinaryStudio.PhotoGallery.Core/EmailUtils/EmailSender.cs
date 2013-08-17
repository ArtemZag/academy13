using System.Net;
using System.Net.Mail;

namespace BinaryStudio.PhotoGallery.Core.EmailUtils
{
    internal class EmailSender : IEmailSender
    {
        public void Send(string fromHost, string fromEmail, string fromPassword, string toEmail, string subject, string text)
        {
            var message = new MailMessage
            {
                From = new MailAddress(fromEmail)
            };

            message.To.Add(toEmail);

            message.Subject = subject;

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