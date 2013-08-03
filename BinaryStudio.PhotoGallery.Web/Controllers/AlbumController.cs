using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
	[RoutePrefix("Albums")]
    public class AlbumController : Controller
    {
		[GET("PhotoView/{albumId}/{photoId}")]
        public ActionResult PhotoView(int albumId, int photoId){
            return View(new PhotoViewModel());
        }
    }
}
