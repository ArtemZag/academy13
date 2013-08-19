using System.Linq;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.Utils;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize] // Only authorized users can access this controller
    [RoutePrefix("Home")]
    public class HomeController : Controller
    {
        /// <summary>
        /// Main page (click on "bingally")
        /// </summary>
        /// <returns>page with flow of public pictures</returns>
        [GET]
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
