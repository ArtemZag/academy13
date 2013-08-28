using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Extensions.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.Photo;
using Microsoft.AspNet.SignalR;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [Authorize]
    [RoutePrefix("api/photo")]
    public class PhotoCommentApiController : BaseApiController
    {
        private readonly IPhotoCommentService _photoCommentService;
        private readonly IUserService _userService;

        public PhotoCommentApiController(IUserService userService, IPhotoCommentService photoCommentService)
        {
            _userService = userService;
            _photoCommentService = photoCommentService;
        }

        [GET("{photoId}/comments?{skip}&{take}")]
        public HttpResponseMessage GetPhotoComments(int photoId, int skip, int take)
        {
            try
            {
                var viewModels = new List<PhotoCommentViewModel>();

                IEnumerable<PhotoCommentModel> photoComments = _photoCommentService.GetPhotoComments(User.Id, photoId,
                    skip, take);

                viewModels.AddRange(from commentModel in photoComments
                    let userModel = _userService.GetUser(commentModel.UserId)
                    select commentModel.ToPhotoCommentViewModel(userModel));

                return Request.CreateResponse(HttpStatusCode.OK, viewModels, new JsonMediaTypeFormatter());
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

        [POST("comment")]
        public HttpResponseMessage AddPhotoComment(NewCommentViewModel viewModel)
        {
            var photoCommentModel = new PhotoCommentModel(
                User.Id,
                viewModel.PhotoId,
                viewModel.CommentText,
                viewModel.Reply);

            try
            {
                _photoCommentService.AddPhotoComment(User.Id, photoCommentModel);

                var viewModels = new List<PhotoCommentViewModel>();
                IEnumerable<PhotoCommentModel> photoComments = _photoCommentService.GetPhotoComments(User.Id, viewModel.PhotoId, 0, 1000);
                viewModels.AddRange(from commentModel in photoComments
                    let userModel = _userService.GetUser(commentModel.UserId)
                    select commentModel.ToPhotoCommentViewModel(userModel));

                return Request.CreateResponse(HttpStatusCode.OK, viewModels, new JsonMediaTypeFormatter());
            }
            catch (NoEnoughPrivilegesException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }
}