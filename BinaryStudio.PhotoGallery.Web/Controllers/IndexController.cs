using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
	[RoutePrefix("Index")]
    public class IndexController : Controller
    {
        [GET("")]
        public ViewResult Index()
        {
            int hour = DateTime.Now.Hour;
            ViewBag.PartOfDay = hour < 12 ? "Good morning" : "Good afternoon";
            return View();
        }

		[GET("")]
        public ActionResult Signin()
        {
            return View();
        }

        [GET("")]
        public ActionResult Signup()
        {
            return View();
        }
    }
}
