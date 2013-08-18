using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.Authorization;
using BinaryStudio.PhotoGallery.Web.ViewModels.PhotoPage;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    public interface IModelConverter
    {
        UserModel GetModel(SignupViewModel registrationViewModel);

        UserModel GetModel(SigninViewModel authorizationViewModel);

        PhotoModel GetPhotoModel(int userId, int albumId, string realFileFormat);

        PhotoCommentViewModel GetViewModel(PhotoCommentModel photoCommentModel, UserModel userModel);

        PhotoViewModel GetViewModel(PhotoModel photoModel);

        UserViewModel GetViewModel(UserModel userModel);
    }
}
