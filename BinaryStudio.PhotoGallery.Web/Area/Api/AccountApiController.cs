using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.Security;
using AttributeRouting;
using AttributeRouting.Web.Http;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.CustomStructure;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.Account;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [RoutePrefix("api")]
    public class AccountApiController : BaseApiController
    {
        private readonly IUserService _userService;
        

        public AccountApiController(IUserService userService)
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
                bool userNotValid = !_userService.IsUserValid(viewModel.Email, viewModel.Password);

                if (userNotValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect email or password");
                }

                UserModel userModel = _userService.GetUser(viewModel.Email);

                var serializer = new JavaScriptSerializer();

                var serializeModel = new UserInfoSerializeModel
                {
                    Id = userModel.Id,
                    Email = userModel.Email,
                    IsAdmin = userModel.IsAdmin
                };

                string serializedUserData = serializer.Serialize(serializeModel);

                var authTicket = new FormsAuthenticationTicket(
                    1,
                    userModel.Id.ToString(),
                    DateTime.Now,
                    DateTime.Now.AddDays(30),
                    viewModel.RememberMe,
                    serializedUserData
                    );

                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
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

                return Request.CreateResponse(HttpStatusCode.OK);
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

        [POST("remind")]
        public HttpResponseMessage RemindPassword([FromBody] RemindPassViewModel viewModel)
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}