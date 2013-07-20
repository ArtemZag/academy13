using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Utils;
using BinaryStudio.PhotoGallery.Web.ViewModels;

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
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new AuthInfoViewModel());
        }

        [POST]
        public ActionResult SignIn(AuthInfoViewModel authInfo)
        {
            UserModel user = ModelConverter.GetModel(authInfo);

            bool userExist = userService.CheckUser(user.Email);

            if (userExist)
            {
                FormsAuthentication.SetAuthCookie(authInfo.Email, authInfo.RememberMe);
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
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new RegistrationViewModel());
        }

        [POST]
        public ActionResult Signup(RegistrationViewModel registrationViewModel)
        {
            UserModel user = ModelConverter.GetModel(registrationViewModel);

            bool userExist = userService.CheckUser(user.Email);

            if (userExist)
            {
                return View(registrationViewModel);
            }

            var userWasCreated = userService.CreateUser(user);

            if (userWasCreated)
            {
                FormsAuthentication.SetAuthCookie(user.Email, registrationViewModel.RememberMe);
                RedirectToAction("Index", "Home");
            }

            return View(registrationViewModel);;
        }

        [GET]
        public ActionResult RemindPass()
        {
            return View(new RemindPassViewModel());
        }

        [POST]
        public ActionResult RemindPass(RemindPassViewModel remindPassViewModel)
        {
            return View(remindPassViewModel);
        }
    }
}