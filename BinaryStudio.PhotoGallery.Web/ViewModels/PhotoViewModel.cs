﻿using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Models;
using Microsoft.Practices.Unity;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class PhotoViewModel
    {
        public string PhotoSource { get; set; }
        public string PhotoThumbSource { get; set; }
        public string PhotoViewPageUrl { get; set; }
        public int PhotoId { get; set; }
        public int AlbumId { get; set; }

        public static PhotoViewModel FromModel(PhotoModel photoModel)
        {
            IUnityContainer container = Bootstrapper.Initialise();

            var pathUtil = container.Resolve<IPathUtil>();

            var urlUtil = container.Resolve<IUrlUtil>();

            string photoSource = pathUtil.BuildOriginalPhotoPath(
                photoModel.OwnerId,
                photoModel.AlbumId,
                photoModel.Id,
                photoModel.Format);

            // todo
            string photoThumbSource = string.Empty;

            var viewModel = new PhotoViewModel
            {
                PhotoSource = photoSource,
                PhotoThumbSource = photoThumbSource,
                AlbumId = photoModel.AlbumId,
                PhotoId = photoModel.Id,
                PhotoViewPageUrl = urlUtil.BuildPhotoViewUrl(photoModel.Id)
            };

            return viewModel;
        }

        public static PhotoModel ToModel(int albumId, int userId, string realFileFormat)
        {
            var model = new PhotoModel
            {
                OwnerId = userId,
                AlbumId = albumId,
                Format = realFileFormat
            };

            return model;
        }
    }
}