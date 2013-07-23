﻿using System.Collections.ObjectModel;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    public static class ModelConverter
    {
        public static UserModel GetModel(RegistrationViewModel viewModel)
        {
            var userModel = new UserModel
                {
                    Email = viewModel.Email,
                    AuthInfos = new Collection<AuthInfoModel> {new AuthInfoModel(-1, viewModel.Password, "local")}
                };

            return userModel;
        }

        public static UserModel GetModel(AuthInfoViewModel viewModel)
        {
            var userModel = new UserModel
                {
                    Email = viewModel.Email,
                    AuthInfos = new Collection<AuthInfoModel> {new AuthInfoModel(-1, viewModel.Password, "local")}
                };

            return userModel;
        }

        public static PhotoViewModel GetViewModel(PhotoModel viewModel)
        {
            var photoModel = new PhotoViewModel {PhotoSource = viewModel.PhotoSource};

            return photoModel;
        }
    }
}