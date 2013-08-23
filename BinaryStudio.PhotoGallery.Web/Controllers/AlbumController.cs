﻿using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("album")]
    public class AlbumController : BaseController
	{
	    private readonly IAlbumService albumService;
        private readonly IPathUtil pathUtil;
        private readonly IAlbumTagService albumTagService;

        public AlbumController(IAlbumService albumService, IPathUtil pathUtil, IAlbumTagService albumTagService)
        {
            this.albumService = albumService;
            this.pathUtil = pathUtil;
            this.albumTagService = albumTagService;
        }

		[GET("{albumId}")]
        public ActionResult Index(int albumId)
		{
		    var mAlbum = albumService.GetAlbum(albumId);
            return View("Index", AlbumViewModel.FromModel(mAlbum, pathUtil, albumTagService));
        }
    }
}
