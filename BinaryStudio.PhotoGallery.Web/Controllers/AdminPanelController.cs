using System.Linq;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Database;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
	[RoutePrefix("AdminPanel")]
    public class AdminPanelController : Controller
    {
		/// <summary>
		/// Administration page
		/// </summary>
		[GET("")]
        public ActionResult Index()
        {
			UnitOfWork unitOfWork = new UnitOfWork();
			var users = unitOfWork.Users.All().ToList();
            return View(users);
        }

    }
}
