using BinaryStudio.PhotoGallery.Core.EmailUtils;
using BinaryStudio.PhotoGallery.Core.Helpers;
using BinaryStudio.PhotoGallery.Core.IOUtils;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Core.PhotoUtils;
using BinaryStudio.PhotoGallery.Core.UserUtils;
using Microsoft.Practices.Unity;

namespace BinaryStudio.PhotoGallery.Core
{
    public static class Bootstrapper
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IEmailSender, EmailSender>();
            container.RegisterType<ICryptoProvider, CryptoProvider>();
            container.RegisterType<IPathUtil, PathUtil>();
            container.RegisterType<IFileHelper, FileHelper>();
            container.RegisterType<IMultipartFormDataStreamProviderWrapper, MultipartFormDataStreamProviderWrapper>();
            container.RegisterType<IFileWrapper, FileWrapper>();
            container.RegisterType<IDirectoryWrapper, DirectoryWrapper>();
            container.RegisterType<IUrlUtil, UrlUtil>();
            container.RegisterType<IPhotoProcessor, PhotoProcessor>();
            container.RegisterType<ICollageProcessor, CollageProcessor>();
        }
    }
}
