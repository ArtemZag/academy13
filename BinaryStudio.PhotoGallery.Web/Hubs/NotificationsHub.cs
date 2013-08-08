using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

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

        public void PhotoAdded()
        {
            User user;
            Users.TryGetValue(Context.User.Identity.Name, out user);
            if (user != null)
            {
                Clients.AllExcept(user.ConnectionIds.ToArray())
                       .broadcastNotification(NotificationTitles.PhotoAdded, Context.User.Identity.Name, "photoname");
            }

        }

        public override Task OnConnected()
        {
            string userEmail = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;

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
        
    }
}