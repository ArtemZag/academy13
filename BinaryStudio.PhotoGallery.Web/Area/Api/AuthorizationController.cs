using System;
using System.Web.Http;
using AttributeRouting;
using System.Net.Http;
using AttributeRouting.Web.Mvc;
using System.Net;
using System.Web.Security;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Web.Utils;
using BinaryStudio.PhotoGallery.Web.ViewModels.Authorization;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [RoutePrefix("Api/Authorization")]
    public class AuthorizationController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IModelConverter _modelConverter;

        public AuthorizationController(IUserService userService, IModelConverter modelConverter)
        {
            _userService = userService;
            _modelConverter = modelConverter;
        }

        [POST]
        public HttpResponseMessage Signin([FromBody] SigninViewModel viewModel)
        {
            if (viewModel == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            var userNotValid = !_userService.IsUserValid(viewModel.Email, viewModel.Password);

            if (userNotValid)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            FormsAuthentication.SetAuthCookie(viewModel.Email.ToLower(), viewModel.RememberMe);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [POST]
        public HttpResponseMessage Signup([FromBody] SignupViewModel viewModel)
        {
            if (viewModel == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            try
            {
                var user = _modelConverter.GetModel(viewModel);

                _userService.ActivateUser(viewModel.Email, viewModel.Password/*, viewModel.Invite*/);

                FormsAuthentication.SetAuthCookie(user.Email.ToLower(), false);
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
