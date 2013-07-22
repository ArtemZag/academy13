using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
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

        [GET("Signin/{service}")]
        public ActionResult SignIn(string service)
        {
            if (string.IsNullOrEmpty(service))
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                // TODO Auth with social (don't change this block!!! It will be changed)
                return RedirectToAction("Index", "Home");
            }

            return View(new AuthInfoViewModel());
        }

        [POST("Signin")]
        public ActionResult SignIn(AuthInfoViewModel authInfo)
        {
            if (ModelState.IsValid)
            {
                var user = ModelConverter.GetModel(authInfo);

                var userExist = userService.CheckUser(user.Email);

                if (userExist)
                {
                    FormsAuthentication.SetAuthCookie(authInfo.Email, authInfo.RememberMe);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "E-mail or password is incorrect");
            }

            return View(authInfo);
        }

        [GET("Signup/{service}")]
        public ActionResult SignUp(string service)
        {
            if (string.IsNullOrEmpty(service))
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                // TODO Auth with social (don't change this block!!! It will be changed)
                return RedirectToAction("Index", "Home");;
            }

            return View(new RegistrationViewModel());
        }

        [POST("Signup")]
        public ActionResult SignUp(RegistrationViewModel registrationViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = ModelConverter.GetModel(registrationViewModel);
                
                var userExist = userService.CheckUser(user.Email);

                if (userExist)
                {
                    ModelState.AddModelError("", "This E-mail address is already in use");
                    return View(registrationViewModel);
                }

                try
                {
                    userService.CreateUser(user);

                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    ModelState.AddModelError("", "Can't create new user. Something happens with server");
                }
            }

            return View(registrationViewModel);
        }

        [GET("Signout")]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Signin", "Account");
        }

        [GET("Remindpass")]
        public ActionResult RemindPass()
        {
            return View(new RemindPassViewModel());
        }

        [POST("Remindpass")]
        public ActionResult RemindPass(RemindPassViewModel remindPassViewModel)
        {
            return View(remindPassViewModel);
        }
    }
}