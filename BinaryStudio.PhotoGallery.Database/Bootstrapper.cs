using Microsoft.Practices.Unity;

namespace BinaryStudio.PhotoGallery.Database
{
    public static class Bootstrapper
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            // todo: register repositories
            container.RegisterType<IUnitOfWorkFactory, UnitOfWorkFactory>(new ContainerControlledLifetimeManager());

//            container.RegisterInstance(typeof (IUnitOfWorkFactory), "IUnitOfWorkFactory", UnitOfWorkFactory.Instance);
        }
    }
}
