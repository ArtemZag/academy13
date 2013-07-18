using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    public class AuthorizationController : Controller
    {
        //
        // GET: /Authorization/

        [HttpGet]
        public ViewResult AuthorizationForm()
        {
            return View();
        }
        [HttpPost]
        public ViewResult AuthorizationForm(AuthorizationViewModel authInfo)
        {
            /*
             * проверяем логин и пароль и пускаем дальше если всё ОК
             * */

            return View("AuthorizationForm", authInfo);

        }
    }
}
