using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
	[RoutePrefix("Upload")]
    public class FileController : Controller
    {
		[GET]
        public ActionResult Upload()
        {
            return View();
        }
    }
}
