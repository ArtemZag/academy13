using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("albums")]
    public class AlbumsController : BaseController
    {
        [GET("")]
        public ActionResult Index()
        {
            return View(new AlbumViewModel());
        }
    }
}