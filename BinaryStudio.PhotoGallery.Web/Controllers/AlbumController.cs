using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("album")]
    public class AlbumController : Controller
	{
	    private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

		[GET("{albumId}")]
        public ActionResult Index(int albumId)
		{
		    var mAlbum = _albumService.GetAlbum(albumId);
            return View("Index", AlbumViewModel.FromModel(mAlbum));
        }
    }
}
