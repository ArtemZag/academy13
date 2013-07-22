using BinaryStudio.PhotoGallery.Core.NotificationsUtils;
using NUnit.Framework;

namespace BinaryStudio.PhotoGalery.Core.Tests
{
    [TestFixture]
    internal class NotificationTest
    {
        [Test]
        public void MessageShouldBeSended()
        {
            // setup 
            string toEmail = "YOUR_EMAIL_TO_TEST";
            string text = "text";

            // tear down
            NotificationSender.Send(toEmail, text);
        }
    }
}
