using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        [POST]
        public HttpResponseMessage CreateNewAlbum([FromBody] string albumName)
        {
            if (albumName == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unknown error");
            }

            var albumAlreadyExist = _albumService.IsExist(User.Identity.Name, albumName);

            if (albumAlreadyExist)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Format("Album '{0}' already exist", albumName));
            }

            _albumService.CreateAlbum(User.Identity.Name, albumName);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        [GET]
        public IEnumerable<string> GetAllAlbumsName()
        {
            return _albumService.GetAllAlbums(User.Identity.Name).Select(album => album.AlbumName);
        }
    }
}
