﻿using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Http;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [RoutePrefix("gallery")]
    public class GalleryController : BaseController
    {
        [GET("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}