using System.Web.Http;
using BinaryStudio.PhotoGallery.Web.Filters;

namespace BinaryStudio.PhotoGallery.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("DefaultApiWithAction", "Api/{controller}/{action}");
            config.Routes.MapHttpRoute(
                name: "DefaultApiWithID",
                routeTemplate: "api/{controller}/{id}",
                defaults: null
            );

            //config.Filters.Add(new ValidateModelAttribute());
        }
    }
}
