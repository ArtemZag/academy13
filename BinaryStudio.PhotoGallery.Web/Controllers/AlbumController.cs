using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
	[RoutePrefix("Album")]
    public class AlbumController : Controller
	{
	    private IAlbumService albumService;

        public AlbumController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

		[GET("/{albumId}")]
        public ActionResult PhotoView(int albumId)
		{
		    var mAlbum = albumService.GetAlbum(albumId);
		    var vmInfoAlbum = new InfoAlbumViewModel()
		        {
                    AlbumModel = mAlbum
		        };
            return View("Index", vmInfoAlbum);
        }
    }
}
