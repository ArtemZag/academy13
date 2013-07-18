using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ViewResult Index()
        {
            int hour = DateTime.Now.Hour;
            ViewBag.PartOfDay = hour < 12 ? "Good morning" : "Good afternoon";
            return View();
        }
    }
}
