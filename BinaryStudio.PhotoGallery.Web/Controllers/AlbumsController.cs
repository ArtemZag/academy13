using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("users")]
    public class AlbumsController : BaseController
    {
        [GET("{userId}")]
        public ActionResult Index(int userId)
        {
            ViewBag.UserId = userId;
            return View("Index");
        }
    }
}