using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using BinaryStudio.PhotoGallery.Domain.Services;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [Authorize]
    [RoutePrefix("api/tag")]
    public class TagApiController : BaseApiController
    {
        private readonly IAlbumService albumService;

        public TagApiController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        [GET("{albumId}")]
        public HttpResponseMessage GetAlbumTags(int albumId)
        {
            try
            {
                IEnumerable<string> tags = albumService.GetAlbumsTags(albumId);

                return Request.CreateResponse(HttpStatusCode.OK, tags);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);                
            }
        }
    }
}