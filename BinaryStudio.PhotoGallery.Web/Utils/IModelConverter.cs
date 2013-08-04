using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.PhotoPage;
using BinaryStudio.PhotoGallery.Web.ViewModels.Search;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    public interface IModelConverter
    {
        UserModel GetModel(RegistrationViewModel viewModel);

        UserModel GetModel(AuthorizationViewModel viewModel);

        SearchedUserViewModel GetViewModel(UserModel model);

        PhotoViewModel GetViewModel(PhotoModel photoModel);

        PhotoCommentViewModel GetViewModel(PhotoCommentModel photoCommentModel, UserModel userModel);

        PhotoViewModel TestGetViewModel(PhotoModel viewModel);
    }
}
