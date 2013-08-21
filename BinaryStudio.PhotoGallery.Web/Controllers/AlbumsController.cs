using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Core.PathUtils;
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
        private IAlbumService albumService;
        private IUserService userService;
        private IPhotoService photoService;
        private IPathUtil pathUtil;
        public AlbumsController(IAlbumService _albumService, IUserService _userService,IPhotoService _photoService, IPathUtil _pathUtil)
        {
            albumService = _albumService;
            userService = _userService;
            pathUtil = _pathUtil;
            photoService = _photoService;
        }
        [GET("")]
        public ActionResult Index()
        {
            return View(new AlbumViewModel());
        }

        [HttpPost]
        public ActionResult GetAlbums(int start, int end)
        {
            string email = User.Identity.Name;
            var user = userService.GetUser(email);
            var albums =
                albumService.GetAlbumsRange(user.Id, start, end - start).Select(AlbumViewModel.FromModel).ToList();

            return Json(albums);
        }
        public ActionResult GetFlowPhotos()
        {
            string email = User.Identity.Name;
            var user = userService.GetUser(email);
            var lastTenPhotos = photoService.GetLastPhotos(user.Id, 0, 10).Select(PhotoViewModel.FromModel);
            return Json(lastTenPhotos);
        }
        public ActionResult GetUserInfo()
        {
            string email = User.Identity.Name;
            var user = userService.GetUser(email);
            var userId = user.Id;

            var fullname = string.Format("{0} {1}", user.FirstName, user.LastName);
            var lastDate = photoService.LastPhotoAdded(userId);
            var lastAdded = string.Format("{0}:{1}:{2} {3}.{4}.{5}", 
                                           lastDate.Hour, 
                                           lastDate.Minute, 
                                           lastDate.Second, 
                                           lastDate.Day,
                                           lastDate.Month, 
                                           lastDate.Year);


            return Json(new UserInfoViewModel(albumService.AlbumsCount(user.Id).ToString(),
                                              photoService.PhotoCount(user.Id).ToString(),
                                              fullname,
                                              lastAdded, user.IsAdmin ? "admin" : "simple user",
                                              user.Department,
                                              (new AsyncPhotoProcessor(userId, 0, 64, pathUtil)).GetUserAvatar()));
        }
    }
}
