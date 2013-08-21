using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.App_Start;
using Microsoft.Practices.Unity;
using PerpetuumSoft.Knockout;

namespace BinaryStudio.PhotoGallery.Web
{
    public class MvcApplication : HttpApplication
    {
        private readonly IUserService _userService;

        public MvcApplication(IUserService userService)
        {
            _userService = userService;
        }

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

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            
        }
    }
}