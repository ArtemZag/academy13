using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class AvailableGroupViewModel
    {
        public bool CanSeePhotos { get; set; }

        public bool CanSeeComments { get; set; }

        public string Name { get; set; }

        public int GroupId { get; set; }

        public static AvailableGroupModel ToModel(AvailableGroupViewModel viewModel)
        {
            return new AvailableGroupModel
            {
                GroupId = viewModel.GroupId,
                CanSeePhotos = viewModel.CanSeePhotos,
                CanSeeComments = viewModel.CanSeeComments
            };
        }
    }
}