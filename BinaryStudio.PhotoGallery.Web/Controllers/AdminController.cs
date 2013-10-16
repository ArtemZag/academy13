using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Web.Filters;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [AdminAuthorize]
    [RoutePrefix("admin")]
    public class AdminController : BaseController
    {
        [GET("")]
        public ActionResult Index()
        {
            return View();
        }

        [GET("invite")]
        public ActionResult Invite()
        {
            return View();
        }
    }
}