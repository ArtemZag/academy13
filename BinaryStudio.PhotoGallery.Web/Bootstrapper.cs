using Microsoft.Practices.Unity;
using System.Web.Http;
using System.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new Unity.Mvc4.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            RegisterTypes(container);

            return container;
        }

        private static void RegisterTypes(IUnityContainer container)
        {
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            // e.g. container.RegisterType<ITestService, TestService>();    
            Domain.Bootstrapper.RegisterTypes(container);
            Database.Bootstrapper.RegisterTypes(container);
            Core.Bootstrapper.RegisterTypes(container);
        }
    }
}
