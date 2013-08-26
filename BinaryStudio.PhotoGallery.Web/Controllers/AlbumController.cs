using System.Web.Mvc;
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
	    private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

		[GET("{albumId}")]
        public ActionResult Index(int albumId)
		{
		    var album = _albumService.GetAlbum(albumId);

		    var albumViewModel = album.ToAlbumViewModel("TODO collage source");

            return View("Index", albumViewModel);
        }
    }
}
