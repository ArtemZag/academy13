using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("search")]
    public class SearchController : BaseController
    {
        [GET("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}