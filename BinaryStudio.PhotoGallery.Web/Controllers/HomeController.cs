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
        private readonly IUserService _userService;

        public HomeController(IUserService userService, IPhotoService photoService)
        {
            _photoService = photoService;
            _userService = userService;
        }
        /// <summary>
        /// Main user page (flow of public pictures) [click on "bingally"]
        /// </summary>
		[GET]
        public ActionResult Index()
        {   
            var viewmodels = _photoService.GetPhotos(User.Identity.Name, 20);
            return View(new InfoViewModel { UserEmail = User.Identity.Name, 
                                            Photos = viewmodels.Select(ModelConverter.GetViewModel).ToList()});
        }

        /// <summary>
        /// Gallery page, where all users photos, sorted by date
        /// </summary>
        [GET]
        public ActionResult Gallery()
        {
            return View();
        }

        /// <summary>
        /// Album page
        /// </summary>
        [GET]
        public ActionResult Albums()
        {
            return View();
        }

        /// <summary>
        /// Gruops page
        /// </summary>
        [GET]
        public ActionResult Groups()
        {
            return View();
        }
    }
}
