﻿using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Core.PhotoUtils;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("Albums")]
    public class AlbumsController : Controller
    {
        private static string usersFolder;
        private static string dataFolderName;
        private static string photosFolderName;
        private IAlbumService albumService;
        private IUserService userService;
        static AlbumsController()
        {
            dataFolderName = ConfigurationManager.AppSettings["dataFolderName"];
            photosFolderName = ConfigurationManager.AppSettings["photosFolderName"];
            usersFolder = GetUsersImagesFolder();
        }
        public AlbumsController(IAlbumService _albumService, IUserService _userService)
        {
            albumService = _albumService;
            userService = _userService;
        }
        [GET("")]
        public ActionResult Index()
        {
            string email = User.Identity.Name;
            var albums = albumService.GetAllAlbums(email);
            return View(new AlbumsViewModel() { UserEmail = email });
        }

        [HttpPost]
        public ActionResult GetAlbums(int start, int end)
        {
            AlbumsViewModel model = new AlbumsViewModel();
            model.UserEmail = User.Identity.Name;
            model.Models = new Collection<AlbumViewModel>();
            string[] descriptions = new string[] { "Pictures", "Cars", "Muscle cars", "Power cars", "Elite cars", "Mega cars", "Import cars" };

            for (int i = 0; i < 7; i++)
            {
                model.Models.Add(
                    new AlbumViewModel()
                    {
                        AlbumName = "Pictures",
                        AlbumTags = new Collection<AlbumTagModel>()
		                        {
		                            new AlbumTagModel()
		                                {
		                                    Id = 0,
		                                    TagName = "fun images",
		                                    Albums = null
		                                }
		                        },
                        DateOfCreation = DateTime.Now,
                        Description = descriptions[i],
                        Id = i,
                        IsDeleted = false,
                        UserModelId = 0,
                    });
            }

            var models = model.Models.Select(p => p).Skip(start).Take(end - start + 1).ToList();
            foreach (var mod in models)
            {
                AsyncPhotoProcessor processor = new AsyncPhotoProcessor(usersFolder, mod.UserModelId, mod.Id, 64);
                processor.SyncOriginalAndThumbnailImages();
                string s = processor.CreateCollageIfNotExist(256, 3);
                s = s.Remove(0, s.IndexOf("data") - 1).Replace(@"\", "/");
                mod.collageSource = s;
            }
            return Json(models);
        }
        public ActionResult GetUserInfo()
        {
            var user = userService.GetUser(User.Identity.Name);
            return Json(new
            {
                nickName = "Superman",
                albumCount = 7,
                photoCount = 90,
                firstName = User.Identity.Name,
                lastName = User.Identity.Name,
                lastPhotoAdded = "Date: " + DateTime.Now.ToShortDateString() + " time: " + DateTime.Now.ToLongTimeString(),
                isAdmin = user.IsAdmin ? "admin" : "simple user",
                department = ".Net development",
                userAvatar = "/data/photos/0/avatar.jpg"
            });
        }
        private static string GetUsersImagesFolder()
        {
            string webProjectPath = HttpRuntime.AppDomainAppPath;
            return Path.Combine(webProjectPath, dataFolderName, photosFolderName);
        }
    }
}
