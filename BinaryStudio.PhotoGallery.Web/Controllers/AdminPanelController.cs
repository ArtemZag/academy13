using System.Linq;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Services;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
	[RoutePrefix("AdminPanel")]
    public class AdminPanelController : Controller
	{
	    private readonly IUserService userService;

	    public AdminPanelController(IUserService userService)
	    {
	        this.userService = userService;
	    }

	    /// <summary>
		/// Administration page
		/// </summary>
		[GET("")]
        public ActionResult Index()
	    {
	        var users = userService.GetAllUsers();
            return View(users);
        }

    }
}
