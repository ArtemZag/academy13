using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Domain.Services.Search;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.PhotoPage;
using BinaryStudio.PhotoGallery.Web.ViewModels.Search;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    internal class ModelConverter : IModelConverter
    {
        private readonly IAlbumService _albumService;

        private readonly IPathUtil _pathUtil;
        private readonly IUrlUtil _urlUtil;
        private readonly IUserService _userService;

        public ModelConverter(IPathUtil pathUtil, IAlbumService albumService, IUserService userService, IUrlUtil urlUtil)
        {
            _albumService = albumService;
            _userService = userService;
            _pathUtil = pathUtil;
            _urlUtil = urlUtil;
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

        public PhotoModel GetPhotoModel(int userId, int albumId, string realFileFormat)
        {
            var photoModel = new PhotoModel
            {
                OwnerId = userId,
                AlbumId = albumId,
                Format = realFileFormat
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
                    _pathUtil.BuildOriginalPhotoPath(albumModel.OwnerId, photoModel.AlbumId,
                        photoModel.Id, photoModel.Format),

                // Maaak: I think needs refactoring. Or another method,
                //        that will create a path by only one parameter - photoID
                // Anton: we need know userId, albumId, photoId, format (jpg, png), size for getting thumbnail

                PhotoThumbSource =
                    _pathUtil.BuildThumbnailPath(photoModel.OwnerId, photoModel.AlbumId, photoModel.Id,
                        photoModel.Format),
                AlbumId = photoModel.AlbumId,
                PhotoId = photoModel.Id,
                PhotoViewPageUrl = _urlUtil.BuildPhotoViewUrl(photoModel.Id)
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
                AvatarPath = _pathUtil.BuildUserAvatarPath(userFound.Id),
                Department = userFound.Department,
                IsOnline = userFound.IsOnline,
                Name = userFound.Name,
                UserViewUri = _urlUtil.BuildUserViewUrl(userFound.Id)
            };
        }

        private IFoundViewModel GetPhotoFoundViewModel(IFound found)
        {
            var photoModel = (PhotoFound) found;

            AlbumModel album = _albumService.GetAlbum(photoModel.AlbumId);
            string albumName = album.Name;

            UserModel user = _userService.GetUser(photoModel.OwnerId);
            string userName = user.FirstName + " " + user.LastName;

            string thumbnailPath = _pathUtil.BuildThumbnailPath(photoModel.OwnerId, photoModel.AlbumId, photoModel.Id,
                photoModel.Format);

            return new PhotoFoundViewModel
            {
                ThumbnailPath = thumbnailPath,
                PhotoViewUrl = _urlUtil.BuildPhotoViewUrl(photoModel.Id),
                AlbumName = albumName,
                AlbumViewUrl = _urlUtil.BuildAlbumViewUrl(photoModel.AlbumId),
                UserName = userName,
                UserViewUrl = _urlUtil.BuildUserViewUrl(photoModel.OwnerId),
                DateOfCreation = photoModel.DateOfCreation,
                Rating = photoModel.Rating
            };
        }
    }
}