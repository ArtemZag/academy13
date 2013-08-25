using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Extensions.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.Photo;
using BinaryStudio.PhotoGallery.Web.ViewModels.User;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("albums")]
    public class AlbumsController : BaseController
    {
        private readonly IAlbumService albumService;
        private readonly IPathUtil pathUtil;
        private readonly IPhotoService photoService;
        private readonly IUserService userService;

        public AlbumsController(IAlbumService albumService, IUserService userService, IPhotoService photoService, IPathUtil pathUtil)
        {
            this.albumService = albumService;
            this.userService = userService;
            this.pathUtil = pathUtil;
            this.photoService = photoService;
        }

        [GET("")]
        public ActionResult Index()
        {
            return View(new AlbumViewModel());
        }

        [GET("{skip:int}/{take:int}")]
        public ActionResult GetAlbums(int skip, int take)
        {
            var albums = albumService.GetAlbumsRange(User.Id, skip, take)
                                      .Select(AlbumViewModel.FromModel).ToList();

            return Json(albums, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFlowPhotos()
        {
            UserModel user = userService.GetUser(User.Id);

            IEnumerable<PhotoViewModel> lastTenPhotos =
                photoService.GetLastPhotos(user.Id, 0, 10).Select(model => model.ToPhotoViewModel());
            return Json(lastTenPhotos);
        }

        [GET("user")]
        public ActionResult GetUserInfo()
        {
            UserModel user = userService.GetUser(User.Id);

            string fullname = string.Format("{0} {1}", user.FirstName, user.LastName);

            // todo! 
            UserViewModel model = null;

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}