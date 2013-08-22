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
    [RoutePrefix("api/album")]
    public class AlbumController : BaseApiController
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

            bool albumAlreadyExist = _albumService.IsExist(User.Id, albumName);

            if (albumAlreadyExist)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    string.Format("Album '{0}' already exist", albumName));
            }

            _albumService.CreateAlbum(User.Id, albumName);

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [GET("all/name")]
        public IEnumerable<string> GetAllNames()
        {
            return _albumService.GetAllAlbums(User.Id)
                .Where(album => album.Name != "Temporary")
                .Select(album => album.Name);
        }
    }
}