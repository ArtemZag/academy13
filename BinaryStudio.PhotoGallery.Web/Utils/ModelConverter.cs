using System.Collections.ObjectModel;
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
                    Email = viewModel.Email
                };

            var authInfo = new AuthInfoModel
                {
                    AuthProvider = viewModel.AuthProvider,
                    UserPassword = viewModel.Password,
                };

            userModel.AuthInfos = new Collection<AuthInfoModel> { authInfo };

            return userModel;
        }

        public static UserModel GetModel(AuthInfoViewModel viewModel)
        {
            var userModel = new UserModel {Email = viewModel.Email};

            var authInfo = new AuthInfoModel
                {
                    AuthProvider = viewModel.AuthProvider,
                    UserPassword = viewModel.Password,
                };
            
            userModel.AuthInfos = new Collection<AuthInfoModel> { authInfo };

            return userModel;
        }
    }
}