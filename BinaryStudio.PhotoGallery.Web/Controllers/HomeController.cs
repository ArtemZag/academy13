using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    using BinaryStudio.PhotoGallery.Web.ViewModels;

    [Authorize] // Only authorized users can access this controller
	[RoutePrefix("Home")]
    public class HomeController : Controller
    {
        /// <summary>
        /// Main user page (click on "bingally")
        /// </summary>
        /// <returns>page with flow of public pictures</returns>
		[GET("Index")]
        public ActionResult Index()
        {
            return View(new InfoViewModel { UserEmail = User.Identity.Name });
        }

        /// <summary>
        /// Gallery page
        /// </summary>
        /// <returns>page with all users photos, sorted by date</returns>
        [GET("Gallery")]
        public ActionResult Gallery()
        {
            return View();
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
