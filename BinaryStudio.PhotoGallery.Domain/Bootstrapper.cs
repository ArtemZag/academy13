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
            container.RegisterType<IAlbumService, AlbumService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IPhotoService, PhotoService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICleanupTask, CleanupTask>(new ContainerControlledLifetimeManager());
            container.RegisterType<IStorage, Storage>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUsersMonitorTask, UsersMonitorTask>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISearchService, SearchService>(new ContainerControlledLifetimeManager());
        }
    }
}