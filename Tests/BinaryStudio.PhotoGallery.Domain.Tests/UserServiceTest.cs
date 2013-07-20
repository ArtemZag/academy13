using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using FluentAssertions;
using Microsoft.Practices.Unity;
using NUnit.Framework;

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
            // todo: should be true 
        }

        [Test]
        public void UserShouldBeAdded()
        {
            // setup
            IUnityContainer container = Bootstrapper.Initialise();
            var userService = container.Resolve<IUserService>();

            var userModel = new UserModel
                {
                    Email = "aaa@gmail.com",
                    NickName = "Nick",
                    FirstName = "First",
                    LastName = "Last"
                };

            // body
            bool creationResult = userService.CreateUser(userModel);
            bool checkingResult = userService.CheckUser(userModel.Email);

            // tear down
            creationResult.Should().Be(true);
            checkingResult.Should().Be(true);
        }
    }
}