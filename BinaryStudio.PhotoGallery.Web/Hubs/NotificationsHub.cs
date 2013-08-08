using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace BinaryStudio.PhotoGallery.Web.Hubs
{
    public class NotificationsHub : Hub
    {
        public void Send(string title, string message)
        {
            Clients.All.broadcastNotification(title, message);
        }
    }
}