using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Domain.Services;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class BaseViewModel
    {
        public IUrlUtil UrlUtil { get; private set; }
        public IPathUtil PathUtil { get; private set; }

        protected BaseViewModel()
        {
            UrlUtil = Bootstrapper.Resolve<IUrlUtil>();
            PathUtil = Bootstrapper.Resolve<IPathUtil>();
        }
    }
}