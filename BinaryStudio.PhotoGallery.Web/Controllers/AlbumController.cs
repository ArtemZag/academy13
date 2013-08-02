using System.Web.Mvc;
using AttributeRouting;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
	[RoutePrefix("Albums")]
    public class AlbumController : Controller
    {
		[HttpGet]
        public ActionResult Index()
		{
		    string name = User.Identity.Name;
            
            return View();
        }

    }
}
