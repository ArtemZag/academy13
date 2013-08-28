using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Management;
using Antlr.Runtime.Tree;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.Extensions.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [Authorize]
    [RoutePrefix("api/album/tags")]
    public class AlbumTagApiController : BaseApiController
    {
        private readonly ITagService tagService;

        public AlbumTagApiController(ITagService tagService)
        {
            this.tagService = tagService;
        }

        [GET("{albumId: int}")]
        public HttpResponseMessage GetTags(int albumId)
        {
            try
            {
                IEnumerable<string> result =
                    tagService.GetAlbumTags(albumId).Select(model => model.TagName);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);                
            }
        }
    }
}