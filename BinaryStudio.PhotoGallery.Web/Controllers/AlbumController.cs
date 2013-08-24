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
        private readonly IResizePhotoService _resizePhoto;
        public AlbumController(IAlbumService albumService, IResizePhotoService resize)
        {
            _albumService = albumService;
            _resizePhoto = resize;
        }

		[GET("{albumId}")]
        public ActionResult Index(int albumId)
		{
		    var model = _albumService.GetAlbum(albumId);

            // TODO use pathUtil here to get path to album collage
		    var collageSource = _resizePhoto.GetCollage(User.Id, model.Id, 256, 64, 3);

		    var viewModel = model.ToAlbumViewModel(collageSource);

            return View("Index", viewModel);
        }
    }
}
