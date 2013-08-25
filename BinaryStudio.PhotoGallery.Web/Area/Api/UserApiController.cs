using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Domain.Services.Tasks;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Extensions.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.User;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [RoutePrefix("api/user")]
    public class UserApiController : BaseApiController
    {
        private readonly IAlbumService _albumService;
        private readonly IPhotoService _photoService;
        private readonly IResizePhotoService _resizePhotoService;
        private readonly IUserService _userService;
        private readonly IUsersMonitorTask _usersMonitorTask;

        public UserApiController(
            IUserService userService,
            IPhotoService photoService,
            IAlbumService albumService,
            IUsersMonitorTask usersMonitorTask,
            IResizePhotoService resizePhotoService)
        {
            _userService = userService;
            _photoService = photoService;
            _albumService = albumService;
            _usersMonitorTask = usersMonitorTask;
            _resizePhotoService = resizePhotoService;
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

                int userAlbumCount = _albumService.AlbumsCount(User.Id);
                int userPhotoCount = _photoService.PhotoCount(User.Id);
                string userAbatarPath = _resizePhotoService.GetUserAvatar(userModel.Id, AvatarSize.Medium);

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
