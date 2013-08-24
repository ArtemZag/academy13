using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Extensions;
using BinaryStudio.PhotoGallery.Web.Extensions.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
	[RoutePrefix("api/user")]
    public class UserApiController : BaseApiController
    {
	    private readonly IUserService _userService;
	    private readonly IPhotoService _photoService;
	    private readonly IAlbumService _albumService;
	    private readonly IResizePhotoService _resizePhotoService;

	    public UserApiController(
            IUserService userService,
            IPhotoService photoService,
            IAlbumService albumService,
            IResizePhotoService resizePhotoService)
	    {
	        _userService = userService;
	        _photoService = photoService;
	        _albumService = albumService;
	        _resizePhotoService = resizePhotoService;
	    }

	    [GET("")]
        public HttpResponseMessage GetCurrentUserInfo()
	    {
	        return this.GetUserInfo(User.Id);
	    }

        [GET("{userId:int}")]
        public HttpResponseMessage GetUserInfo(int userId)
        {
            try
            {
                UserModel userModel = _userService.GetUser(userId);

                var userAlbumCount = _albumService.AlbumsCount(User.Id);
                var userPhotoCount = _photoService.PhotoCount(User.Id);
                var userAbatarPath = _resizePhotoService.GetUserAvatar(userModel.Id, AvatarSize.Medium);

                var viewModel = userModel.ToUserInfoViewModel(userAlbumCount, userPhotoCount, userAbatarPath);

                return Request.CreateResponse(HttpStatusCode.OK, viewModel, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
