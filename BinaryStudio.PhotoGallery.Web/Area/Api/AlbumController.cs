using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using BinaryStudio.PhotoGallery.Domain.Services;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [Authorize]
    [RoutePrefix("api/album")]
    public class AlbumController : ApiController
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [POST("")]
        public HttpResponseMessage Create([FromBody] string albumName)
        {
            if (albumName == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unknown error");
            }

            bool albumAlreadyExist = _albumService.IsExist(User.Identity.Name, albumName);

            if (albumAlreadyExist)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    string.Format("Album '{0}' already exist", albumName));
            }

            _albumService.CreateAlbum(User.Identity.Name, albumName);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        [GET("all/name")]
        public IEnumerable<string> GetAllNames()
        {
            return
                _albumService.GetAllAlbums(User.Identity.Name)
                    .Where(album => album.Name != "Temporary")
                    .Select(album => album.Name);
        }
    }
}