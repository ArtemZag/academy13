using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
	[RoutePrefix("Profile")]
    public class ProfileController : Controller
    {
		[GET("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
