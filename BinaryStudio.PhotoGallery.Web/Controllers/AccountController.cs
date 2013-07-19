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
            if (true)
            {
//                FormsAuthentication.SetAuthCookie(authInfo.Email, false);
                return RedirectToAction("Index", "Home");
            }

            return View(authInfo);
        }

        [GET]
        public ActionResult SingOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Signin", "Account");
        }

        [GET]
        public ActionResult Signup()
        {
            return View();
        }

        [POST]
        public ActionResult Signup(RegistrationViewModel registrationViewModel)
        {
            return View();
        }

        [GET]
        public ActionResult RemindPass()
        {
            return View();
        }
    }
}
