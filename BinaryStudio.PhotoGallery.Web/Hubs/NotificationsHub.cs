using System.Threading.Tasks;
using System.Web;
using BinaryStudio.PhotoGallery.Web.CustomStructure;
using Microsoft.AspNet.SignalR;
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