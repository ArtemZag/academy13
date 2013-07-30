using System.Text;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Utils;
using BinaryStudio.PhotoGallery.Models;
using Microsoft.Practices.Unity;
using NSubstitute;
using NUnit.Framework;
using FluentAssertions;

namespace BinaryStudio.PhotoGallery.Domain.Tests
{
    [TestFixture]
    public class StorageTest
    {
        private IStorage storage;
        private IUnitOfWorkFactory workFactory;

        private IPathUtil pathUtil;

        [SetUp]
        public void Setup()
        {
            IUnityContainer container = Bootstrapper.Initialise();

            storage = container.Resolve<IStorage>();
            workFactory = container.Resolve<IUnitOfWorkFactory>();

            pathUtil = Substitute.For<IPathUtil>();

            pathUtil.BuildPhotoDirectoryPath().Returns(info => @"App_Data/photos");

            pathUtil.BuildAlbumPath(Arg.Any<int>(), Arg.Any<int>()).Returns(info =>
            {
                var userId = (int)info[0];
                var albumId = (int)info[1];

                return pathUtil.BuildPhotoDirectoryPath() + "/" + userId + "/" + albumId;
            });

            pathUtil.BuildOriginalPhotoPath(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>())
                    .Returns(info
                             =>
                        {
                            var userId = (int) info[0];
                            var albuId = (int) info[1];
                            var photoId = (int) info[2];
                            var photoFormat = (string) info[3];

                            string albumPath = pathUtil.BuildAlbumPath(userId, albuId);

                            return albumPath + "/" + photoId + photoFormat;
                        });

            storage.PathUtil = pathUtil;
            FillRepository();
        }

        [Test]
        public void ShouldReturnCorrectAlbumPath()
        {
            // setup
            var album = new AlbumModel
                {
                    Id = 1,
                    UserModelId = 1
                };

            // body
            string albumPath = storage.GetAlbumPath(album);

            // tear down
            albumPath.Should().Be("App_Data/photos/1/1");
        }

        [Test]
        public void ShouldReturnPathToOriginalPhoto()
        {
            // setup
            var photo = new PhotoModel
            {
                Id = 1,
                AlbumModelId = 1,
                Format = ".png"
            };

            // body
            string originalPhotoPath = storage.GetOriginalPhotoPath(photo);

            // tear down
            originalPhotoPath.Should().Be("App_Data/photos/1/1/1.png");
        }

        private void FillRepository()
        {
            using (IUnitOfWork unitOfWork = workFactory.GetUnitOfWork())
            {
                var user = new UserModel
                    {
                        Id = 1
                    };

                var album = new AlbumModel
                    {
                        Id = 1,
                        UserModelId = 1
                    };

                unitOfWork.Users.Add(user);
                unitOfWork.Albums.Add(album);
            }
        }
    }
}
