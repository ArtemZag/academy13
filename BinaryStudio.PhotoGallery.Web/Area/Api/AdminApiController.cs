using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using BinaryStudio.PhotoGallery.Core.EmailUtils;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.Filters;
using BinaryStudio.PhotoGallery.Web.Properties;
using BinaryStudio.PhotoGallery.Web.ViewModels.Admin;

namespace BinaryStudio.PhotoGallery.Web.Area.Api
{
    [AdminAuthorize]
    [RoutePrefix("api/admin")]
    public class AdminApiController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;

        public AdminApiController(IUserService userService, IEmailSender emailSender)
        {
            _userService = userService;
            _emailSender = emailSender;
        }

        [POST("invite")]
        public HttpResponseMessage PostSendInvite([FromBody] InviteUserViewModel viewModel)
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
                    "Dear, {0} {1}!\n\nYou have been invited to the great photogallery " +
                    "project of Binary Studio! For the end of registration, please click on this link:\n{2}",
                    viewModel.FirstName,
                    viewModel.LastName,
                    activationLink);

                _emailSender.Send(host, fromEmail, fromPass, toEmail, mailSubject, text);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [POST("block")]
        public HttpResponseMessage PostBlockAccount([FromBody] int userId)
        {
            try
            {
                _userService.BlockUser(userId);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [POST("unblock")]
        public HttpResponseMessage PostUnblockAccount([FromBody] int userId)
        {
            try
            {
                _userService.UnblockUser(userId);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [DELETE("delete/{userId:int}")]
        public HttpResponseMessage Delete([FromUri] int userId)
        {
            try
            {
                _userService.DeleteUser(userId);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}