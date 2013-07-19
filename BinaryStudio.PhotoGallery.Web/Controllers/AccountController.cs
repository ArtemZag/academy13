using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{

    public class AccountController : Controller
    {

        [HttpPost]
        public ActionResult SignIn(AuthInfoModel account)
        {

            if (IsValid(account.AuthName, account.UserPassword))
            {
                FormsAuthentication.SetAuthCookie(account.UserEmail, false);
                return RedirectToAction("Index", "Index");
            }
            else
            {
                ModelState.AddModelError("", "Login details are wrong.");
            }
            return View(account);

        }

        public ActionResult SingOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Index");
        }

        [GET("")]
        public ActionResult Signup()
        {
            return View();
        }

        private bool IsValid(string name, string password)
        {
            bool IsValid = false;
            // todo: How can i access to users repository?
            /*
            using (var db = new DatabaseContext())
            {
                var user = db.Users.FirstOrDefault(u => u.AuthName == name);
                if (user != null)
                {
                    if (user.UserPassword == password)
                    {
                        IsValid = true;
                    }
                }
            }
            */
            return IsValid;
        }
    }
}
