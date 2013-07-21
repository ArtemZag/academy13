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
        public void UserShoulBeAbsent()
        {
            // setup
            IUnityContainer container = Bootstrapper.Initialise();
            var userService = container.Resolve<IUserService>();

            // body
            bool result = userService.CheckUser("aaa@gmail.com");

            // tear down
            result.Should().Be(false);
        }

        [Test]
        public void UserShouldBePresent()
        {
            // setup 
            IUnityContainer container = Bootstrapper.Initialise();
            var userService = container.Resolve<IUserService>();

            // body
            bool result = userService.CheckUser("Maaak@gmail.com");

            // tear down
            result.Should().Be(true);
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

        [Test]
        public void UserShouldBeDeleted()
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
            bool isPresentAfterCreation = userService.CheckUser(userModel.Email);

            bool deletingResult = userService.DeleteUser(userModel);
            bool isPresentAfterDeleting = userService.CheckUser(userModel.Email);

            // tear down
            creationResult.Should().Be(true);
            isPresentAfterCreation.Should().Be(true);

            deletingResult.Should().Be(true);
            isPresentAfterDeleting.Should().Be(false);
        }
    }
}