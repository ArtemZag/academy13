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
        private readonly IAlbumService albumService;
        private readonly IUserService userService;

        private readonly IPathUtil pathUtil;
        private readonly IUrlUtil urlUtil;

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

        private IFoundViewModel GetUserFoundViewModel(IFound found)
        {
            var userFound = (UserFound) found;

            return new UserFoundViewModel
            {
                AvatarPath = pathUtil.BuildUserAvatarPath(userFound.Id),
                Department = userFound.Department,
                IsOnline = userFound.IsOnline,
                Name = userFound.Name
            };
        }

        private IFoundViewModel GetPhotoFoundViewModel(IFound found)
        {
            var photoFound = (PhotoFound) found;

            var album = albumService.GetAlbum(photoFound.AlbumId);
            string albumName = album.AlbumName;

            var user = userService.GetUser(photoFound.UserId);
            string userName = user.FirstName + " " + user.LastName;

            return new PhotoFoundViewModel
            {
                ThumbnailPath = string.Empty, // todo

                PhotoName = photoFound.PhotoName,
                PhotoViewUrl = urlUtil.BuildPhotoViewUrl(photoFound.Id),

                AlbumName = albumName,
                AlbumViewUrl = urlUtil.BuildAlbumViewUrl(photoFound.AlbumId),

                UserName = userName,
                UserViewUrl = urlUtil.BuildUserViewUrl(photoFound.UserId),

                DateOfCreation = photoFound.DateOfCreation,
                Rating = photoFound.Rating
            };
        }

        public PhotoModel GetPhotoModel(int userId, int albumId, string fullPhotoName)
        {
            var photoModel = new PhotoModel
                {
                    UserId = userId,
                    AlbumId = albumId,
                    PhotoFileName = fullPhotoName
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
                        pathUtil.BuildOriginalPhotoPath(albumModel.UserId, photoModel.AlbumId,
                                                         photoModel.PhotoFileName, photoModel.Format),

                    // Maaak: I think needs refactoring. Or another method,
                    //        that will create a path by only one parameter - photoID
                    PhotoThumbSource =
                        string.Format("{0}\\{1}{2}",
                        pathUtil.BuildThumbnailsPath(albumModel.UserId, photoModel.AlbumId),
                        photoModel.PhotoFileName,
                        photoModel.Format),

                    AlbumId = photoModel.AlbumId,
                    PhotoId = photoModel.Id
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
    }
}