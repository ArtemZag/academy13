using BinaryStudio.PhotoGallery.Domain.Services;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using FluentAssertions;

namespace BinaryStudio.PhotoGallery.Domain.Tests
{
    [TestFixture]
    internal class UserServiceTest
    {
        [Test]
        public void UserCheckingShouldWork()
        {
            //setup
            IUnityContainer container = Bootstrapper.Initialise();
            var userService = container.Resolve<IUserService>();

            // body
            bool result = userService.CheckUser("abs@dnc.ru");

            // tear down
            result.Should().Be(false);
            // todo: shoule be true 
        }
    }
}