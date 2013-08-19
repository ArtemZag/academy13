using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.Photo;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [Authorize]
	[RoutePrefix("Api/Photo")]
    public class PhotoController : ApiController
    {
        private readonly IPhotoService photoService;

        public PhotoController(IPhotoService photoService)
        {
            this.photoService = photoService;
        }

        [GET]
        public HttpResponseMessage GetPhotos(string albumName, int skip, int take)
        {
            try
            {
                var photoModels = photoService.GetPhotos(User.Identity.Name, albumName, skip, take);

                var photoViewModels = photoModels.Select(PhotoViewModel.FromModel).ToList();

                var responseData = new ObjectContent<IEnumerable<PhotoViewModel>>(photoViewModels, new JsonMediaTypeFormatter());

                var response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = responseData
                };

                return response;
            }
            catch (NoEnoughPrivileges ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [GET]
        public HttpResponseMessage GetPhotos(int skip, int take, int albumId)
        {
            try
            {
                var photoModels = photoService.GetPhotos(User.Identity.Name, albumId, skip, take);

                var photoViewModels = photoModels.Select(PhotoViewModel.FromModel).ToList();

                var responseData = new ObjectContent<IEnumerable<PhotoViewModel>>(photoViewModels, new JsonMediaTypeFormatter());

                var response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = responseData
                };

                return response;
            }
            catch (NoEnoughPrivileges ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [GET("GetLikes/{photoId}")]
        public HttpResponseMessage GetLikes(int photoId)
        {
            try
            {
                var photoLikeViewModels = photoService
                    .GetLikes(User.Identity.Name, photoId)
                    .Select(PhotoLikeViewModel.FromModel)
                    .ToList();

                var responseData = new ObjectContent<IEnumerable<PhotoLikeViewModel>>(photoLikeViewModels, new JsonMediaTypeFormatter());

                var response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = responseData
                };

                return response;
            }
            catch (NoEnoughPrivileges ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [POST("AddLike/{photoId}")] // TODO Must be replaced with PUT method [but it not work, while it forbidden in server settings]
        public HttpResponseMessage AddLike(int photoId)
        {
            try
            {
                photoService.AddLike(User.Identity.Name, photoId);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return this.GetLikes(photoId);
        }

        [GET]
        public HttpResponseMessage GetAllUserPhotos(int skip, int take)
        {
            List<PhotoViewModel> viewModels;

            try
            {
                viewModels = photoService
                    .GetPhotos(User.Identity.Name, skip, take)
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


        [GET]
        public HttpResponseMessage GetAllAvailablePhotos(int skip, int take)
        {
            List<PhotoViewModel> viewModels;

            try
            {
                viewModels = photoService
                    .GetPublicPhotos(User.Identity.Name, skip, take)
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

        [GET("{photoId}")]
        public HttpResponseMessage GetPhoto(int photoId)
        {
            try
            {
                var photoModel = photoService.GetPhoto(User.Identity.Name, photoId);

                var photoViewModel = PhotoViewModel.FromModel(photoModel);

                var responseData = new ObjectContent<PhotoViewModel>(photoViewModel, new JsonMediaTypeFormatter());

                var response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = responseData
                };

                return response;
            }
            catch (NoEnoughPrivileges ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
