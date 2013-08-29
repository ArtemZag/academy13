using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("user")]
    public class UserController : BaseController
    {
        [GET("{userId}")]
        public ActionResult Index(int userId)
        {
            ViewBag.UserId = userId;
            return View();
        }
    }
}