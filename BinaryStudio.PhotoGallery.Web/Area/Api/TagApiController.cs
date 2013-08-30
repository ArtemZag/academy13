using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels.Photo;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [Authorize]
    [RoutePrefix("api/tag")]
    public class TagApiController : BaseApiController
    {
        private readonly IAlbumService albumService;
	    private readonly ITagService tagService;

        public TagApiController(IAlbumService albumService, ITagService tagService)
        {
            this.albumService = albumService;
	        this.tagService = tagService;
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

		[GET("{photoId:int}/phototags")]
		public HttpResponseMessage GetPhotoTags(int photoId)
		{
			try
			{
				IEnumerable<string> tags = tagService.GetTagsByPhoto(photoId);

				return Request.CreateResponse(HttpStatusCode.OK, tags);
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
			}
		}

		[POST("")]
		public HttpResponseMessage AddTagsToPhoto([FromBody] PhotoTagsViewModel photoTags)
		{
			try
			{
				string[] allTags = photoTags.Tags.Split(' ');
				tagService.RemoveAllTagsByPhoto(photoTags.PhotoId);
				foreach (var tag in allTags)
				{
					tagService.AddPhotoTag(photoTags.PhotoId, tag);
				}
				return Request.CreateResponse(HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
			}
		}
    }
}