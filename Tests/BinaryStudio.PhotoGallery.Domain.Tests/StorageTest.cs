using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Tests.Mocked;
using BinaryStudio.PhotoGallery.Domain.Utils;
using BinaryStudio.PhotoGallery.Models;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace BinaryStudio.PhotoGallery.Domain.Tests
{
    [TestFixture]
    internal class StorageTest
    {
        [SetUp]
        public void Setup()
        {
            IPathUtil pathUtil = MockPathUtil();

            workFactory = new TestUnitOfWorkFactory();
            storage = new Storage(workFactory, pathUtil);

            FillRepository();
        }

        private IStorage storage;
        private IUnitOfWorkFactory workFactory;

        private IPathUtil MockPathUtil()
        {
            var result = Substitute.For<IPathUtil>();

            result.BuildPhotoDirectoryPath().Returns(info => @"App_Data\photos");

            result.GetAlbumPath(Arg.Any<int>(), Arg.Any<int>()).Returns(info =>
                {
                    var userId = (int) info[0];
                    var albumId = (int) info[1];

                    return result.BuildPhotoDirectoryPath() + @"\" + userId + @"\" + albumId;
                });

            result.BuildOriginalPhotoPath(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>())
                  .Returns(info
                           =>
                      {
                          var userId = (int) info[0];
                          var albuId = (int) info[1];
                          var photoId = (int) info[2];
                          var photoFormat = (string) info[3];

                          string albumPath = result.GetAlbumPath(userId, albuId);

                          return albumPath + @"\" + photoId + photoFormat;
                      });

            return result;
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

        [Test]
        public void ShouldReturnCorrectAlbumPath()
        {
            // setup
            var album = new AlbumModel
                {
                    Id = 1,
                    UserModelId = 1
                };

            var photo = new PhotoModel
                {
                    AlbumModelId = 1
                };

            // body
            string albumPathByAlbum = storage.GetAlbumPath(album);
            string albumPathByPhoto = storage.GetAlbumPath(photo);

            // tear down
            const string CORRECT = @"App_Data\photos\1\1";
            albumPathByAlbum.Should().Be(CORRECT);
            albumPathByPhoto.Should().Be(CORRECT);
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
            originalPhotoPath.Should().Be(@"App_Data\photos\1\1\1.png");
        }
    }
}