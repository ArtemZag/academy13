namespace BinaryStudio.PhotoGallery.Core.EmailUtils
{
    public interface IEmailSender
    {
        void Send(string fromHost, string fromEmail, string fromPassword, string toEmail, string theme, string text);
    }
}