using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Web.App_Start;
using Microsoft.Practices.Unity;
using PerpetuumSoft.Knockout;

namespace BinaryStudio.PhotoGallery.Web
{
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

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            var principal = HttpContext.Current.User = new CustomPrincipal(-1, "", false);

            if (authCookie == null)
            {
                HttpContext.Current.User = principal;
                return;
            }
            
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            if (authTicket == null)
            {
                HttpContext.Current.User = principal;
                return;
            }

            var serializer = new JavaScriptSerializer();

            var model = serializer.Deserialize<UserInfoSerializeModel>(authTicket.UserData);

            principal = new CustomPrincipal(model.Id, model.Email, model.IsAdmin);
                    
            HttpContext.Current.User = principal;
        }
    }
}