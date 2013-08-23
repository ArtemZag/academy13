using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
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
                 IsAdmin = model.IsAdmin
             };

             viewModel.PhotoUrl = viewModel.PathUtil.BuildUserAvatarPath(model.Id);

             return viewModel;
         }
    }
}