using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Core.EmailUtils;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Domain.Services.Tasks;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Filters;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [AdminAuthorize]
    [RoutePrefix("admin")]
    public class AdminPanelController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IUserService _userService;
        private readonly IUsersMonitorTask _usersMonitorTask;

        public AdminPanelController(
            IUserService userService,
            IUsersMonitorTask usersMonitorTask,
            IEmailSender emailSender)
        {
            _userService = userService;
            _usersMonitorTask = usersMonitorTask;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     Administration page
        /// </summary>
        [GET("")]
        public ActionResult Index()
        {
            List<UserModel> users = _userService.GetAllUsers().ToList();

            // todo: it's ModelConverter role
            List<UserViewModel> extendedUserList = users.Select(user => new UserViewModel
            {
                IsOnline = _usersMonitorTask.IsOnline(user.Email),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                AlbumsCount = user.Albums.Count
            }).ToList();

            return View(new UsersListViewModel {UserViewModels = extendedUserList});
        }

        [DELETE("user/delete")]
        public HttpResponseMessage DeleteUser(string userEmail)
        {
            _userService.DeleteUser(userEmail);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [POST("user/invite")]
        public HttpResponseMessage SendInvite(UserViewModel invitedUser)
        {
            string host = ConfigurationManager.AppSettings["NotificationHost"];
            string fromEmail = ConfigurationManager.AppSettings["NotificationEmail"];
            string fromPass = ConfigurationManager.AppSettings["NotificationPassword"];

            string toEmail = invitedUser.Email;

            string activationLink = "http://localhost:57367/Authorization/Signup/" +
                                    _userService.CreateUser(invitedUser.Email, invitedUser.FirstName,
                                        invitedUser.LastName);

            const string SUBJECT = "Binary Studio gallery invitation";

            string text = string.Format("Dear {0} {1}!\n\nYou have been invited to the great photogallery project" +
                                        " of Binary Studio! For the end of registration, please click on this link:\n{2} ",
                invitedUser.FirstName, invitedUser.LastName, activationLink);

            _emailSender.Send(host, fromEmail, fromPass, toEmail, SUBJECT, text);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}