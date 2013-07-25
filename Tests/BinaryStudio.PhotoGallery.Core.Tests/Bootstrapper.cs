using Microsoft.Practices.Unity;

namespace BinaryStudio.PhotoGalery.Core.Tests
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
            PhotoGallery.Core.Bootstrapper.RegisterTypes(container);
        }
    }
}