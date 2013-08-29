using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Services.Tasks;
using BinaryStudio.PhotoGallery.Web.App_Start;
using BinaryStudio.PhotoGallery.Web.CustomStructure;
using BinaryStudio.PhotoGallery.Web.Extensions;
using BinaryStudio.PhotoGallery.Web.Registers;
using FluentScheduler;
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

            System.Data.Entity.Database.SetInitializer(new DatabaseInitializer());

            IUnityContainer container = Bootstrapper.Initialise();

            TaskManager.Initialize(new CleanupRegistry(container.Resolve<ICleanupTask>()));
            TaskManager.Initialize(new SearchCacheRegistry(container.Resolve<ISearchCacheTask>()));

            // todo
//            TaskManager.Initialize(new UsersMonitorRegistry(container.Resolve<IUsersMonitorTask>()));
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

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var httpException = exception as HttpException;
            string actionName;

            if (httpException != null)
            {
                switch (httpException.GetHttpCode())
                {
                    case 500:
                        actionName = "HttpError500";
                        break;
                    case 404:
                        actionName = "NotFound";
                        break;
                    case 403:
                        actionName = "AccessDenied";
                        break;
                    default:
                        actionName = "Error";
                        break;
                }
            }
            else
            {
                actionName = "Error";
            }

            Server.ClearError();
            exception.Render(actionName, Context);
        }
    }
}