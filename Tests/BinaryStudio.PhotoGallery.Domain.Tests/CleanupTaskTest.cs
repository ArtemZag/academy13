using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Core.Helpers;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using FizzWare.NBuilder;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace BinaryStudio.PhotoGallery.Domain.Tests
{
    [TestFixture]
    public class CleanupTaskTest
    {
        private ICleanupTask cleanupTask;
        private IUnitOfWorkFactory workFactory;
        private IPathHelper pathHelper;

        [SetUp]
        public void Setup()
        {
            IUnityContainer container = Bootstrapper.Initialise();

            cleanupTask = container.Resolve<ICleanupTask>();
            workFactory = container.Resolve<IUnitOfWorkFactory>();

            
        }

        private void FillRepository()
        {
            var user = new UserModel
                {
                    Id = 1
                };

            IList<AlbumModel> albums =
                Builder<AlbumModel>.CreateListOfSize(3).Build();

            IList<PhotoModel> photos =
                Builder<PhotoModel>.CreateListOfSize(10)
                                   .All().With(model => model.IsDeleted = false)
                                   .Random(4)
                                   .With(model => model.AlbumModelId = 1)
                                   .And(model => model.IsDeleted = true)
                                   .And(model => model.Format = ".png")
                                   .Build();

            using (IUnitOfWork unitOfWork = workFactory.GetUnitOfWork())
            {
                unitOfWork.Users.Add(user);

                foreach (AlbumModel album in albums)
                {
                    unitOfWork.Albums.Add(album);
                }

                foreach (PhotoModel photo in photos)
                {
                    unitOfWork.Photos.Add(photo);
                }
            }
        }

        [Test]
        public void PhotosShouldBeDeleted()
        {
            FillRepository();

            cleanupTask.Execute();
        }
    }
}