using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using BinaryStudio.PhotoGallery.Database;

namespace BinaryStudio.PhotoGallery.Web
{
    using BinaryStudio.PhotoGallery.Web.App_Start;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            BootstrapBundleConfig.RegisterBundles();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            AttributeRoutingConfig.Start();

            Bootstrapper.Initialise();
            System.Data.Entity.Database.SetInitializer(new DatabaseInitializer());

            //delete
            // BinaryStudio.PhotoGallery.Database.Bootstrapper.Test();
        }
    }
}