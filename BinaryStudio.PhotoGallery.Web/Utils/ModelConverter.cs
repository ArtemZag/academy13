using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.PhotoPage;
using BinaryStudio.PhotoGallery.Web.ViewModels.Search;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    internal class ModelConverter : IModelConverter
    {
        private readonly IPathUtil _pathUtil;

        public ModelConverter(IPathUtil pathUtil)
        {
            this._pathUtil = pathUtil;
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

        public SearchedUserViewModel GetViewModel(UserModel model)
        {
            throw new System.NotImplementedException();
        }

        public PhotoViewModel GetViewModel(PhotoModel viewModel)
        {
            var photoModel = new PhotoViewModel
            {
                PhotoThumbSource = _pathUtil.BuildThumbnailsPath(viewModel.UserModelId, viewModel.AlbumModelId)
                                    + @"\" + viewModel.PhotoName,
                PhotoSource = _pathUtil.BuildAlbumPath(viewModel.UserModelId, viewModel.AlbumModelId)
                                    + @"\" + viewModel.PhotoName,
                AlbumId = viewModel.AlbumModelId,
                PhotoId = viewModel.Id
            };

            return photoModel;
        }

        public PhotoCommentViewModel GetViewModel(PhotoCommentModel photoCommentModel, UserModel userModel)
        {
            return new PhotoCommentViewModel
                {
                    UserInfo = new UserInfoViewModel
                        {
                            OwnerFirstName = userModel.FirstName,
                            OwnerLastName = userModel.LastName
                        },
                        Rating   = photoCommentModel.Rating,
                        DateOfCreating = photoCommentModel.DateOfCreating,

                        // this shit needs fixing
                        Reply = photoCommentModel.Reply,
                        Text = photoCommentModel.Text
                };
        }
    }
}