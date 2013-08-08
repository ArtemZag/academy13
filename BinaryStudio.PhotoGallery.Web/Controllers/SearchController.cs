using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    public class SearchController : Controller
    {
        [GET("Search")]
        public ActionResult Search()
        {
            return View();
        }
    }
}