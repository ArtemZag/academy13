using System.Linq;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.Utils;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("Album")]
    public class PhotoController : Controller
    {
        private readonly IPhotoService _photoService;
        private IUserService _userService;
        private readonly IModelConverter _modelConverter;

        public PhotoController(IUserService userService, IPhotoService photoService, IModelConverter modelConverter)
        {
            _userService = userService;
            _photoService = photoService;
            _modelConverter = modelConverter;
        }


        [HttpPost]
        public ActionResult GetPhotos(string albumName, int begin, int last)
        {
            var photoModels = _photoService.GetPhotos(User.Identity.Name, albumName, begin, last);

            return Json(photoModels.Select(model => _modelConverter.GetViewModel(model)).ToList());
        }

        [GET("photo{photoID}")]
        public ActionResult Index(string photoId)
        {
            return View();
        }
    }
}
