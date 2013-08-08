using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.PhotoPage;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    public interface IModelConverter
    {
        UserModel GetModel(RegistrationViewModel registrationViewModel);

        UserModel GetModel(AuthorizationViewModel authorizationViewModel);

        PhotoCommentViewModel GetViewModel(PhotoCommentModel photoCommentModel, UserModel userModel);

        PhotoViewModel GetViewModel(PhotoModel photoModel);
    }
}
