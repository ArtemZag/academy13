using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Utils;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Core.PhotoUtils;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
	[RoutePrefix("Albums")]
    public class AlbumsController : Controller
    {
	    private readonly IAlbumService albumService;
	    private readonly IUserService userService;
        private readonly IPhotoService photoService;
	    private readonly IPhotoSynchronizingService photoSynchronizing;
        private IModelConverter modelConverter;
        public AlbumsController(IAlbumService _albumService, IUserService _userService, IPhotoSynchronizingService _photoSynchronizing, IModelConverter _modelConverter,IPhotoService _photoService)
        {
            albumService = _albumService;
            userService = _userService;
            photoSynchronizing = _photoSynchronizing;
            modelConverter = _modelConverter;
            photoService = _photoService;
        }
	    [GET("")]
	    public ActionResult Index()
	    {
	        return View(new AlbumsViewModel());
	    }

        [HttpPost]
        public ActionResult GetAlbums(int start, int end)
        {
            string email = User.Identity.Name;
            //var albumModels = albumService.GetAlbums(email, start, end);
            //UserModel user = userService.GetUser(email);

            var model = new AlbumsViewModel
                {
                    UserEmail = User.Identity.Name,
                    lastPhotos = photoService.GetPhotos(email, 0, 10).Select(modelConverter.GetViewModel).ToList(),
                    totalPhotos = 0,

                };

            //model.totalPhotos = model.Models.Sum(VARIABLE => photoService.PhotoCount(email, VARIABLE.AlbumName));

            model.Models = new List<AlbumViewModel>();

            var descriptions = new string[]
                {"Pictures", "Cars", "Muscle cars", "Power cars", "Elite cars", "Mega cars", "Import cars"};

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
                                            ID = 0,
                                            TagName = "fun images",
                                            AlbumModels = null
                                        }
                                },
                            DateOfCreation = DateTime.Now,
                            Description = descriptions[i],
                            Id = i,
                            UserModelId = 0,
                        });
                photoSynchronizing.Initialize(i, 0, 64);
                photoSynchronizing.SyncOriginalAndThumbnailImages();
                model.Models[i].collageSource = photoSynchronizing.CreateCollageIfNotExist(256, 3);
            }

            model.Models = model.Models.Select(p => p).Skip(start).Take(end - start + 1).ToList();
            return Json(model);
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
                    lastPhotoAdded = "Date: " + DateTime.Now.ToShortDateString() +" time: "+ DateTime.Now.ToLongTimeString(),
                    isAdmin = user.IsAdmin ? "admin" : "simple user",
                    department = ".Net development",
                    userAvatar = "/data/photos/0/avatar.jpg"
                });
        }
    }
}
