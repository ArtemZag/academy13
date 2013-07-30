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
        }

        [Test]
        public void ShouldReturnCorrectAlbumPath()
        {
            // setup
            FillRepository();

            var album = new AlbumModel
                {
                    Id = 1,
                    UserModelId = 1
                };

            pathUtil.BuildAlbumPath(Arg.Any<int>(), Arg.Any<int>()).Returns(info =>
                {
                    var userId = (int) info[0];
                    var albumId = (int) info[1];

                    return pathUtil.BuildPhotoDirectoryPath() + "/" + userId + "/" + albumId;
                });

            storage.PathUtil = pathUtil;

            // body
            string albumPath = storage.GetAlbumPath(album);

            // tear down
            albumPath.Should().Be("App_Data/photos/1/1");
        }

        private void FillRepository()
        {
            using (IUnitOfWork unitOfWork = workFactory.GetUnitOfWork())
            {
                var user = new UserModel
                    {
                        Id = 1
                    };

                unitOfWork.Users.Add(user);
            }
        }
    }
}
