using System;
using System.Collections.ObjectModel;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using FluentAssertions;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace BinaryStudio.PhotoGallery.Domain.Tests
{
    [TestFixture]
    public class AlbumServiceTest
    {
        [SetUp]
        public void Setup()
        {
            IUnityContainer container = Bootstrapper.Initialise();

            albumService = container.Resolve<IAlbumService>();
            userService = container.Resolve<IUserService>();
        }

        private IAlbumService albumService;
        private IUserService userService;

        [Test]
        public void AlbumShouldBeAdded()
        {
            // setup
            var userModel = new UserModel
                {
                    Email = "test1@gmail.com",
                    UserPassword = "abc123",
                    NickName = "Nick",
                    FirstName = "First",
                    LastName = "Last",
                    Albums = new Collection<AlbumModel>()
                };

            var albumModel = new AlbumModel
                {
                    AlbumName = "name",
                    DateOfCreation = DateTime.Now,
                    Description = "description"
                };

            // body
            userService.CreateUser(userModel);
            albumService.CreateAlbum(userModel.Email, albumModel);

            // tear down
            userModel.Albums.Count.Should().Be(1);
        }

        [Test]
        public void AlbumShouldBeDeleted()
        {
            var userModel = new UserModel
                {
                    Email = "test2@gmail.com",
                    UserPassword = "abc123",
                    NickName = "Nick",
                    FirstName = "First",
                    LastName = "Last",
                    Albums = new Collection<AlbumModel>()
                };

            var albumModel = new AlbumModel
                {
                    AlbumName = "name",
                    DateOfCreation = DateTime.Now,
                    Description = "description"
                };

            // body
            userService.CreateUser(userModel);
            albumService.CreateAlbum(userModel.Email, albumModel);
            int countAfterAlbumCreation = userModel.Albums.Count;

            albumService.DeleteAlbum(userModel.Email, albumModel.AlbumName);
            int countAfterAlbumDeleting = userModel.Albums.Count;

            // tear down
            countAfterAlbumCreation.Should().Be(1);
            countAfterAlbumDeleting.Should().Be(0);
        }
    }
}