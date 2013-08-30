using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Extensions.ViewModels
{
    public static class AvailableGroupConvertationExtension
    {
        public static AvailableGroupViewModel ToAvialableGroupViewModel(this AvailableGroupModel groupViewModel, string name)
        {
            return new AvailableGroupViewModel
            {
                Name = name,
                GroupId = groupViewModel.GroupId,
                CanSeePhotos = groupViewModel.CanSeePhotos,
                CanSeeComments = groupViewModel.CanSeeComments
            };
        }
    }
}