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
	    private readonly IModelConverter modelConverter;

        public AlbumController(IAlbumService albumService, IModelConverter modelConverter)
        {
            this.albumService = albumService;
            this.modelConverter = modelConverter;
        }

		[GET("{albumId}")]
        public ActionResult PhotoView(int albumId)
		{
		    var mAlbum = albumService.GetAlbum(albumId);
		    var vmAlbum = modelConverter.GetViewModel(mAlbum);
            return View("Index", vmAlbum);
        }
    }
}
