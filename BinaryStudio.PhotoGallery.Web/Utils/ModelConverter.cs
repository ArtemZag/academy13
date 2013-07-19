using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    public static class ModelConverter
    {
        public static UserModel ToModel(RegistrationViewModel viewModel)
        {
            var userModel = new UserModel(viewModel.Nickname, viewModel.FirstName, viewModel.LastName);
            var authInfo = new AuthInfoModel(viewModel.AuthName, viewModel.Email, viewModel.Password);

            userModel.Authinfos.Add(authInfo);

            return userModel;
        }
    }
}