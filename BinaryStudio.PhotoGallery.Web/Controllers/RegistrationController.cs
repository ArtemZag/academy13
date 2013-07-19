using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    public class RegistrationController : Controller
    {
        /*//
        // GET: /Registration/

        [HttpGet]
        public ViewResult RegistrationForm()
        {
            /*Это выполняется если мы перешли на страницу регистрации#1#
            ViewBag.Colors = new string[] { "Black", "Black", "Black", "Black", "Black", "Black", "Black" };
            return View();
        }

        [HttpPost]
        public ViewResult RegistrationForm(RegistrationViewModel registration)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Colors = registration.ColorsForFirlds;
                /* запись в базу данных .#1#
                // todo: How can i access to users repository?

                //using (var db = new DatabaseContext())
                //{
                //    var newAccount = db.Users.Create();
                //    newAccount.UserEmail = registration.Email;
                //    newAccount.UserPassword = registration.Password;
                //    newAccount.AuthName = registration.Login;
                //    db.SaveChanges();
                //    return View("RegistrationResult", registration);
                //}    
            }
            else
            {
                ModelState.AddModelError("", "Data is not correct");
            }

            /*Это выполняется если мы ввели чтото неудачно#1#
            ViewBag.Colors = registration.ColorsForFirlds;
            return View();
        }*/
    }
}
