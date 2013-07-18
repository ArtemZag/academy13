using BinaryStudio.PhotoGallery.Domain.Services;
using Microsoft.Practices.Unity;

namespace BinaryStudio.PhotoGallery.Domain
{
    public static class Bootstrapper
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType(typeof (IUserService), typeof (TestService));
        }
    }
}