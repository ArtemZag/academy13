using BinaryStudio.PhotoGallery.Domain.Services.Search;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.PhotoPage;
using BinaryStudio.PhotoGallery.Web.ViewModels.Search;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    public interface IModelConverter
    {
        SearchArguments GetModel(SearchViewModel searchViewModel, int userId);

        IFoundViewModel GetViewModel(IFound found);

        PhotoModel GetPhotoModel(int userId, int albumId, string realFileFormat);

        PhotoCommentViewModel GetViewModel(PhotoCommentModel photoCommentModel, UserModel userModel);

        PhotoViewModel GetViewModel(PhotoModel photoModel);

        UserViewModel GetViewModel(UserModel userModel);
    }
}
