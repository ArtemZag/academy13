using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Extensions.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [RoutePrefix("api/user")]
    public class UserApiController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserApiController(IUserService userService)
        {
            _userService = userService;
        }

        [GET("")]
        public HttpResponseMessage GetCurrentUserInfo()
        {
            return GetUserInfo(User.Id);
        }

        [GET("{userId:int}")]
        public HttpResponseMessage GetUserInfo(int userId)
        {
            try
            {
                UserModel userModel = _userService.GetUser(userId);

                UserViewModel viewModel = userModel.ToUserViewModel();

                return Request.CreateResponse(HttpStatusCode.OK, viewModel, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [GET("all?{skip:int}&{take:int}")]
        public HttpResponseMessage GetAll(int skip, int take)
        {
            try
            {
                List<UserViewModel> usersViewModels = _userService.GetAllUsers(skip, take)
                    .Select(userModel => userModel.ToUserViewModel())
                    .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, usersViewModels, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [DELETE("")]
        public HttpResponseMessage Delete(int userId)
        {
            try
            {
                _userService.DeleteUser(userId);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
