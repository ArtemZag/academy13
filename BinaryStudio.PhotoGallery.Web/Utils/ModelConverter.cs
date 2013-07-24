using System.Collections.ObjectModel;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    public static class ModelConverter
    {
        public static UserModel GetModel(RegistrationViewModel viewModel, string authProvider)
        {
            var userModel = new UserModel
                {
                    Email = viewModel.Email,
                    UserPassword = viewModel.Password
                };

            if (authProvider != AuthInfoModel.LOCAL_PROFILE)
            {
                userModel.AuthInfos = new Collection<AuthInfoModel> {new AuthInfoModel(-1, authProvider)};
            }

            return userModel;
        }

        public static UserModel GetModel(AuthInfoViewModel viewModel, string authProvider)
        {
            var userModel = new UserModel
            {
                Email = viewModel.Email,
                UserPassword = viewModel.Password
            };

            if (authProvider != AuthInfoModel.LOCAL_PROFILE)
            {
                userModel.AuthInfos = new Collection<AuthInfoModel> { new AuthInfoModel(-1, authProvider) };
            }

            return userModel;
        }

        public static PhotoViewModel GetViewModel(PhotoModel viewModel)
        {
            var photoModel = new PhotoViewModel {PhotoThumbSource = viewModel.PhotoThumbSource};

            return photoModel;
        }
    }
}