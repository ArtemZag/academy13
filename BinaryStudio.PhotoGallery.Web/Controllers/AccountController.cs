using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using AttributeRouting;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [RoutePrefix("Account")]
    public class AccountController : Controller
    {
        private readonly IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        [GET]
        public ActionResult SignIn()
        {
            return View();
        }

        [POST]
        public ActionResult SignIn(AuthInfoViewModel authInfo)
        {
//            userService.CheckUser() TODO how can I known user name or last name to check user email ? WTF?
            if (true)
            {
//                FormsAuthentication.SetAuthCookie(authInfo.Email, false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Login details are wrong.");
            }

            return View(authInfo);
        }

        [GET]
        public ActionResult SingOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Signin", "Account");
        }

        [POST]
        public ActionResult Signup(RegistrationViewModel registrationViewModel)
        {
            return View();
        }
    }
}
