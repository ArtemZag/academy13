using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.Utils;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web;

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
                    // recheck user (maybe it was deleted, while cookie is truth)
                    var userExist = userService.IsUserExist(User.Identity.Name);

                    if (userExist)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    // Clear cookie
                    FormsAuthentication.SignOut();
                }
            }
            else
            {
                // TODO Auth with social (don't change this block!!! It will be changed)
                return RedirectToAction("Index", "Home");
            }

            return View(new AuthInfoViewModel { RememberMe = true });
        }

        [POST("Signin")]
        public JsonResult SignIn(AuthInfoViewModel authInfo)
        {
            if (ModelState.IsValid)
            {
                var userValid = userService.IsUserValid(authInfo.Email, authInfo.Password);

                if (userValid)
                {
                    FormsAuthentication.SetAuthCookie(authInfo.Email, authInfo.RememberMe);
                    return Json("ok");
                }
            }

            return Json(ModelState.SelectMany(item => item.Value.Errors).Select(error => error.ErrorMessage).ToList());
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
                var userValid = userService.IsUserValid(registrationViewModel.Email, registrationViewModel.Password);

                if (userValid)
                {
                    ModelState.AddModelError("", "This e-mail address is already in use");
                    return View(registrationViewModel);
                }

                try
                {
                    var user = ModelConverter.GetModel(registrationViewModel);

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