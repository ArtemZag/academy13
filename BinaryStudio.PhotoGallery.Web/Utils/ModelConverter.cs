﻿using BinaryStudio.PhotoGallery.Core.PathUtils;
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
        private readonly IAlbumService albumService;

        private readonly IPathUtil pathUtil;
        private readonly IUrlUtil urlUtil;
        private readonly IUserService userService;

        public ModelConverter(IPathUtil pathUtil, IAlbumService albumService, IUserService userService, IUrlUtil urlUtil)
        {
            this.albumService = albumService;
            this.userService = userService;

            this.pathUtil = pathUtil;
            this.urlUtil = urlUtil;
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

        public SearchArguments GetModel(SearchViewModel searchViewModel, int userId)
        {
            return new SearchArguments
            {
                UserId = userId,
                SearchCacheToken = searchViewModel.SearchCacheToken,
                Interval = searchViewModel.Interval,
                SearchQuery = searchViewModel.SearchQuery,
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
            IFoundViewModel result = null;

            switch (found.Type)
            {
                case ItemType.Photo:

                    result = GetPhotoFoundViewModel(found);
                    break;

                case ItemType.User:

                    result = GetUserFoundViewModel(found);
                    break;
            }

            return result;
        }

        public PhotoModel GetPhotoModel(int userId, int albumId, string fullPhotoName)
        {
            var photoModel = new PhotoModel
            {
                OwnerId = userId,
                AlbumId = albumId,
                Name = fullPhotoName
            };

            return photoModel;
        }

        public PhotoViewModel GetViewModel(PhotoModel photoModel)
        {
            // We need to grab photos from album's owner, not from photo's creator.
            // So needs to take an userID via albumModel.UserModelID
            AlbumModel albumModel = albumService.GetAlbum(photoModel.AlbumId);

            var viewModel = new PhotoViewModel
            {
                // todo: UserId in photoModel != userId which album contain this photo
                // is PhotoSource necessary in PhotoViewModel?!

                // Maaak: UserModelID is an userID, who has created(added) this photo. 
                //        So it can be as equel to userId, which album contain this photo, so not.
                //        Look at PhotoModel to check the meaning of property UserModelID
                PhotoSource =
                    pathUtil.BuildOriginalPhotoPath(albumModel.OwnerId, photoModel.AlbumId,
                        photoModel.Id, photoModel.Format),

                // Maaak: I think needs refactoring. Or another method,
                //        that will create a path by only one parameter - photoID
                // Anton: we need know userId, albumId, photoId, format (jpg, png), size for getting thumbnail

                PhotoThumbSource =
                    pathUtil.BuildThumbnailPath(photoModel.OwnerId, photoModel.AlbumId, photoModel.Id, photoModel.Format),
                AlbumId = photoModel.AlbumId,
                PhotoId = photoModel.Id,
                PhotoViewPageUrl = urlUtil.BuildPhotoViewUrl(photoModel.Id)
            };

            return viewModel;
        }

        public UserViewModel GetViewModel(UserModel userModel)
        {
            return new UserViewModel
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName
            };
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

        private IFoundViewModel GetUserFoundViewModel(IFound found)
        {
            var userFound = (UserFound) found;

            return new UserFoundViewModel
            {
                AvatarPath = pathUtil.BuildUserAvatarPath(userFound.Id),
                Department = userFound.Department,
                IsOnline = userFound.IsOnline,
                Name = userFound.Name,
                UserViewUri = urlUtil.BuildUserViewUrl(userFound.Id)
            };
        }

        private IFoundViewModel GetPhotoFoundViewModel(IFound found)
        {
            var photoModel = (PhotoFound) found;

            AlbumModel album = albumService.GetAlbum(photoModel.AlbumId);
            string albumName = album.AlbumName;

            UserModel user = userService.GetUser(photoModel.OwnerId);
            string userName = user.FirstName + " " + user.LastName;

            string thumbnailPath = pathUtil.BuildThumbnailPath(photoModel.OwnerId, photoModel.AlbumId, photoModel.Id,
                photoModel.Format);

            return new PhotoFoundViewModel
            {
                ThumbnailPath = thumbnailPath,
                PhotoViewUrl = urlUtil.BuildPhotoViewUrl(photoModel.Id),
                AlbumName = albumName,
                AlbumViewUrl = urlUtil.BuildAlbumViewUrl(photoModel.AlbumId),
                UserName = userName,
                UserViewUrl = urlUtil.BuildUserViewUrl(photoModel.OwnerId),
                DateOfCreation = photoModel.DateOfCreation,
                Rating = photoModel.Rating
            };
        }
    }
}