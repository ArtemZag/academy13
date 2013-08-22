using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.Events;
using BinaryStudio.PhotoGallery.Web.Hubs;
using BinaryStudio.PhotoGallery.Web.Utils;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;
using Unity.Mvc4;

namespace BinaryStudio.PhotoGallery.Web
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            IUnityContainer container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

            GlobalHost.DependencyResolver = new SignalRUnityDependencyResolver(container);

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

            container.RegisterType<ISearchModelConverter, SearchModelConverter>();

            Domain.Bootstrapper.RegisterTypes(container);
            Database.Bootstrapper.RegisterTypes(container);
            Core.Bootstrapper.RegisterTypes(container);

            container.RegisterType<IGlobalEventsAggregator, GlobalEventsAggregator>(new ContainerControlledLifetimeManager());
            container.RegisterType<IGlobalEventsHandler, GlobalEventsHandler>(new ContainerControlledLifetimeManager());
            container.RegisterType<NotificationsHub>(new InjectionFactory(CreateNotificationsHub));
            container.RegisterInstance(container.Resolve<IGlobalEventsHandler>());
        }

        private static object CreateNotificationsHub(IUnityContainer p)
        {
            var myHub = new NotificationsHub(p.Resolve<IUserService>(), p.Resolve<IPhotoService>());
            return myHub;
        }
    }

    public class SignalRUnityDependencyResolver : DefaultDependencyResolver
    {
        private readonly IUnityContainer _container;

        public SignalRUnityDependencyResolver(IUnityContainer container)
        {
            _container = container;
        }

        public override object GetService(Type serviceType)
        {
            if (_container.IsRegistered(serviceType)) return _container.Resolve(serviceType);
            return base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            if (_container.IsRegistered(serviceType)) return _container.ResolveAll(serviceType);
            return base.GetServices(serviceType);
        }
    }
}