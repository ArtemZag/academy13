using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Web.App_Start;
using Microsoft.Practices.Unity;
using PerpetuumSoft.Knockout;

namespace BinaryStudio.PhotoGallery.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ModelBinders.Binders.DefaultBinder = new KnockoutModelBinder();
            AreaRegistration.RegisterAllAreas();

            BootstrapBundleConfig.RegisterBundles();

            ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            IUnityContainer container = Bootstrapper.Initialise();
            System.Data.Entity.Database.SetInitializer(new DatabaseInitializer());

            // todo
            // TaskManager.Initialize(new CleanupRegistry(container.Resolve<ICleanupTask>()));
            // TaskManager.Initialize(new UsersMonitorRegistry(container.Resolve<IUsersMonitorTask>()));
            // TaskManager.Initialize(new SearchCacheRegistry(container.Resolve<ISearchCacheTask>()));
        }
    }
}