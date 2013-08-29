using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.Photo;

namespace BinaryStudio.PhotoGallery.Web.Extensions.ViewModels
{
    public static class AlbumConvertionExtensions
    {
        public static AlbumViewModel ToAlbumViewModel(this AlbumModel model, string collageSource)
        {
            AlbumViewModel viewModel = model.ToAlbumViewModel();

            viewModel.CollagePath = collageSource;

            return viewModel;
        }

        public static AlbumViewModel ToAlbumViewModel(this AlbumModel model)
        {
            var viewModel = new AlbumViewModel
            {
                AlbumName = model.Name,
                Description = model.Description,
                DateOfCreation = model.DateOfCreation,
                OwnerId = model.OwnerId,
                Id = model.Id,
                Photos = new List<PhotoViewModel>(),
            };

            return viewModel;
        }
    }
}