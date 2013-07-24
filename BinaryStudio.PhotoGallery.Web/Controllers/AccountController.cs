using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [RoutePrefix("Account")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            this._userService = userService;
        }

        [GET]
        public ActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                // recheck user (maybe it was deleted, while cookie is truth)
                var userExist = this._userService.IsUserExist(User.Identity.Name);

                if (userExist)
                {
                    return RedirectToAction("Index", "Home");
                }

                // Clear cookie
                FormsAuthentication.SignOut();
            }

            return View();
        }

        [GET]
        public ActionResult SignUp()
        {
            if (User.Identity.IsAuthenticated)
            {
                // recheck user (maybe it was deleted, while cookie is truth)
                var userExist = this._userService.IsUserExist(User.Identity.Name);

                if (userExist)
                {
                    return RedirectToAction("Index", "Home");
                }

                // Clear cookie
                FormsAuthentication.SignOut();
            }

            return View();
        }

        [GET]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Signin", "Account");
        }

        [GET]
        public ActionResult RemindPass()
        {
            return View(new RemindPassViewModel());
        }

        [POST]
        public ActionResult RemindPass(RemindPassViewModel remindPassViewModel)
        {
            return View();
        }
    }
}