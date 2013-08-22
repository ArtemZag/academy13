using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Core.PhotoUtils;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("albums")]
    public class AlbumsController : BaseController
    {
        private readonly IAlbumService _albumService;
        private readonly IPathUtil _pathUtil;
        private readonly IPhotoService _photoService;
        private readonly IUserService _userService;

        public AlbumsController(
            IAlbumService albumService,
            IUserService userService,
            IPhotoService photoService,
            IPathUtil pathUtil)
        {
            _albumService = albumService;
            _userService = userService;
            _pathUtil = pathUtil;
            _photoService = photoService;
        }

        [GET("")]
        public ActionResult Index()
        {
            return View(new AlbumViewModel());
        }

        [GET("{skip:int}/{take:int}")]
        public ActionResult GetAlbums(int skip, int take)
        {
//            List<AlbumViewModel> albums = _albumService.GetAlbumsRange(User.Id, skip, take)
//                .Select(AlbumViewModel.FromModel)
//                .ToList();
//
//            return Json(albums, JsonRequestBehavior.AllowGet);
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFlowPhotos()
        {
            UserModel user = _userService.GetUser(User.Id);
            IEnumerable<PhotoViewModel> lastTenPhotos =
                _photoService.GetLastPhotos(user.Id, 0, 10).Select(PhotoViewModel.FromModel);
            return Json(lastTenPhotos);
        }

        [GET("user")]
        public ActionResult GetUserInfo()
        {
            UserModel user = _userService.GetUser(User.Id);

            string fullname = string.Format("{0} {1}", user.FirstName, user.LastName);

            DateTime lastDate = _photoService.LastPhotoAdded(User.Id);

            string lastAdded = string.Format("{0}:{1}:{2} {3}.{4}.{5}",
                lastDate.Hour,
                lastDate.Minute,
                lastDate.Second,
                lastDate.Day,
                lastDate.Month,
                lastDate.Year);

            return Json(new UserInfoViewModel(_albumService.AlbumsCount(User.Id).ToString(),
                _photoService.PhotoCount(User.Id).ToString(),
                fullname,
                lastAdded, user.IsAdmin ? "admin" : "simple user",
                user.Department,
                (new AsyncPhotoProcessor(User.Id, 0, 64, _pathUtil)).GetUserAvatar(AvatarSize.Medium)), JsonRequestBehavior.AllowGet);
        }
    }
}