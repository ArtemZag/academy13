using System.Linq;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.Utils;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    using BinaryStudio.PhotoGallery.Web.ViewModels;

    [Authorize] // Only authorized users can access this controller
    [RoutePrefix("Home")]
    public class HomeController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly IModelConverter _modelConverter;

        public HomeController(IPhotoService photoService, IModelConverter modelConverter)
        {
            _photoService = photoService;
            _modelConverter = modelConverter;
        }

        /// <summary>
        /// Main page (click on "bingally")
        /// </summary>
        /// <returns>page with flow of public pictures</returns>
        [GET]
        public ActionResult Index()
        {
            var photoModels = _photoService.GetPhotos(User.Identity.Name, 0, 30);

            var infoViewModel = new InfoViewModel
            {
                UserEmail = User.Identity.Name,
                Photos = photoModels.Select(_modelConverter.GetViewModel).ToList()
            };

            return View(infoViewModel);
        }

        [HttpPost]
        public ActionResult GetPhotosViaAjax(int startIndex, int endIndex)
        {
            var photos = _photoService.GetPhotos(User.Identity.Name, startIndex, endIndex)
                .Select(_modelConverter.GetViewModel);
            return Json(photos);
        }
    }
}
