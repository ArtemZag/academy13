using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.PhotoPage;
using BinaryStudio.PhotoGallery.Web.ViewModels.Search;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    public interface IModelConverter
    {
        UserModel GetModel(RegistrationViewModel registrationViewModel);

        UserModel GetModel(AuthorizationViewModel authorizationViewModel);

        SearchedUserViewModel GetViewModel(UserModel userModel);

        PhotoCommentViewModel GetViewModel(PhotoCommentModel photoCommentModel, UserModel userModel);

        PhotoViewModel GetViewModel(PhotoModel photoModel);

        AlbumViewModel GetViewModel(AlbumModel model);
    }
}
