using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    public class RegistrationController : Controller
    {
        //
        // GET: /Registration/

        [HttpGet]
        public ViewResult RegistrationForm()
        {
            /*Это выполняется если мы перешли на страницу регистрации*/
            ViewBag.Colors = new string[] { "Black", "Black", "Black", "Black", "Black", "Black", "Black" };
            return View();
        }

        [HttpPost]
        public ViewResult RegistrationForm(RegistrationFormViewModel registration)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Colors = registration.ColorsForFirlds;
                /*
             Здесь мы должны внести запись в базу данных */
                return View("RegistrationResult", registration);
            }

            /*Это выполняется если мы ввели чтото неудачно*/
            ViewBag.Colors = registration.ColorsForFirlds;
            return View();

        }
    }
}
