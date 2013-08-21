using System.Web.Http;
using BinaryStudio.PhotoGallery.Web.Filters;

namespace BinaryStudio.PhotoGallery.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new ValidateModelAttribute());
        }
    }
}