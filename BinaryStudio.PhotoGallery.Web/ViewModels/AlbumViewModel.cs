using System;
using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Core.PhotoUtils;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class AlbumViewModel
    {
        public int OwnerId { get; set; }

        public int Id { get; set; }

        public string collageSource { get; set; }

        public string AlbumName { get; set; }

        public string Description { get; set; }

        public DateTime DateOfCreation { get; set; }

        public IEnumerable<AlbumTagModel> Tags { get; set; }

        public List<PhotoViewModel> Photos { get; set; }

        public static AlbumViewModel FromModel(AlbumModel model)
        {
            return new AlbumViewModel
            {
                //collageSource = processor.CreateCollageIfNotExist(256,3),
                AlbumName = model.Name,
                Description = model.Description,
                DateOfCreation = model.DateOfCreation,
                OwnerId = model.OwnerId,
                Id = model.Id,
                Photos = new List<PhotoViewModel>(),
            };
        }

        public static AlbumViewModel FromModel(AlbumModel model, IResizePhotoService service)
        {
            var result = FromModel(model);
            result.collageSource = service.GetCollage(model.OwnerId, model.Id, 256, 64, 3);
            return result;
        }
    }
}