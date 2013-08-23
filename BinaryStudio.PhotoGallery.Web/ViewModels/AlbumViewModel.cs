using System;
using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Database.ModelInterfaces;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class AlbumViewModel
    {
        public int OwnerId { get; set; }

        public int Id { get; set; }

        public string CollageSource { get; set; }

        public string AlbumName { get; set; }

        public string Description { get; set; }

        public DateTime DateOfCreation { get; set; }

        public IEnumerable<AlbumTagModel> Tags { get; set; }

        public List<PhotoViewModel> Photos { get; set; }

        public static AlbumViewModel FromModel(AlbumModel model, IPathUtil pathUtil, IAlbumTagService albumTagService)
        {
            // todo: use pathUtil

            //todo shall fix this. Exception throwns when i try to open oan album page /album/{id}
            //var processor = new AsyncPhotoProcessor(model.OwnerId, model.Id, 64, pathUtil);
            //processor.SyncOriginalAndThumbnailImages();

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
    }
}