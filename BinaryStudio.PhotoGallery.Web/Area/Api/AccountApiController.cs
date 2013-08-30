using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.Security;
using AttributeRouting;
using AttributeRouting.Web.Http;
using BinaryStudio.PhotoGallery.Core.EmailUtils;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Core.UserUtils;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.CustomStructure;
using BinaryStudio.PhotoGallery.Web.Properties;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.Account;
using BinaryStudio.PhotoGallery.Web.ViewModels.Admin;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [RoutePrefix("api")]
    public class AccountApiController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;

        public AccountApiController(IUserService userService, IEmailSender emailSender)
        {
            _userService = userService;
            _emailSender = emailSender;
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

        [POST("invite")]
        public HttpResponseMessage SendInvite([FromBody] InviteUserViewModel viewModel)
        {
            try
            {
                string host = ConfigurationManager.AppSettings["NotificationHost"];
                string fromEmail = ConfigurationManager.AppSettings["NotificationEmail"];
                string fromPass = ConfigurationManager.AppSettings["NotificationPassword"];

                string toEmail = viewModel.Email;

                string mailSubject = Resources.Email_InviteSubject;

                var activateCode = _userService.CreateUser(viewModel.Email, viewModel.FirstName, viewModel.LastName);

                // TODO replace hard link
                string activationLink = "http://localhost:57367/registration/" + activateCode;

                string text = string.Format(
                    Resources.Email_InviteMessage,
                    viewModel.FirstName,
                    viewModel.LastName,
                    activationLink);

                _emailSender.Send(host, fromEmail, fromPass, toEmail, mailSubject, text);

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
        public HttpResponseMessage RemindPass(RemindPassViewModel remindPassViewModel)
        {
            try
            {
                var mUser = _userService.GetUser(remindPassViewModel.Email);
                string host = ConfigurationManager.AppSettings["NotificationHost"];
                string fromEmail = ConfigurationManager.AppSettings["NotificationEmail"];
                string fromPass = ConfigurationManager.AppSettings["NotificationPassword"];
                string mailSubject = Resources.Email_RemindPassSubject;
                var emailHash = WebUtility.UrlEncode(_userService.UserRestorePasswordAsk(mUser));
                string remindLink = string.Format("<a href='http://{0}/remind/{1}/{2}'>http://{0}/remind/{1}/{2}</a>", 
                    HttpContext.Current.Request.Url.Authority, mUser.Id, emailHash);
                string text = string.Format(
                    "<p>Dear {0} {1}!\n\n<p>You or someone else asked to recover password procedure.</p> " +
                    "<p>To change you password, follow this link:\n{2}. <br/>This link will be available until the tomorrow.</p>" +
                    "<p>If it wasn't you, just ignore this message.</p>",
                    mUser.FirstName, mUser.LastName, remindLink);
                _emailSender.Send(host, fromEmail, fromPass, remindPassViewModel.Email, mailSubject, text);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (UserNotFoundException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, e.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [POST("changepass")]
        public HttpResponseMessage ChangePass(SignupViewModel signupView)
        {
            try
            {
                _userService.UserRestorePasswordChangePass(signupView.Email, signupView.Password);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}