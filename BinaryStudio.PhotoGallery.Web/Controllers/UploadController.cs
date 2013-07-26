using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
	[RoutePrefix("Upload")]
    public class UploadController : Controller
    {
		[GET("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
