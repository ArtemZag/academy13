using System;
using System.Web.Helpers;
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
        private readonly IUserService userService;

        private readonly IModelConverter modelConverter;

        public AuthorizationController(IUserService userService, IModelConverter modelConverter)
        {
            this.userService = userService;
            this.modelConverter = modelConverter;
        }

        [POST]
        public HttpResponseMessage Signin([FromBody] SigninViewModel viewModel)
        {
            if (viewModel == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            var userNotValid = !userService.IsUserValid(viewModel.Email, viewModel.Password);

            if (userNotValid)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            FormsAuthentication.SetAuthCookie(viewModel.Email, viewModel.RememberMe);

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
                var user = modelConverter.GetModel(viewModel);

                userService.CreateUser(user);

                FormsAuthentication.SetAuthCookie(user.Email, false);
            }
            catch (UserAlreadyExistException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        [POST]
        public HttpResponseMessage ChangePassword([FromBody] SignupViewModel viewModel)
        {
            if (viewModel == null){ return new HttpResponseMessage(HttpStatusCode.BadRequest); }

            try
            {
                userService.ActivateUser(viewModel.Email, viewModel.Password, viewModel.Invite);
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

		[GET]
		public HttpResponseMessage GetEmail(string hash)
		{
			var user = userService.FindNonActivatedUser(hash);
			if (user!=null)
			{
				return new HttpResponseMessage
					{
						StatusCode = HttpStatusCode.OK,
						Content = new StringContent(user.Email)
					};
			}
			else return new HttpResponseMessage(HttpStatusCode.NotFound);
		}
    }
}
