using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Core.SocialNetworkUtils.Facebook;
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

        [GET("Signin/{service}")]
        public ActionResult SignIn(string service)
        {
            if (string.IsNullOrEmpty(service))
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

            return View(new AuthorizationViewModel());
                }


                return RedirectToAction("Index", "Home");
            }

            return View(new AuthorizationViewModel {RememberMe = true});
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

            return View(new RegistrationViewModel());
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

        //[GET("FacebookCallBack/{code}")]
        public void FacebookCallBack(string code)
        {
            var facebook = new Facebook();
            facebook.GetAccessToken(code);

        }
    }
}