using System.Web.Http;
using System.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain;
using BinaryStudio.PhotoGallery.Web.Events;
using BinaryStudio.PhotoGallery.Web.Utils;
using Microsoft.Practices.Unity;
using Unity.Mvc4;

namespace BinaryStudio.PhotoGallery.Web
{
    public static class Bootstrapper
    {
        private static IUnityContainer _container;

        public static IUnityContainer Initialise()
        {
            IUnityContainer container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
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

            container.RegisterType<IGlobalEventsAggregator, GlobalEventsAggregator>(new ContainerControlledLifetimeManager());
            container.RegisterType<INotificationsEventManager, NotificationsEventManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<IGlobalEventsHandler, GlobalEventsHandler>(new ContainerControlledLifetimeManager());
            container.RegisterInstance(container.Resolve<IGlobalEventsHandler>());

            container.RegisterType<ISearchModelConverter, SearchModelConverter>();
        }

        public static T Resolve<T>()
        {
            return getContainer().Resolve<T>();
        }

        private static IUnityContainer getContainer()
        {
            return _container ?? (_container = Initialise());
        }
    }

}