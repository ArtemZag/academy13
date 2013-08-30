using BinaryStudio.PhotoGallery.Core.PhotoUtils;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels.User;

namespace BinaryStudio.PhotoGallery.Web.Extensions.ViewModels
{
    public static class UserConvertionExtensions
    {
        public static UserViewModel ToUserViewModel(this UserModel model, bool isBlocked)
        {
            var viewModel = model.ToUserViewModel();
            viewModel.IsBlocked = isBlocked;
            return viewModel;
        }

        public static UserViewModel ToUserViewModel(this UserModel model)
        {
            var viewModel = new UserViewModel
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Department = model.Department,
                Birthday = model.Birthday,
                IsActivated = model.IsActivated
            };

            viewModel.AvatarUrl = viewModel.PathUtil.BuildAvatarPath(model.Id, ImageSize.Medium);
            viewModel.ProfileUrl = viewModel.UrlUtil.BuildUserViewUrl(model.Id);

            return viewModel;
        }

        public static UserViewModel ToNoneUserViewModel(this UserModel model)
        {
            var viewModel = new UserViewModel
            {
                FirstName = "None",
                LastName = "",
                AlbumsCount = 0,
                PhotoCount = 0
            };
            viewModel.AvatarUrl = viewModel.PathUtil.BuildAvatarPath(model.Id, ImageSize.Medium);
            return viewModel;
        }

        public static UserViewModel ToUserViewModel(this UserModel model, int photoCount, int albumCount)
        {
            var viewModel = model.ToUserViewModel();
            viewModel.AlbumsCount = albumCount;
            viewModel.PhotoCount = photoCount;
            return viewModel;
        }
    }
}