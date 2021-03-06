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
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Extensions.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.User;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [Authorize]
    [RoutePrefix("api/user")]
    public class UserApiController : BaseApiController
    {
        private readonly IAlbumService _albumService;
        private readonly IPhotoService _photoService;
        private readonly IUserService _userService;

        public UserApiController(IUserService userService, IPhotoService photoService, IAlbumService albumService)
        {
            _userService = userService;
            _albumService = albumService;
            _photoService = photoService;
        }

        [GET("")]
        public HttpResponseMessage GetCurrent()
        {
            return Get(User.Id);
        }

        [GET("{userId:int}")]
        public HttpResponseMessage Get(int userId)
        {
            try
            {
                UserModel userModel = _userService.GetUser(userId);
                UserViewModel viewModel;
                if (userModel == null)
                {
                    userModel = new UserModel();
                    viewModel = userModel.ToNoneUserViewModel();
                }
                else
                {
                    int tempAlbumId = _albumService.GetAlbumId(userId, "Temporary");

                    int photoCount = _photoService.PhotoCount(userId, tempAlbumId);
                    int albumCount = _albumService.AlbumsCount(userId, tempAlbumId);
                    viewModel = userModel.ToUserViewModel(photoCount, albumCount);
                }
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
                List<UserViewModel> usersViewModels = _userService
                    .GetAllUsers(skip, take)
                    .Select(userModel => userModel.ToUserViewModel(_userService.IsUserBlocked(userModel.Id)))
                    .ToList();

                return Request.CreateResponse(HttpStatusCode.OK, usersViewModels, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}