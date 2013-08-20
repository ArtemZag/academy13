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
	    private readonly IPhotoService photoService;

        public AlbumController(IAlbumService albumService, IPhotoService photoService)
        {
            this.albumService = albumService;
            this.photoService = photoService;
        }

		[GET("{albumId}")]
        public ActionResult PhotoView(int albumId)
		{
		    var mAlbum = albumService.GetAlbum(albumId);
            return View("Index", AlbumViewModel.FromModel(mAlbum));
        }
    }
}
