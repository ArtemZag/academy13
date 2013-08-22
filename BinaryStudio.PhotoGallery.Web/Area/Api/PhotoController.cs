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
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.Photo;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [Authorize]
    [RoutePrefix("api/photo")]
    public class PhotoController : BaseApiController
    {
        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [GET("?{albumId:int}&{skip:int}&{take:int}")]
        public HttpResponseMessage GetPhotos(int albumId, int skip, int take)
        {
            try
            {
                IEnumerable<PhotoModel> photoModels = _photoService.GetPhotos(User.Id, albumId, skip, take);

                List<PhotoViewModel> photoViewModels = photoModels.Select(PhotoViewModel.FromModel).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, photoViewModels, new JsonMediaTypeFormatter());
            }
            catch (NoEnoughPrivilegesException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [GET("{photoId:int}/likes")]
        public HttpResponseMessage GetLikes(int photoId)
        {
            try
            {
                List<PhotoLikeViewModel> photoLikeViewModels = _photoService
                    .GetLikes(User.Id, photoId)
                    .Select(PhotoLikeViewModel.FromModel)
                    .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, photoLikeViewModels, new JsonMediaTypeFormatter());
            }
            catch (NoEnoughPrivilegesException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [POST("like")]
        public HttpResponseMessage AddLike([FromBody] int photoId)
        {
            try
            {
                _photoService.AddLike(User.Id, photoId);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        [GET("all?{skip:int}&{take:int}")]
        public HttpResponseMessage GetAllUserPhotos(int skip, int take)
        {
            try
            {
                List<PhotoViewModel> viewModels = _photoService
                    .GetPhotos(User.Id, skip, take)
                    .Select(PhotoViewModel.FromModel).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, viewModels, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [GET("allusers?{skip:int}&{take:int}")]
        public HttpResponseMessage GetAllPublicPhotos(int skip, int take)
        {
            try
            {
                List<PhotoViewModel> viewModels = _photoService
                    .GetPublicPhotos(User.Id, skip, take)
                    .Select(PhotoViewModel.FromModel).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, viewModels, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [GET("{photoId}")]
        public HttpResponseMessage GetPhoto(int photoId)
        {
            try
            {
                PhotoModel photoModel = _photoService.GetPhoto(User.Id, photoId);

                PhotoViewModel photoViewModel = PhotoViewModel.FromModel(photoModel);

                return Request.CreateResponse(HttpStatusCode.OK, photoViewModel, new JsonMediaTypeFormatter());
            }
            catch (NoEnoughPrivilegesException ex)
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