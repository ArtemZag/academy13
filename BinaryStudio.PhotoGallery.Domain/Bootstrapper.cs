using BinaryStudio.PhotoGallery.Domain.Services;
using Microsoft.Practices.Unity;

namespace BinaryStudio.PhotoGallery.Domain
{
    public static class Bootstrapper
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IUserService, UserService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAlbumService, AlbumService>();
            container.RegisterType<IPhotoService, PhotoService>();
            container.RegisterType<IPhotoCleanupTask, PhotoCleaupTask>();
        }
    }
}