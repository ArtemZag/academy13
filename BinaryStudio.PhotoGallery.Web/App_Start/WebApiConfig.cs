using System.Web.Http;
using BinaryStudio.PhotoGallery.Web.Filters;

namespace BinaryStudio.PhotoGallery.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("DefaultApiWithAction", "api/{controller}/{action}");
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { Id = RouteParameter.Optional });
            config.Filters.Add(new ValidateModelAttribute());
        }
    }
}
