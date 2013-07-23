using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Core.UserUtils;
using Microsoft.Practices.Unity;

namespace BinaryStudio.PhotoGallery.Domain
{
    public static class Bootstrapper
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType(typeof(IUserService), typeof(UserService));
            container.RegisterType(typeof(IAlbumService), typeof(AlbumService));
            container.RegisterType(typeof(IPhotoService), typeof(PhotoService));
            container.RegisterType(typeof(ICryptoProvider), typeof(CryptoProvider));
        }
    }
}