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
	[RoutePrefix("Api/Photo")]
    public class PhotoController : ApiController
    {
        public class GetPhotosOptions
        {
            public int SkipCount { get; set; }
            public int TakeCount { get; set; }
            public int AlbumId { get; set; }
        }

        private readonly IPhotoService photoService;
        private readonly IModelConverter modelConverter;

        public PhotoController(IPhotoService photoService, IModelConverter modelConverter)
        {
            this.photoService = photoService;
            this.modelConverter = modelConverter;
        }

        [HttpPost]
	    public HttpResponseMessage GetAllUserPhotos(GetPhotosOptions options)
        {
            List<PhotoViewModel> viewModels;

            try
            {
                viewModels = photoService
                    .GetPhotos(User.Identity.Name, options.SkipCount, options.TakeCount)
                    .Select(modelConverter.GetViewModel).ToList();
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

        [HttpPost]
        public HttpResponseMessage GetPhotosFromAlbum(GetPhotosOptions options)
        {
            List<PhotoViewModel> viewModels;

            try
            {
                viewModels = photoService
                    .GetPhotos(User.Identity.Name, options.AlbumId, options.SkipCount, options.TakeCount)
                    .Select(modelConverter.GetViewModel).ToList();
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

        [HttpPost]
        public HttpResponseMessage GetAllAvailablePhotos(GetPhotosOptions options)
        {
            List<PhotoViewModel> viewModels;

            try
            {
                viewModels = photoService
                    .GetPublicPhotos(User.Identity.Name, options.SkipCount, options.TakeCount)
                    .Select(modelConverter.GetViewModel).ToList();
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
