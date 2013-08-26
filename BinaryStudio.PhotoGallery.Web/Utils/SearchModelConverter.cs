using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Core.PhotoUtils;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Domain.Services.Search;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels.Search;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    internal class SearchModelConverter : ISearchModelConverter
    {
        private readonly IAlbumService _albumService;

        private readonly IPathUtil _pathUtil;
        private readonly IUrlUtil _urlUtil;
        private readonly IUserService _userService;

        public SearchModelConverter(IUserService userService, IPathUtil pathUtil, IUrlUtil urlUtil,
            IAlbumService albumService)
        {
            _userService = userService;
            _pathUtil = pathUtil;
            _urlUtil = urlUtil;
            _albumService = albumService;
        }

        public SearchArguments GetModel(SearchViewModel searchViewModel, int userId)
        {
            return new SearchArguments
            {
                UserId = userId,
                SearchCacheToken = searchViewModel.SearchCacheToken,
                Skip = searchViewModel.Skip,
                Take = searchViewModel.Take,
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

                case ItemType.Album:

                    result = GetAlbumFoundViewModel(found);
                    break;

                case ItemType.Comment:

                    result = GetCommentFoundViewModel(found);
                    break;
            }

            return result;
        }

        private IFoundViewModel GetCommentFoundViewModel(IFound found)
        {
            var commentFound = (CommentFound) found;

            UserModel user = _userService.GetUser(commentFound.OwnerId);
            string userName = user.FirstName + " " + user.LastName;

            return new CommentFoundViewModel
            {
                CommentUrl = _urlUtil.BuildCommentUrl(commentFound.PhotoId, commentFound.Id),
                DateOfCreation = commentFound.DateOfCreation,
                UserName = userName,
                Text = commentFound.Text,
                UserViewUrl = _urlUtil.BuildUserViewUrl(commentFound.OwnerId),
                UserAvatarPath = _pathUtil.BuildAvatarPath(commentFound.OwnerId, ImageSize.Medium)
            };
        }

        private IFoundViewModel GetAlbumFoundViewModel(IFound found)
        {
            var albumFound = (AlbumFound) found;

            UserModel user = _userService.GetUser(albumFound.OwnerId);
            string userName = user.FirstName + " " + user.LastName;

            return new AlbumFoundViewModel
            {
                Name = albumFound.Name,
                DateOfCreation = albumFound.DateOfCreation,
                UserViewUrl = _urlUtil.BuildUserViewUrl(albumFound.OwnerId),
                AlbumViewUrl = _urlUtil.BuildAlbumViewUrl(albumFound.Id),
                ThumbnailPath = string.Empty,
                UserName = userName
            };
        }

        private IFoundViewModel GetUserFoundViewModel(IFound found)
        {
            var userFound = (UserFound) found;

            return new UserFoundViewModel
            {
                AvatarPath = _pathUtil.BuildAvatarPath(userFound.Id, ImageSize.Medium),
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
                photoModel.Format, ImageSize.Medium);

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