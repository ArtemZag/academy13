using System.Linq;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Core;
using BinaryStudio.PhotoGallery.Core.SocialNetworkUtils.Facebook;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("Photo")]
    public class PhotoController : Controller
    {
        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpPost]
        public ActionResult GetPhoto(int photoID)
        {
            var photoModel = _photoService.GetPhoto(User.Identity.Name, photoID);

            return Json(PhotoViewModel.FromModel(photoModel));
        }

        [HttpPost]
        public ActionResult GetPhotos(string albumName, int begin, int last)
        {
            var photoModels = _photoService.GetPhotos(User.Identity.Name, albumName, begin, last);

            return Json(photoModels.Select(PhotoViewModel.FromModel).ToList());
        }

        [POST]
        public ActionResult GetPhotosIDFromAlbum(int albumID, int begin, int end)
        {
            var photoModels = _photoService.GetPhotos(User.Identity.Name, albumID, begin, end);

            return Json(photoModels.Select(PhotoViewModel.FromModel).ToList());
        }

        [POST]
        public ActionResult GetLikes(int photoID)
        {
            var likes = _photoService.GetLikes(User.Identity.Name, photoID);

            return Json(likes.Select(UserViewModel.FromModel).ToList());
        }

        [POST]
        public ActionResult AddLike(int photoID)
        {
            _photoService.AddLike(User.Identity.Name, photoID);

            var likes = _photoService.GetLikes(User.Identity.Name, photoID);

            return Json(likes.Select(UserViewModel.FromModel).ToList());
        }
        
        [HttpPost]
        public ActionResult FbSync(string photoID)
        {
           /* var photoModel = _photoService.GetPhoto(Int32.Parse(photoID));
            var photoPath = new List<string>();
*/
            return Redirect(FB.CreateAuthURL(Randomizer.GetString(16)));
            //FB.AddPhotosToAlbum(photoPath,"MakTest",);
        }
        [GET("{photoID}")]
        public ActionResult Index(string photoID)
        {
            ViewBag.PhotoID = photoID;
            return View();
        }
    }
}
