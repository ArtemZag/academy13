using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [Authorize]
	[RoutePrefix("Api/Album")]
    public class AlbumController : ApiController
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [GET]
        public IEnumerable<object> GetAllAlbumsName()
        {
            return _albumService.GetAllAlbums(User.Identity.Name).Select(album => new { Id = album.Id, Name = album.AlbumName });
        }
    }
}
