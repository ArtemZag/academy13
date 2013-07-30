using System.Web.Mvc;
using AttributeRouting;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
	[RoutePrefix("Albums")]
    public class AlbumController : Controller
    {
		[HttpGet]
        public ActionResult Albums()
		{
		    string name = User.Identity.Name;
            
            return View();
        }

    }
}
