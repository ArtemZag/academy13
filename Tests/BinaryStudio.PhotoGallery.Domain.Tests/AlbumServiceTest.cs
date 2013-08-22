using System;
using System.Collections.ObjectModel;
using System.Linq;
using BinaryStudio.PhotoGallery.Core.UserUtils;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Domain.Tests.Mocked;
using BinaryStudio.PhotoGallery.Models;
using FluentAssertions;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace BinaryStudio.PhotoGallery.Domain.Tests
{
    [TestFixture]
    internal class AlbumServiceTest
    {
        private readonly IAlbumService albumService;
        private readonly IUserService userService;

        public AlbumServiceTest()
        {
            IUnityContainer container = Bootstrapper.Initialise();

            var cryptoProvidrer = container.Resolve<ICryptoProvider>();
            albumService = container.Resolve<IAlbumService>();
            var workFactory = new TestUnitOfWorkFactory();

//            _albumService = new AlbumService(workFactory);
            userService = new UserService(workFactory, cryptoProvidrer, albumService);
        }

        [Test]
        public void AlbumShouldBeAdded()
        {
            // setup
            var userModel = new UserModel
                {
                    Email = "test1@gmail.com",
                    UserPassword = "abc123",
                    FirstName = "First",
                    LastName = "Last",
                    Albums = new Collection<AlbumModel>()
                };

            var albumModel = new AlbumModel
                {
                    Name = "name",
                    DateOfCreation = DateTime.Now,
                    Description = "description"
                };

            // body
            userService.CreateUser(userModel);
            albumService.CreateAlbum(userModel.Id, albumModel);

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
                    FirstName = "First",
                    LastName = "Last",
                    Albums = new Collection<AlbumModel>()
                };

            var albumModel = new AlbumModel
                {
                    Name = "name",
                    DateOfCreation = DateTime.Now,
                    Description = "description"
                };

            // body
            userService.CreateUser(userModel);
            albumService.CreateAlbum(userModel.Id, albumModel);
            int deletedAlbumsAfterCreation =
                userModel.Albums.Select(model => model).Count(model => model.IsDeleted);

            albumService.DeleteAlbum(userModel.Id, albumModel.Id);
            int deletedAlbumsAfterDeleting = userModel.Albums.Select(model => model).Count(model => model.IsDeleted);

            // tear down
            deletedAlbumsAfterCreation.Should().Be(0);
            deletedAlbumsAfterDeleting.Should().Be(1);
        }
    }
}