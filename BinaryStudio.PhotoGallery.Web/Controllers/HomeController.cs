using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
//    [Authorize] // Only authorized users can access this controller
	[RoutePrefix("Home")]
    public class HomeController : Controller
    {
        /// <summary>
        /// Main user page ("Profile" in main menu)
        /// </summary>
        /// <returns>Return page with flow of pictures</returns>
		[GET]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Albums()
        {
            return View();
        }

        [GET]
        public ActionResult Settings()
        {
            return View();
        }
    }
}
