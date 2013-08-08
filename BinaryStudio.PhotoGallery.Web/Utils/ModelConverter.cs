using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Domain.Services.Search;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.PhotoPage;
using BinaryStudio.PhotoGallery.Web.ViewModels.Search;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    internal class ModelConverter : IModelConverter
    {
        private readonly IAlbumService albumService;
        private readonly IPathUtil pathUtil;

        public ModelConverter(IPathUtil pathUtil, IAlbumService albumService)
        {
            this.pathUtil = pathUtil;
            this.albumService = albumService;
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

        public SearchArguments GetModel(SearchViewModel searchViewModel)
        {
            return new SearchArguments
                {
                    Begin = searchViewModel.Begin,
                    End = searchViewModel.End,

                    IsSearchPhotosByName = searchViewModel.IsSearchPhotosByName,
                    IsSearchPhotosByTags = searchViewModel.IsSearchPhotosByTags,
                    IsSearchPhotosByDescription = searchViewModel.IsSearchPhotosByDescription,

                    IsSearchAlbumsByName = searchViewModel.IsSearchAlbumsByName,
                    IsSearchAlbumsByTags = searchViewModel.IsSearchAlbumsByTags,
                    IsSearchAlbumsByDescription = searchViewModel.IsSearchAlbumsByDescription,

                    IsSearchUsersByName = searchViewModel.IsSearchUsersByName,
                    IsSearchUserByDepartment = searchViewModel.IsSearchUserByDepartment,

                    IsSearchByComments = searchViewModel.IsSearchByComments
                };
        }


        public PhotoViewModel GetViewModel(PhotoModel photoModel)
        {
            // We need to grab photos from album's owner, not from photo's creator.
            // So needs to take an userID via albumModel.UserModelID
            AlbumModel albumModel = albumService.GetAlbumByID(photoModel.AlbumModelId);

            var viewModel = new PhotoViewModel
                {
                    // todo: UserModelId in photoModel != userId which album contain this photo
                    // is PhotoSource necessary in PhotoViewModel?!

                    // Maaak: UserModelID is an userID, who has created(added) this photo. 
                    //        So it can be as equel to userId, which album contain this photo, so not.
                    //        Look at PhotoModel to check the meaning of property UserModelID
                    PhotoSource =
                        pathUtil.BuildOriginalPhotoPath(albumModel.UserModelId, photoModel.AlbumModelId,
                                                        photoModel.Id, photoModel.Format),

                    // Maaak: I think needs refactoring. Or another method,
                    //        that will create a path by only one parameter - photoID
                    PhotoThumbSource =
                        pathUtil.BuildThumbnailsPath(albumModel.UserModelId, photoModel.AlbumModelId)
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