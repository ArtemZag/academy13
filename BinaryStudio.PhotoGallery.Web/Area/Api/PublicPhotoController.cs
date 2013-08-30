using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.Extensions.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.Photo;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
	[RoutePrefix("api/publicphoto")]
    public class PublicPhotoApiController : BaseApiController
    {
        private readonly IPhotoService _photoService;

        public PublicPhotoApiController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [GET("{take: int}")]
        public HttpResponseMessage GetAllPublicPhotos(int take)
        {
            try
            {
                List<PhotoViewModel> viewModels = _photoService
                    .GetRandomPublicPhotos(take)
                    .Select(mPhoto => mPhoto.ToPhotoViewModel()).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, viewModels, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
