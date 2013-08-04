using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    internal class ModelConverter : IModelConverter
    {
        private readonly IPathUtil pathUtil;

        public ModelConverter(IPathUtil pathUtil)
        {
            this.pathUtil = pathUtil;
        }

        public UserModel GetModel(RegistrationViewModel viewModel)
        {
            var userModel = new UserModel
            {
                Email = viewModel.Email,
                UserPassword = viewModel.Password
            };

            return userModel;
        }

        public UserModel GetModel(AuthorizationViewModel viewModel)
        {
            var userModel = new UserModel
            {
                Email = viewModel.Email,
                UserPassword = viewModel.Password
            };

            return userModel;
        }

        public PhotoViewModel TestGetViewModel(PhotoModel viewModel)
        {
            var photoModel = new PhotoViewModel
            {
                PhotoThumbSource = pathUtil.BuildThumbnailsPath(viewModel.UserModelId, viewModel.AlbumModelId)
                                    + @"\" + viewModel.PhotoName,
                PhotoSource = pathUtil.BuildAlbumPath(viewModel.UserModelId, viewModel.AlbumModelId)
                                    + @"\" + viewModel.PhotoName,
                AlbumId = viewModel.AlbumModelId,
                PhotoId = viewModel.Id
            };

            return photoModel;
        }
    }
}