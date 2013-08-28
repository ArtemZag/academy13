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
using BinaryStudio.PhotoGallery.Web.Extensions.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.Photo;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [Authorize]
    [RoutePrefix("api/photo")]
    public class PhotoApiController : BaseApiController
    {
        private readonly IPhotoService _photoService;

        public PhotoApiController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [GET("?{albumId:int}&{skip:int}&{take:int}")]
        public HttpResponseMessage GetPhotos(int albumId, int skip, int take)
        {
            try
            {
                IEnumerable<PhotoModel> photoModels = _photoService.GetPhotos(User.Id, albumId, skip, take);

                List<PhotoViewModel> photoViewModels = photoModels.Select(model => model.ToPhotoViewModel()).ToList();

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
                    .Select(model => model.ToPhotoLikeViewModel())
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

                // TODO COSTIL
                List<PhotoLikeViewModel> photoLikeViewModels = _photoService
                    .GetLikes(User.Id, photoId)
                    .Select(model => model.ToPhotoLikeViewModel())
                    .ToList();

                return Request.CreateResponse(HttpStatusCode.Created, photoLikeViewModels, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [GET("all?{skip:int}&{take:int}")]
        public HttpResponseMessage GetAllUserPhotos(int skip, int take)
        {
            try
            {
                List<PhotoViewModel> viewModels = _photoService
                    .GetPhotos(User.Id, skip, take)
                    .Select(model => model.ToPhotoViewModel()).ToList();

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
                    .Select(model => model.ToPhotoViewModel()).ToList();

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

                PhotoViewModel photoViewModel = photoModel.ToPhotoViewModel();

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

        [GET("{photoId}/photobytags")]
        public HttpResponseMessage GetPhotosByTags(int photoId)
        {
            try
            {
                List<PhotoViewModel> photosByTags = _photoService
                    .GetPhotosByTags(User.Id, photoId, 0, 10)
                    .Select(model => model.ToPhotoViewModel())
                    .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, photosByTags, new JsonMediaTypeFormatter());
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

        [DELETE("{photoId:int}")]
        public HttpResponseMessage Delete(int photoId)
        {
            if (photoId < 1)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Can't delete photo by invalid id");
            }

            try
            {
                _photoService.DeletePhoto(User.Id, photoId);
            }
            catch (NoEnoughPrivilegesException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [POST("movephoto?{photoId}&{albumId}")]
        public HttpResponseMessage MovePhoto(int photoId, int albumId)
        {
            if (photoId < 1 || albumId < 1)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid photoID or albumID");
            }

            try
            {
                _photoService.MovePhotoToAlbum(User.Id, photoId, albumId);
            }
            catch (NoEnoughPrivilegesException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}