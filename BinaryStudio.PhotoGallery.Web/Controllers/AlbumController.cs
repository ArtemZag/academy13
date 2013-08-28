using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("album")]
    public class AlbumController : BaseController
	{
		[GET("{albumId}")]
        public ActionResult Index(int albumId)
		{
		    ViewBag.AlbumId = albumId;

            return View("Index");
        }
    }
}
