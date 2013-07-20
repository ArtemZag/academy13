using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize] // Only authorized user can access this controller
	[RoutePrefix("Home")]
    public class HomeController : Controller
    {
        /// <summary>
        /// Main user page
        /// </summary>
        /// <returns>Return page with flow of pictures</returns>
		[GET]
        public ActionResult Index()
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
