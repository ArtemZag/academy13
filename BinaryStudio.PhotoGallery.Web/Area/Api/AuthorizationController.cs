using System;
using System.Web.Http;
using AttributeRouting;
using System.Net.Http;
using AttributeRouting.Web.Mvc;
using System.Net;
using System.Web.Security;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Web.ViewModels.Authorization;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [RoutePrefix("Api/Authorization")]
    public class AuthorizationController : ApiController
    {
        private readonly IUserService _userService;

        public AuthorizationController(IUserService userService)
        {
            _userService = userService;
        }

        [POST]
        public HttpResponseMessage Signin([FromBody] SigninViewModel viewModel)
        {
            if (viewModel == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unknown error");
            }

            var userNotValid = !_userService.IsUserValid(viewModel.Email, viewModel.Password);

            if (userNotValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect email or password");
            }

            FormsAuthentication.SetAuthCookie(viewModel.Email.ToLower(), viewModel.RememberMe);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [POST]
        public HttpResponseMessage Signup([FromBody] SignupViewModel viewModel)
        {
            if (viewModel == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unknown error");
            }

            try
            {
                _userService.ActivateUser(viewModel.Email, viewModel.Password);
                FormsAuthentication.SetAuthCookie(viewModel.Email.ToLower(), false);
            }
            catch (UserAlreadyExistException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
