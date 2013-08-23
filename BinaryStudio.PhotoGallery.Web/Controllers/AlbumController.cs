using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.ViewModels;

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
		    var mAlbum = _albumService.GetAlbum(albumId);
            return View("Index", AlbumViewModel.FromModel(mAlbum,_resizePhoto));
        }
    }
}
