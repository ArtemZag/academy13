﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Services;
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
        [SetUp]
        public void Setup()
        {
            IUnityContainer container = Bootstrapper.Initialise();

            photoService = container.Resolve<IPhotoService>();
            userService = container.Resolve<IUserService>();
            albumService = container.Resolve<IAlbumService>();
            workFactory = container.Resolve<IUnitOfWorkFactory>();
        }

        private IPhotoService photoService;
        private IUserService userService;
        private IAlbumService albumService;

        private IUnitOfWorkFactory workFactory;

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
                    UserModelId = 1,
                    AlbumName = "albumName",
                    Photos = new Collection<PhotoModel>()
                };

            userService.CreateUser(user);
            albumService.CreateAlbum(user.Email, album);

            var photo = new PhotoModel
                {
                    Id = 1
                };

            // body
            int photosCountBeforeAdd = album.Photos.Count;

            photoService.AddPhoto("some@gmail.com", "albumName", photo);

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

            using (IUnitOfWork unitOfWork = workFactory.GetUnitOfWork())
            {
                unitOfWork.Photos.Add(photo);

                // body
                deletedPhotosAfterCreation = unitOfWork.Photos.Filter(model => model.IsDeleted).Count();
                photoService.DeletePhoto(photo);

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
                UserModelId = 2,
                AlbumName = "albumName",
                Photos = new Collection<PhotoModel>()
            };

            userService.CreateUser(user);
            albumService.CreateAlbum(user.Email, album);

            photoService.AddPhotos("some1@gmail.com", "albumName", photosToFill);

            // body
            IEnumerable<PhotoModel> photos = photoService.GetPhotos("some1@gmail.com", "albumName", 0, 5);
            int count = photos.Count();

            // tear down
            count.Should().Be(5);
        }
    }
}