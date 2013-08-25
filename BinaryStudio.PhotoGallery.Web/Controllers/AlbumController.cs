﻿using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.Extensions.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("album")]
    public class AlbumController : BaseController
	{
	    private readonly IAlbumService albumService;

        public AlbumController(IAlbumService albumService, IPathUtil pathUtil, IAlbumTagService albumTagService)
        {
            this.albumService = albumService;
        }

		[GET("{albumId}")]
        public ActionResult Index(int albumId)
		{
		    var album = albumService.GetAlbum(albumId);

            return View("Index", AlbumViewModel.FromModel(album));
        }
    }
}
