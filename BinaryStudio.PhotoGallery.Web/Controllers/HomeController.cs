using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;
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
        private readonly IUserService _userService;

        public HomeController(IUserService userService, IPhotoService photoService)
        {
            _photoService = photoService;
            _userService = userService;
        }
        /// <summary>
        /// Main user page (click on "bingally")
        /// </summary>
        /// <returns>page with flow of public pictures</returns>
		[GET("Index")]
        public ActionResult Index()
        {
            var viewmodels = _photoService.GetPhotos(User.Identity.Name, 0, 20);
            return View(new InfoViewModel { UserEmail = User.Identity.Name, 
                                            Photos = viewmodels.Select(ModelConverter.GetViewModel).ToList()});
        }

        /// <summary>
        /// Gallery page
        /// </summary>
        /// <returns>page with all users photos, sorted by date</returns>
        [GET("Gallery")]
        public ActionResult Gallery()
        {
            return View();
            // for example&test get 20 photos
            var viewmodels = _photoService.GetPhotos(User.Identity.Name, 0, 20);
            List<PhotoViewModel> photos = viewmodels.Select(ModelConverter.GetViewModel).ToList();
            return View(photos);
        }

        /// <summary>
        /// Album page
        /// </summary>
        /// <returns>page with all users albums</returns>
        [GET("Albums")]
        public ActionResult Albums()
        {
            return View();
        }

        /// <summary>
        /// Gruops page
        /// </summary>
        /// <returns>page with all users groups</returns>
        [GET("Groups")]
        public ActionResult Groups()
        {
            return View();
        }
    }
}
