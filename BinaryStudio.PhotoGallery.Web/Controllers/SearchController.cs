using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [RoutePrefix("search")]
    public class SearchController : Controller
    {
        [GET("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}