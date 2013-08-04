using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    public interface IModelConverter
    {
        UserModel GetModel(RegistrationViewModel viewModel);

        UserModel GetModel(AuthorizationViewModel viewModel);

        PhotoViewModel TestGetViewModel(PhotoModel viewModel);
    }
}