using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Web.ViewModels.Photo
{
    public class PhotoLikeViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public static PhotoLikeViewModel FromModel(UserModel userModel)
        {
            var viewModel = new PhotoLikeViewModel
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName
            };

            return viewModel;
        }
    }
}