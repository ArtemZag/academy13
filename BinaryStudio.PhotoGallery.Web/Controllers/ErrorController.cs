﻿using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
	[RoutePrefix("Error")]
    public class ErrorController : Controller
    {
		[GET]
        public ActionResult NotFound()
		{
		    Response.StatusCode = 404;
            return View();
        }

    }
}
