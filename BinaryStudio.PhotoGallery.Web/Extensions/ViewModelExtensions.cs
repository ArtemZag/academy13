using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Extensions
{
    public static class ViewModelExtensions
    {
         public static UserViewModel ToUserViewModel(this UserModel model)
         {
             var viewModel =  new UserViewModel
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
    }
}