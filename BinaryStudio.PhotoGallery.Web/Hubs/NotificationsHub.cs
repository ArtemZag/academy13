using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;

namespace BinaryStudio.PhotoGallery.Web.Hubs
{
    public class User
    {
        public string Name { get; set; }
        public HashSet<string> ConnectionIds { get; set; }
    }

    [Authorize]
    public class NotificationsHub : Hub
    {
        private static readonly ConcurrentDictionary<string, User> Users
        = new ConcurrentDictionary<string, User>();

        private readonly IPhotoService _photoService;
        private readonly IUserService _userService;

        public NotificationsHub(IUserService userService, IPhotoService photoService)
        {
            _photoService = photoService;
            _userService = userService;
        }

        public void PhotoAdded(string photoName)
        {
            User user;
            Users.TryGetValue(Context.User.Identity.Name, out user);
            var uModel = _userService.GetUser(Context.User.Identity.Name);

            if (user != null)
            {
                Clients.AllExcept(user.ConnectionIds.ToArray())
                       .broadcastNotification(
                            NotificationTitles.PhotoAdded,
                            String.Format("{0} {1}", uModel.FirstName, uModel.LastName),
                            photoName
                       );
            }

        }

        public void PhotoAddedHandler(string photoName, int userId)
        {
            var uModelHeAdded = _userService.GetUser(userId);

            Clients.Group(Context.User.Identity.Name).broadcastNotification(
                            NotificationTitles.PhotoAdded,
                            String.Format("{0} {1}", uModelHeAdded.FirstName, uModelHeAdded.LastName),
                            photoName
                       );
        }

        public void CommentAddedHandler(string userName, string photoName)
        {

        }

        public override Task OnConnected()
        {
            string userEmail = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;

            Groups.Add(Context.ConnectionId, Context.User.Identity.Name);

            var user = Users.GetOrAdd(userEmail, new User
            {
                Name = userEmail,
                ConnectionIds = new HashSet<string>()
            });

            lock (user.ConnectionIds)
            {
                user.ConnectionIds.Add(connectionId);
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            string userEmail = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;
            User signalRUser;
            Users.TryGetValue(userEmail, out signalRUser);
            Groups.Remove(Context.ConnectionId, Context.User.Identity.Name);

            if (signalRUser != null)
                lock (signalRUser.ConnectionIds)
                {
                    signalRUser.ConnectionIds.Remove(connectionId);
                }

            return base.OnDisconnected();
        }

    }
}