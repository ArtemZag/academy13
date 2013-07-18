using System.Web.Http;
using BinaryStudio.PhotoGallery.Web.Filters;

namespace BinaryStudio.PhotoGallery.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new ValidateModelAttribute());
        }
    }
}
