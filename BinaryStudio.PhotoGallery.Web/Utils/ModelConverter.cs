using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    public static class ModelConverter
    {
        public static UserModel ToModel(RegistrationViewModel viewModel)
        {
            var userModel = new UserModel()
                {
                    Email = viewModel.Email
                };

            var authInfo = new AuthInfoModel()
                {
                    AuthName = viewModel.AuthProvider,
                    UserPassword = viewModel.Password,
                };

            userModel.Authinfos.Add(authInfo);

            return userModel;
        }
    }
}