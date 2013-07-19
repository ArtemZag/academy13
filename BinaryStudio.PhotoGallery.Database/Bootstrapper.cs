using Microsoft.Practices.Unity;

namespace BinaryStudio.PhotoGallery.Database
{
    public static class Bootstrapper
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            // todo: register repositories
            container.RegisterInstance(typeof (IUnitOfWorkFactory), "IUnitOfWorkFactory", UnitOfWorkFactory.Instance);
        }
    }
}
