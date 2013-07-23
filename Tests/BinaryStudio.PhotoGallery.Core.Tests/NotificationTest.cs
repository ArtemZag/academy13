using System.Configuration;
using BinaryStudio.PhotoGallery.Core.NotificationsUtils;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace BinaryStudio.PhotoGalery.Core.Tests
{
    [TestFixture]
    internal class NotificationTest
    {
        private INotificationSender sender;

        [SetUp]
        public void Setup()
        {
            IUnityContainer container = Bootstrapper.Initialise();
            sender = container.Resolve<INotificationSender>();
        }

        [Test]
        public void MessageShouldBeSended()
        {
            // setup 
            string ownHost = ConfigurationManager.AppSettings["NotificationHost"];

            string ownEmail = ConfigurationManager.AppSettings["NotificationEmail"];
            string ownPassword = ConfigurationManager.AppSettings["NotificationPassword"];

            string toEmail = "antnglvn@gmail.com";
            string text = "text";

            // tear down
            sender.Send(ownHost, ownEmail, ownPassword, toEmail, text);
        }
    }
}
