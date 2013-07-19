using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
	[RoutePrefix("Home")]
    public class HomeController : Controller
    {
        /// <summary>
        /// Is it user page (flow of his pictures)
        /// </summary>
        /// <returns></returns>
		[GET("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
