using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain;

namespace BinaryStudio.PhotoGallery.Web
{
    using BinaryStudio.PhotoGallery.Web.App_Start;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
      ModelBinders.Binders.DefaultBinder = new PerpetuumSoft.Knockout.KnockoutModelBinder();
            AreaRegistration.RegisterAllAreas();

            BootstrapBundleConfig.RegisterBundles();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            AttributeRoutingConfig.Start();

            Bootstrapper.Initialise();
            System.Data.Entity.Database.SetInitializer(new DatabaseInitializer());

            // todo: delete
             Database.Bootstrapper.Test();
        }
    }
}