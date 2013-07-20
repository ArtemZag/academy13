﻿using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using AttributeRouting;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    using BinaryStudio.PhotoGallery.Web.Utils;

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
            var user = ModelConverter.GetModel(authInfo);

            var userExist = userService.CheckUser(user.Email);

            if (userExist)
            {
                FormsAuthentication.SetAuthCookie(authInfo.Email, false);
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
            var user = ModelConverter.GetModel(registrationViewModel);

            var userExist = userService.CheckUser(user);

            if (userExist)
            {
                return View(registrationViewModel);
            }

            userService.CreateUser(user);

            return RedirectToAction("Index", "Home");
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
