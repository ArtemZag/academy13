using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("albums")]
    public class AlbumsController : BaseController
    {
        [GET("")]
        public ActionResult Index()
        {
            return View(new AlbumViewModel());
        }

        [GET("{skip:int}/{take:int}")]
        public ActionResult GetAlbums(int skip, int take)
        {
            var albums = _albumService.GetAlbumsRange(User.Id, skip, take)
                                      .Select(album => AlbumViewModel.FromModel(album, _resizePhoto)).ToList();

            return Json(albums, JsonRequestBehavior.AllowGet);
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

            var model = new UserInfoViewModel(_albumService.AlbumsCount(User.Id).ToString(),
                                              _photoService.PhotoCount(User.Id).ToString(),
                                              fullname,
                                              lastAdded, user.IsAdmin ? "admin" : "simple user",
                                              user.Department,
                                              _resizePhoto.GetUserAvatar(user.Id, ImageSize.Medium));
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}