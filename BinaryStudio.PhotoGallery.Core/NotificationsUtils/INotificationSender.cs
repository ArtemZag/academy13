namespace BinaryStudio.PhotoGallery.Core.NotificationsUtils
{
    public interface INotificationSender
    {
        void Send(string fromHost, string fromEmail, string fromPassword, string toEmail, string text);
    }
}