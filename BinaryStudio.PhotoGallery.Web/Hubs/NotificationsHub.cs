using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using BinaryStudio.PhotoGallery.Web.CustomStructure;
using Microsoft.AspNet.SignalR;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using Microsoft.AspNet.SignalR.Hubs;

namespace BinaryStudio.PhotoGallery.Web.Hubs
{
    [Authorize]
    [HubName("NotificationsHub")]
    public class NotificationsHub : Hub, ICustomPrincipalInHub
    {
        public CustomPrincipal User
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }

        public override Task OnConnected()
        {
            Groups.Add(Context.ConnectionId, User.Id.ToString("d"));
            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            Groups.Remove(Context.ConnectionId, User.Id.ToString("d"));
            return base.OnDisconnected();
        }
        
    }
}