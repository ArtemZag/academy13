using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels.Albums;
using BinaryStudio.PhotoGallery.Web.ViewModels.User;

namespace BinaryStudio.PhotoGallery.Web.Extensions.ViewModels
{
    public static class UserConvertionExtensions
    {
        public static UserViewModel ToUserViewModel(this UserModel model)
        {
            var viewModel = new UserViewModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                IsAdmin = model.IsAdmin,
                Birthday = model.Birthday
            };

            viewModel.PhotoUrl = viewModel.PathUtil.BuildUserAvatarPath(model.Id);

            return viewModel;
        }

        public static UserInfoViewModel ToUserInfoViewModel(
            this UserModel model,
            int albumCount,
            int photoCount,
            string avatarPath)
        {
            string userFullName = string.Format("{0} {1}", model.FirstName, model.LastName);

            var viewModel = new UserInfoViewModel
            {
                FullName = userFullName,
                Department = model.Department,
                AlbumCount = albumCount,
                PhotoCount = photoCount,
                AvatarPath = avatarPath
            };

            return viewModel;
        }
    }
}