using System.Web.Routing;

namespace BinaryStudio.PhotoGallery.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapHubs();
        }
    }
}