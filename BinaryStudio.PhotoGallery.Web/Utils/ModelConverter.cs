using System;
using System.IO;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Domain.Services.Search;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.Authorization;
using BinaryStudio.PhotoGallery.Web.ViewModels.PhotoPage;
using BinaryStudio.PhotoGallery.Web.ViewModels.Search;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    internal class ModelConverter : IModelConverter
    {
        private readonly IAlbumService _albumService;
        private readonly IPathUtil _pathUtil;

        public ModelConverter(IPathUtil pathUtil, IAlbumService albumService)
        {
            _pathUtil = pathUtil;
            _albumService = albumService;
        }

        public UserModel GetModel(SignupViewModel registrationViewModel)
        {
            var userModel = new UserModel
                {
                    Email = registrationViewModel.Email,
                    UserPassword = registrationViewModel.Password
                };

            return userModel;
        }

        public UserModel GetModel(SigninViewModel authorizationViewModel)
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
                    CacheToken = searchViewModel.CacheToken,
                    Begin = searchViewModel.Begin,
                    End = searchViewModel.End,
                    SearchQuery = searchViewModel.SearchQuery,
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

        public IFoundViewModel GetViewModel(IFound found)
        {
            throw new NotImplementedException();
        }

        public PhotoModel GetPhotoModel(int userId, int albumId, string fullPhotoName)
        {
            var photoModel = new PhotoModel
                {
                    UserId = userId,
                    AlbumId = albumId,
                    PhotoName = Path.GetFileNameWithoutExtension(fullPhotoName),
                    Format = Path.GetExtension(fullPhotoName).Remove(0, 1)
                };

            return photoModel;
        }

        public PhotoViewModel GetViewModel(PhotoModel photoModel)
        {
            // We need to grab photos from album's owner, not from photo's creator.
            // So needs to take an userID via albumModel.UserModelID
            AlbumModel albumModel = _albumService.GetAlbum(photoModel.AlbumId);

            var viewModel = new PhotoViewModel
                {
                    // todo: UserId in photoModel != userId which album contain this photo
                    // is PhotoSource necessary in PhotoViewModel?!

                    // Maaak: UserModelID is an userID, who has created(added) this photo. 
                    //        So it can be as equel to userId, which album contain this photo, so not.
                    //        Look at PhotoModel to check the meaning of property UserModelID
                    PhotoSource =
                        _pathUtil.BuildOriginalPhotoPath(albumModel.UserId, photoModel.AlbumId,
                                                         photoModel.PhotoName, photoModel.Format),

                    // Maaak: I think needs refactoring. Or another method,
                    //        that will create a path by only one parameter - photoID
                    PhotoThumbSource =
                        _pathUtil.BuildThumbnailsPath(albumModel.UserId, photoModel.AlbumId)
                        + @"\" + photoModel.PhotoName + photoModel.Format,
                    AlbumId = photoModel.AlbumId,
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