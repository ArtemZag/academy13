using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Domain.Utils;
using Microsoft.Practices.Unity;

namespace BinaryStudio.PhotoGallery.Domain
{
    public static class Bootstrapper
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IUserService, UserService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAlbumService, AlbumService>();
            container.RegisterType<IPhotoService, PhotoService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IPhotoCommentService, PhotoCommentService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICleanupTask, CleanupTask>();
            container.RegisterType<IStorage, Storage>();
        }
    }
}