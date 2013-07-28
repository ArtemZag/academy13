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
        private IUserService userService;

        [SetUp]
        public void Setup()
        {
            IUnityContainer container = Bootstrapper.Initialise();
            userService = container.Resolve<IUserService>();
        }

        [Test]
        public void UserShoulBeAbsent()
        {
            // body
            bool result = userService.IsUserExist("nononono@gmail.com");

            // tear down
            result.Should().Be(false);
        }

        [Test]
        public void UserShouldBeAdded()
        {
            // setup
            var userModel = new UserModel
            {
                Email = "bbb@gmail.com",
                UserPassword = "abc123",
                NickName = "Nick",
                FirstName = "First",
                LastName = "Last"
            };

            // body
            userService.CreateUser(userModel);
            bool checkingResult = userService.IsUserExist(userModel.Email);

            // tear down
            checkingResult.Should().Be(true);
        }

        [Test]
        public void UserShouldBeDeleted()
        {
            // setup
            var userModel = new UserModel
            {
                Email = "aaa@gmail.com",
                UserPassword = "abc123",
                NickName = "Bill",
                FirstName = "Billy",
                LastName = "Last"
            };

            // body
            userService.CreateUser(userModel);
            bool isPresentAfterCreation = userService.IsUserExist(userModel.Email);

            userService.DeleteUser(userModel.Email);
            bool isPresentAfterDeleting = userService.IsUserExist(userModel.Email);

            // tear down
            isPresentAfterCreation.Should().Be(true);

            isPresentAfterDeleting.Should().Be(false);
        }
    }
}