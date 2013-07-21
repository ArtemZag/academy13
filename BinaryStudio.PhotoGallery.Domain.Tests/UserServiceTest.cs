﻿using BinaryStudio.PhotoGallery.Database;
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
            System.Data.Entity.Database.SetInitializer(new DatabaseInitializer());

            IUnityContainer container = Bootstrapper.Initialise();
            userService = container.Resolve<IUserService>();
        }

        [Test]
        public void UserShoulBeAbsent()
        {
            // body
            bool result = userService.CheckUser("aaa@gmail.com");

            // tear down
            result.Should().Be(false);
        }

        [Test]
        public void UserShouldBePresent()
        {
            // body
            bool result = userService.CheckUser("Maaak@gmail.com");

            // tear down
            result.Should().Be(true);
        }

        [Test]
        public void UserShouldBeAdded()
        {
            // setup
            var userModel = new UserModel
            {
                Email = "aaa@gmail.com",
                NickName = "Nick",
                FirstName = "First",
                LastName = "Last"
            };

            // body
            userService.CreateUser(userModel);
            bool checkingResult = userService.CheckUser(userModel.Email);

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
                NickName = "Nick",
                FirstName = "First",
                LastName = "Last"
            };

            // body
            userService.CreateUser(userModel);
            bool isPresentAfterCreation = userService.CheckUser(userModel.Email);

            userService.DeleteUser(userModel);
            bool isPresentAfterDeleting = userService.CheckUser(userModel.Email);

            // tear down
            isPresentAfterCreation.Should().Be(true);

            isPresentAfterDeleting.Should().Be(false);
        }
    }
}