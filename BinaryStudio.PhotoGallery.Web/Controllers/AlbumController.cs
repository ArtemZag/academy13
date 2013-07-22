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
    public class AlbumController : Controller
    {
		[HttpGet]
        public ActionResult Albumns()
		{
		    string name = User.Identity.Name;
            
            return View();
        }

    }
}
