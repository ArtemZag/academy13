using System.Web.Mvc;
using AttributeRouting;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("Menu")]
    public class MenuController : Controller
    {
        private readonly IUserService _userService;

        public MenuController(IUserService userService)
        {
            _userService = userService;
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            var menu = new MenuViewModel
            {
                UserEmail = User.Identity.Name,
                IsAdmin = _userService.IsUserAdmin(User.Identity.Name)
            };

            return View("_Menu", menu);
        }
    }
}