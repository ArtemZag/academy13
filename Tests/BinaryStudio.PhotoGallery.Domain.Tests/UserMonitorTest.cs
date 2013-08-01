using BinaryStudio.PhotoGallery.Domain.Services;
using FluentAssertions;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace BinaryStudio.PhotoGallery.Domain.Tests
{
    [TestFixture]
    internal class UserMonitorTest
    {
        [SetUp]
        public void Setup()
        {
            IUnityContainer container = Bootstrapper.Initialise();
            monitorTask = container.Resolve<IUsersMonitorTask>();
        }

        private IUsersMonitorTask monitorTask;

        [Test]
        public void UserShouldBeOffline()
        {
            // setup
            const string EMAIL = "some@gmail.com";

            // tear down
            monitorTask.IsOnline(EMAIL).Should().Be(false);
        }

        [Test]
        public void UserShouldBeOfflineAfterLongTimeInactivity()
        {
            // setup
            const string EMAIL = "some@gmail.com";

            // body 
            monitorTask.SetOnline(EMAIL);
            monitorTask.Period = 2;

            for (int i = 0; i < 16 / 2; i++)
            {
                monitorTask.Execute();
            }

            // tear down
            monitorTask.IsOnline(EMAIL).Should().Be(false);
        }

        [Test]
        public void UserShouldBeOnline()
        {
            // setup
            const string EMAIL = "some@gmail.com";

            // body
            monitorTask.SetOnline(EMAIL);

            // tear down 
            monitorTask.IsOnline(EMAIL).Should().Be(true);
        }
    }
}