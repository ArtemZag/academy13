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
using Microsoft.AspNet.SignalR.Hubs;

namespace BinaryStudio.PhotoGallery.Web.Hubs
{
    public class User
    {
        public string Name { get; set; }
        public HashSet<string> ConnectionIds { get; set; }
    }


    public interface INotificationsHub
    {
        void CreateNotification(string title, string text, string url);
    }

    [Authorize]
    [HubName("NotificationsHub")]
    public class NotificationsHub : Hub, INotificationsHub
    {
        private static readonly ConcurrentDictionary<string, User> Users
        = new ConcurrentDictionary<string, User>();

        private readonly IPhotoService _photoService;
        private readonly IUserService _userService;

        public NotificationsHub(){}

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

            var message = "Пользователь " + uModel.FirstName+ " "+ uModel.LastName + " добавил фотографию " + photoName;
            if (user != null)
            {
                Clients.All.broadcastNotification(NotificationTitles.PhotoAdded, message);
            }

        }

        public void CreateNotification(string title, string text, string url)
        {
            //User user;
            //Users.TryGetValue(Context.User.Identity.Name, out user);
        }

        public override Task OnConnected()
        {
            string userEmail = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;

            Groups.Add(connectionId, userEmail);

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
            string userEmail = Context.User.Identity.Name; // null reference exception, when I closed all tabs with site
            string connectionId = Context.ConnectionId;

            Groups.Remove(connectionId, userEmail);

            User user;
            Users.TryGetValue(userEmail, out user);

            if (user != null)
                lock (user.ConnectionIds)
                {
                    user.ConnectionIds.Remove(connectionId);
                }

            return base.OnDisconnected();
        }
        
    }
}