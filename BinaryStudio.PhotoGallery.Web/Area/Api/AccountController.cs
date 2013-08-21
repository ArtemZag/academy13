using System;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System.Web.Security;
using AttributeRouting;
using AttributeRouting.Web.Http;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Web.ViewModels.Authorization;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [RoutePrefix("api")]
    public class AccountController : ApiController
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [POST("login")]
        public HttpResponseMessage Signin([FromBody] SigninViewModel viewModel)
        {
            if (viewModel == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unknown error");
            }

            try
            {
                var userNotValid = !_userService.IsUserValid(viewModel.Email, viewModel.Password);

                if (userNotValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect email or password");
                }

                FormsAuthentication.SetAuthCookie(viewModel.Email.ToLower(), viewModel.RememberMe);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [POST("registration")]
        public HttpResponseMessage Signup([FromBody] SignupViewModel viewModel)
        {
            if (viewModel == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unknown error");
            }

            try
            {
                _userService.ActivateUser(viewModel.Email, viewModel.Password, viewModel.Invite);

                FormsAuthentication.SetAuthCookie(viewModel.Email.ToLower(), false);

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (UserAlreadyExistException ex)
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
