using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BinaryStudio.PhotoGallery.Core.UserUtils;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Domain.Tests.Mocked;
using BinaryStudio.PhotoGallery.Models;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace BinaryStudio.PhotoGallery.Domain.Tests
{
    [TestFixture]
    internal class PhotoServiceTest
    {
        private readonly IPhotoService _photoService;
        private readonly IUserService _userService;
        private readonly IAlbumService _albumService;

        private readonly IUnitOfWorkFactory _workFactory;

        public PhotoServiceTest()
        {
            IUnityContainer container = Bootstrapper.Initialise();

            var cryptoProvider = container.Resolve<ICryptoProvider>();
            var secureService = container.Resolve<ISecureService>();

            _workFactory = new TestUnitOfWorkFactory();

            _photoService = new PhotoService(_workFactory, secureService);
            _userService = new UserService(_workFactory, cryptoProvider);
//            _albumService = new AlbumService(_workFactory);
        }

        private IEnumerable<PhotoModel> GetListOfPhotos()
        {
            return Builder<PhotoModel>.CreateListOfSize(10).Build();
        }

        [Test]
        public void PhotoShouldBeAdded()
        {
            // setup
            var user = new UserModel
                {
                    Id = 1,
                    Email = "some@gmail.com",
                    UserPassword = "abc123",
                    Albums = new Collection<AlbumModel>()
                };

            var album = new AlbumModel
                {
                    Id = 1,
                    UserId = 1,
                    AlbumName = "albumName",
                    Photos = new Collection<PhotoModel>()
                };

            _userService.CreateUser(user);
            _albumService.CreateAlbum(user.Email, album);

            var photo = new PhotoModel
                {
                    Id = 1
                };

            // body
            int photosCountBeforeAdd = album.Photos.Count;

            _photoService.AddPhoto("some@gmail.com", "albumName", photo);

            int photosCountAfterAdd = album.Photos.Count;

            // tear down
            photosCountBeforeAdd.Should().Be(0);
            photosCountAfterAdd.Should().Be(1);
        }

        [Test]
        public void PhotoShouldBeMarkedAsDeleted()
        {
            // setup
            var photo = new PhotoModel
                {
                    Id = 1
                };

            int deletedPhotosAfterCreation;
            int deletedPhotosBeforeCreation;

            using (IUnitOfWork unitOfWork = _workFactory.GetUnitOfWork())
            {
                unitOfWork.Photos.Add(photo);

                // body
                deletedPhotosAfterCreation = unitOfWork.Photos.Filter(model => model.IsDeleted).Count();
                _photoService.DeletePhoto("ololo@gmail.com", photo);

                deletedPhotosBeforeCreation = unitOfWork.Photos.Filter(model => model.IsDeleted).Count();
            }

            // tear down
            deletedPhotosAfterCreation.Should().Be(0);
            deletedPhotosBeforeCreation.Should().Be(1);
        }

        [Test]
        public void ServiceShouldReturnPhotos()
        {
            // setup
            IEnumerable<PhotoModel> photosToFill = GetListOfPhotos();

            var user = new UserModel
                {
                    Id = 2,
                    Email = "some1@gmail.com",
                    UserPassword = "abc123",
                    Albums = new Collection<AlbumModel>()
                };

            var album = new AlbumModel
                {
                    Id = 2,
                    UserId = 2,
                    AlbumName = "albumName",
                    Photos = new Collection<PhotoModel>()
                };

            _userService.CreateUser(user);
            _albumService.CreateAlbum(user.Email, album);

            _photoService.AddPhotos("some1@gmail.com", "albumName", photosToFill);

            // body
            IEnumerable<PhotoModel> photos = _photoService.GetPhotos("some1@gmail.com", "albumName", 0, 5);
            int count = photos.Count();

            // tear down
            count.Should().Be(5);
        }
    }
}