using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("")]
    public class HomeController : BaseController
    {
        [GET("", RouteName = "PublicFlow")]
        public ActionResult Index()
        {
            var t = User.Id;
            return View(new PublicPhotosViewModel {UserId = User.Id});
            
        }
    }
}
