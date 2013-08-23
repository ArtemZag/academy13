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
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
	[RoutePrefix("api/publicphoto")]
    public class PublicPhotoController : BaseApiController
    {
        private readonly IPhotoService _photoService;

        public PublicPhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [GET("{take: int}")]
        public HttpResponseMessage GetAllPublicPhotos(int take)
        {
            try
            {
                List<PhotoViewModel> viewModels = _photoService
                    .GetRandomPublicPhotos(3, take) // todo make right
                    .Select(PhotoViewModel.FromModel).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, viewModels, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
