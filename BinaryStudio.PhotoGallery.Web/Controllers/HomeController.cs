using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Http;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("")]
    public class HomeController : Controller
    {
        [GET("")]
        public ActionResult Index()
        {
            var infoViewModel = new InfoViewModel
            {
                UserEmail = User.Identity.Name,
            };

            return View(infoViewModel);
        }
    }
}
