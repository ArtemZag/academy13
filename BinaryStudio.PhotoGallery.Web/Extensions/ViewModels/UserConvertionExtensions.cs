using BinaryStudio.PhotoGallery.Core.PhotoUtils;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Extensions.ViewModels
{
    public static class UserConvertionExtensions
    {
        public static UserViewModel ToUserViewModel(this UserModel model)
        {
            var viewModel = new UserViewModel
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                IsAdmin = model.IsAdmin,
                Birthday = model.Birthday
            };

            viewModel.AlbumsCount = viewModel.AlbumService.AlbumsCount(model.Id);
            viewModel.PhotoCount = viewModel.PhotoService.PhotoCount(model.Id);
            viewModel.PhotoUrl = viewModel.PathUtil.BuildAvatarPath(model.Id, ImageSize.Medium);

            return viewModel;
        }
    }
}