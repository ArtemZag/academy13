using BinaryStudio.PhotoGallery.Core.PhotoUtils;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public static class ViewModelExtensions
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

             viewModel.PhotoUrl = viewModel.PathUtil.BuildAvatarPath(model.Id, ImageSize.Original);

             return viewModel;
         }
    }
}