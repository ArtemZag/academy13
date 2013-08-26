using BinaryStudio.PhotoGallery.Domain.Services.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace BinaryStudio.PhotoGallery.Domain.Tests
{
    [TestFixture]
    internal class UserMonitorTest
    {
        private readonly IUsersMonitorTask _monitorTask;

        public UserMonitorTest()
        {
            _monitorTask = new UsersMonitorTask();
        }

        [Test]
        public void UserShouldBeOffline()
        {
            // setup
            const int USER_ID = 1;

            // tear down
            _monitorTask.IsOnline(USER_ID).Should().Be(false);
        }

        [Test]
        public void UserShouldBeOfflineAfterLongTimeInactivity()
        {
            // setup
            const int USER_ID = 1;

            // body 
            _monitorTask.SetOnline(USER_ID);
            _monitorTask.Period = 2;

            for (int i = 0; i < 16/2; i++)
            {
                _monitorTask.Execute();
            }

            // tear down
            _monitorTask.IsOnline(USER_ID).Should().Be(false);
        }

        [Test]
        public void UserShouldBeOnline()
        {
            // setup
            const int USER_ID = 1;

            // body
            _monitorTask.SetOnline(USER_ID);

            // tear down 
            _monitorTask.IsOnline(USER_ID).Should().Be(true);
        }
    }
}