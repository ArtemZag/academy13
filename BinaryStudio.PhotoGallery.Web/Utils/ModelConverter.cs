using System;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.PhotoPage;
using BinaryStudio.PhotoGallery.Web.ViewModels.Search;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    internal class ModelConverter : IModelConverter
    {
        private readonly IPathUtil pathUtil;

        public ModelConverter(IPathUtil pathUtil)
        {
            this.pathUtil = pathUtil;
        }

        public UserModel GetModel(RegistrationViewModel registrationViewModel)
        {
            var userModel = new UserModel
                {
                    Email = registrationViewModel.Email,
                    UserPassword = registrationViewModel.Password
                };

            return userModel;
        }

        public UserModel GetModel(AuthorizationViewModel authorizationViewModel)
        {
            var userModel = new UserModel
                {
                    Email = authorizationViewModel.Email,
                    UserPassword = authorizationViewModel.Password
                };

            return userModel;
        }

        public SearchedUserViewModel GetViewModel(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        public PhotoViewModel GetViewModel(PhotoModel photoModel)
        {
            var viewModel = new PhotoViewModel
                {
                    // todo: UserModelId in photoModel != userId which album contain this photo
                    // is PhotoSource necessary in PhotoViewModel?!
                    PhotoSource =
                        pathUtil.BuildOriginalPhotoPath(photoModel.UserModelId, photoModel.AlbumModelId, photoModel.Id,
                                                         photoModel.Format),

                    PhotoThumbSource = pathUtil.BuildThumbnailsPath(photoModel.UserModelId, photoModel.AlbumModelId)
                                       + @"\" + photoModel.PhotoName + photoModel.Format,

                    AlbumId = photoModel.AlbumModelId,
                    PhotoId = photoModel.Id
                };

            return viewModel;
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
                    Rating = photoCommentModel.Rating,
                    DateOfCreating = photoCommentModel.DateOfCreating,

                    // this shit needs fixing
                    Reply = photoCommentModel.Reply,
                    Text = photoCommentModel.Text
                };
        }
    }
}