using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Core.SocialNetworkUtils;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Core.Helpers;
using BinaryStudio.PhotoGallery.Core.SocialNetworkUtils.Facebook;

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
            }
            if (service == "facebook")
            {
                return Redirect(FB.CreateAuthURL(Randomizer.GetString(16)));
            }


            return View(new AuthorizationViewModel());
        }

        [GET("Signup/{service}")]
        public ActionResult SignUp(string service)
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
            }
            if (service == "facebook")
            {
                return Redirect(FB.CreateAuthURL(Randomizer.GetString(16)));
            }
            return View(new RegistrationViewModel());
        }

        [GET("FacebookCallBack/{userSecret}")]
        public RedirectToRouteResult FacebookCallBack(string userSecret, string code)
        {
            var token = FB.GetAccessToken(userSecret, code);

            FB.GetAccountInfo("", token); //magic


            if (!_userService.IsUserExist(FB.Email))
            {
                var newUser = new UserModel
                    {
                        FirstName = FB.FirstName,
                        LastName = FB.LastName,
                        Email = FB.Email,
                        AuthInfos = new Collection<AuthInfoModel>
                            {
                                new AuthInfoModel()
                                    {
                                        AuthProvider = AuthInfoModel.ProviderType.Facebook.ToString(),
                                        AuthProviderToken = token
                                    }
                            }
                    };
                _userService.CreateUser(newUser, AuthInfoModel.ProviderType.Facebook);

            }

            Session["AccessToken"] = token;
            FormsAuthentication.SetAuthCookie(FB.Email, false);

            //var fb = new FB();
           // fb.CreateAlbum("test", "my test album", token);
            var photoCollection = new string[]
                {
                    "e:\\rabbit.jpg",
                    "e:\\rabbit1.jpg",
                    "e:\\rabbit2.jpg",
                    "e:\\rabbit3.jpg",
                    "e:\\rabbit4.jpg",
                    "e:\\rabbit5.jpg",
                    "e:\\rabbit6.jpg",
                    "e:\\rabbit7.jpg"
                };
            FB.AddPhotosToAlbum(photoCollection, "Bingally", token);

            return RedirectToAction("Index", "Home");
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