using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.Utils;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
	[RoutePrefix("Album")]
    public class AlbumController : Controller
	{
	    private readonly IAlbumService albumService;

        public AlbumController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

		[GET("{albumId}")]
        public ActionResult PhotoView(int albumId)
		{
		    var mAlbum = albumService.GetAlbum(albumId);
            return View("Index", AlbumViewModel.FromModel(mAlbum));
        }
    }
}
