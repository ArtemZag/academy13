using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
	[RoutePrefix("StaticContent")]
    public class StaticContentController : Controller
    {
		[GET("")]
        public ActionResult PageNotFound()
        {
            return View();
        }

    }
}
