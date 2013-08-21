using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels.PhotoPage;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [RoutePrefix("api/photo")]
    public class PhotoCommentController : ApiController
    {
        private readonly IPhotoCommentService _photoCommentService;
        private readonly IUserService _userService;

        public PhotoCommentController(IUserService userService, IPhotoCommentService photoCommentService)
        {
            _userService = userService;
            _photoCommentService = photoCommentService;
        }

        [GET("{photoId}/comments?{skip}&{take}")]
        public HttpResponseMessage GetPhotoComments(int photoId, int skip, int take)
        {
            try
            {
                int userId = _userService.GetUserId(User.Identity.Name);

                var viewModels = new List<PhotoCommentViewModel>();

                IEnumerable<PhotoCommentModel> models = _photoCommentService.GetPhotoComments(userId, photoId, skip,
                    take);

                viewModels.AddRange(from model in models
                                    let userModel = _userService.GetUser(model.UserId)
                                    select PhotoCommentViewModel.FromModel(model, userModel));

                var responseData = new ObjectContent<IEnumerable<PhotoCommentViewModel>>
                    (viewModels, new JsonMediaTypeFormatter());

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

        [POST("comment")]
        public HttpResponseMessage AddPhotoComment(NewCommentViewModel viewModel)
        {
            int userId = _userService.GetUserId(User.Identity.Name);

            var photoCommentModel = new PhotoCommentModel(userId, viewModel.PhotoId, viewModel.CommentText,
                viewModel.Reply);

            try
            {
                _photoCommentService.AddPhotoComment(userId, photoCommentModel);
            }
            catch (NoEnoughPrivileges ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}