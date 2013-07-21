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
            Domain.Bootstrapper.RegisterTypes(container);
            Database.Bootstrapper.RegisterTypes(container);
        }
    }
}