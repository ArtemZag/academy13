using System.Web.Http;
using AttributeRouting;
using System.Net.Http;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using System.Net;
using System.Web.Security;
using BinaryStudio.PhotoGallery.Domain.Services;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    using BinaryStudio.PhotoGallery.Domain.Exceptions;
    using BinaryStudio.PhotoGallery.Web.Utils;

    [RoutePrefix("Api/Account")]
    public class AccountController : ApiController
    {
        private readonly IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        [POST]
        public HttpResponseMessage Signin([FromBody] AuthorizationViewModel viewModel)
        {
            var userNotValid = !userService.IsUserValid(viewModel.Email, viewModel.Password);

            if (userNotValid)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            FormsAuthentication.SetAuthCookie(viewModel.Email, viewModel.RememberMe);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [POST]
        public HttpResponseMessage Signup([FromBody] RegistrationViewModel viewModel)
        {
            try
            {
                var user = ModelConverter.GetModel(viewModel);

                userService.CreateUser(user);

                FormsAuthentication.SetAuthCookie(user.Email, false);
            }
            catch(UserAlreadyExistException)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
