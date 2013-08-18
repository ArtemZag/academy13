using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.Utils;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [Authorize]
	[RoutePrefix("Api/Photo")]
    public class PhotoController : ApiController
    {
        public struct GetPhotosOptions
        {
            public int SkipCount;
            public int TakeCount;
        }

        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [POST("UserFlow")]
	    public HttpResponseMessage GetAllUserPhotos([FromBody] GetPhotosOptions options)
        {
            List<PhotoViewModel> viewModels;

            try
            {
                viewModels = _photoService
                    .GetPhotos(User.Identity.Name, options.SkipCount, options.TakeCount)
                    .Select(PhotoViewModel.FromModel).ToList();
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            var responseData = new ObjectContent<IEnumerable<PhotoViewModel>>
                (viewModels, new JsonMediaTypeFormatter());

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = responseData
            };

            return response;
	    }

        [POST("AllAvailable")]
        public HttpResponseMessage GetAllAvailablePhotos([FromBody] GetPhotosOptions options)
        {
            List<PhotoViewModel> viewModels;

            try
            {
                viewModels = _photoService
                    .GetPublicPhotos(User.Identity.Name, options.SkipCount, options.TakeCount)
                    .Select(PhotoViewModel.FromModel).ToList();
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            var responseData = new ObjectContent<IEnumerable<PhotoViewModel>>
                (viewModels, new JsonMediaTypeFormatter());

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = responseData
            };

            return response;
        }
    }
}
