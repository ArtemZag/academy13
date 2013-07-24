using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;

namespace BinaryStudio.PhotoGallery.Web
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

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