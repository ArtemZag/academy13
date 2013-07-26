using BinaryStudio.PhotoGallery.Core.NotificationsUtils;
using BinaryStudio.PhotoGallery.Core.UserUtils;
using Microsoft.Practices.Unity;

namespace BinaryStudio.PhotoGallery.Core
{
    public static class Bootstrapper
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<INotificationSender, NotificationSender>();
            container.RegisterType<ICryptoProvider, CryptoProvider>();
        }
    }
}
