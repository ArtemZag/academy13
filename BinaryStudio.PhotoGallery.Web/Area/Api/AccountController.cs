﻿using System.Web.Http;
using AttributeRouting;
using System.Net.Http;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using System.Net;
using System.Web.Security;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Web.Utils;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [RoutePrefix("Api/Account")]
    public class AccountController : ApiController
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [POST]
        public HttpResponseMessage Signin([FromBody] AuthorizationViewModel viewModel)
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

            FormsAuthentication.SetAuthCookie(viewModel.Email, viewModel.RememberMe);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [POST]
        public HttpResponseMessage Signup([FromBody] RegistrationViewModel viewModel)
        {
            if (viewModel == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            try
            {
                var user = ModelConverter.GetModel(viewModel);

                _userService.CreateUser(user);

                FormsAuthentication.SetAuthCookie(user.Email, false);
            }
            catch(UserAlreadyExistException)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}
