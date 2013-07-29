using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Tests.Mocked;
using Microsoft.Practices.Unity;

namespace BinaryStudio.PhotoGallery.Domain.Tests
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = new UnityContainer();
            RegisterTypes(container);

            return container;
        }

        private static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IUnitOfWorkFactory, TestUnitOfWorkFactory>();

            Domain.Bootstrapper.RegisterTypes(container);

            Core.Bootstrapper.RegisterTypes(container);
        }
    }
}