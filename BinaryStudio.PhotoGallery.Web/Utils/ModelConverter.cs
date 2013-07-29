using BinaryStudio.PhotoGallery.Core.Helpers;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    public static class ModelConverter
    {
        public static UserModel GetModel(RegistrationViewModel viewModel)
        {
            var userModel = new UserModel
                {
                    Email = viewModel.Email,
                    UserPassword = viewModel.Password
                };

            return userModel;
        }

        public static UserModel GetModel(AuthorizationViewModel viewModel)
        {
            var userModel = new UserModel
                {
                    Email = viewModel.Email,
                    UserPassword = viewModel.Password
                };

            return userModel;
        }

        public static PhotoViewModel GetViewModel(PhotoModel viewModel)
        {
            var photoModel = new PhotoViewModel 
            { 
                PhotoThumbSource = PathHelper.BuildThumbnailsPath(viewModel.UserModelId, viewModel.AlbumModelId)
                                    +"/"+viewModel.PhotoName,
                PhotoSource = PathHelper.BuildAlbumPath(viewModel.UserModelId, viewModel.AlbumModelId)
                                    +"/"+viewModel.PhotoName,
                AlbumId = viewModel.AlbumModelId,
                PhotoId = viewModel.Id
            };

            return photoModel;
        }
    }
}