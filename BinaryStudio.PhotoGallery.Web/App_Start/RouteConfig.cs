using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHubs();
        }
    }
}