using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Core;
using BinaryStudio.PhotoGallery.Core.SocialNetworkUtils.Facebook;
using BinaryStudio.PhotoGallery.Core.UserUtils;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.Account;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [RoutePrefix("")]
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ICryptoProvider _cryptoProvider;

        public AccountController(IUserService userService, ICryptoProvider cryptoProvider)
        {
            _userService = userService;
            _cryptoProvider = cryptoProvider;
        }

        [GET("login/{service?}", RouteName = "Login")]
        public ActionResult SignIn(string service)
        {
            if (string.IsNullOrEmpty(service))
            {
                if (User.Identity.IsAuthenticated)
                {
                    // recheck user (maybe it was deleted, while cookie is truth)
                    var userExist = _userService.IsUserExist(User.Id);

                    if (userExist)
                    {
                        return RedirectToRoute("PublicFlow");
                    }

                    // Clear cookie
                    FormsAuthentication.SignOut();
                }
            }

            if (service == "facebook")
            {
                return Redirect(FB.CreateAuthUrl(Randomizer.GetString(16)));
            }

            return View(new SigninViewModel {RememberMe = true});
        }

        [GET("registration/{invite}", RouteName = "Registration")]
        public ActionResult SignUp(string invite)
        {
            if (string.IsNullOrEmpty(invite))
            {
                if (User.Identity.IsAuthenticated)
                {
                    // recheck user (maybe it was deleted, while cookie is truth)
                    var userExist = _userService.IsUserExist(User.Id);

                    if (userExist)
                    {
                        return RedirectToRoute("PublicFlow");
                    }
                    // Clear cookie
                    FormsAuthentication.SignOut();
                }
            }

            var signupViewModel = new SignupViewModel();

            try
            {
                var user = _userService.GetUnactivatedUser(invite);
                signupViewModel.Email = user.Email;
            }
            catch (UserNotFoundException)
            {
                return RedirectToRoute("Login");
            }
            
            return View(signupViewModel);
        }

        [GET("facebook?{userSecret}&{code}")]
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
                                        AuthProviderId = token
                                    }
                            }
                    };
                // TODO
//                _userService.CreateUser(newUser, AuthInfoModel.ProviderType.Facebook);

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
           var albumList =  FB.GetListOfAlbums(token);

            return RedirectToRoute("PublicFlow");
        }

        [GET("signout")]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToRoute("Login");
        }

        [GET("remind/{userId}/{hash}")]
        public ActionResult ChangePass(int userId, string hash)
        {
            try
            {
                var mUser = _userService.GetUser(userId);
                if (mUser.RemindPasswordSalt.Equals(hash))
                {
                    return View(new RemindPassViewModel () { Email = mUser.Email });
                }
                return RedirectToRoute("Login");
            }
            catch (UserNotFoundException)
            {
                return RedirectToRoute("Login");
            }
        } 
    }
}