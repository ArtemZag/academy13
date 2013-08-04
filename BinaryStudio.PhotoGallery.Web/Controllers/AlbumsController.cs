using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
	[RoutePrefix("Albums")]
    public class AlbumsController : Controller
    {
		[GET("")]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AjaxResponse(int start, int end)
        {
            return Json(new object());
        }
    }
}
