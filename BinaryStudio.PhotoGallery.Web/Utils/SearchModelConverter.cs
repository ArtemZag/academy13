using System.Runtime.Serialization;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Domain.Services.Search;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels.Search;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    internal class SearchModelConverter : ISearchModelConverter
    {
        private readonly IAlbumService albumService;

        private readonly IPathUtil pathUtil;
        private readonly IUrlUtil urlUtil;
        private readonly IUserService userService;

        public SearchModelConverter(IUserService userService, IPathUtil pathUtil, IUrlUtil urlUtil,
            IAlbumService albumService)
        {
            this.userService = userService;
            this.pathUtil = pathUtil;
            this.urlUtil = urlUtil;
            this.albumService = albumService;
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

            UserModel user = userService.GetUser(commentFound.OwnerId);
            string userName = user.FirstName + " " + user.LastName;

            return new CommentFoundViewModel
            {
                CommentUrl = urlUtil.BuildCommentUrl(commentFound.PhotoId, commentFound.Id),
                DateOfCreation = commentFound.DateOfCreation,
                UserName = userName,
                Text = commentFound.Text,
                UserViewUrl = urlUtil.BuildUserViewUrl(commentFound.OwnerId),
                UserAvatarPath = pathUtil.BuildUserAvatarPath(commentFound.OwnerId)
            };
        }

        private IFoundViewModel GetAlbumFoundViewModel(IFound found)
        {
            var albumFound = (AlbumFound) found;

            UserModel user = userService.GetUser(albumFound.OwnerId);
            string userName = user.FirstName + " " + user.LastName;

            return new AlbumFoundViewModel
            {
                Name = albumFound.Name,
                DateOfCreation = albumFound.DateOfCreation,
                UserViewUrl = urlUtil.BuildUserViewUrl(albumFound.OwnerId),
                AlbumViewUrl = urlUtil.BuildAlbumViewUrl(albumFound.Id),
                ThumbnailPath = string.Empty,
                UserName = userName
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
            string albumName = album.Name;

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