using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Domain.Services;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class BaseViewModel
    {
        public IUrlUtil UrlUtil { get; private set; }
        public IPathUtil PathUtil { get; private set; }
        public IAlbumService AlbumService { get; private set; }
        public IPhotoService PhotoService { get; private set; }

        protected BaseViewModel()
        {
            UrlUtil = Bootstrapper.Resolve<IUrlUtil>();
            PathUtil = Bootstrapper.Resolve<IPathUtil>();
            AlbumService = Bootstrapper.Resolve<IAlbumService>();
            PhotoService = Bootstrapper.Resolve<IPhotoService>();
        }
    }
}