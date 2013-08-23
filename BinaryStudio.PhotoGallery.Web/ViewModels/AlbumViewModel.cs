using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Core.PhotoUtils;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using Microsoft.Practices.Unity;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class AlbumViewModel
    {
        public int ownerId { get; set; }
        public int id { get; set; }
        public string collageSource { get; set; }
        public string albumName { get; set; }
        public string description { get; set; }
        public DateTime dateOfCreation { get; set; }
        public IEnumerable<AlbumTagModel> albumTags { get; set; }
        public IList<PhotoViewModel> Photos { get; set; }

        public static AlbumViewModel FromModel(AlbumModel model)
        {
            IUnityContainer container = Bootstrapper.Initialise();
            var pathUtil = container.Resolve<IPathUtil>();
            //todo shall fix this. Exception throwns when i try to open oan album page /album/{id}
            //var processor = new AsyncPhotoProcessor(model.OwnerId, model.Id, 64, pathUtil);
            //processor.SyncOriginalAndThumbnailImages();
            return new AlbumViewModel()
                {
                    //collageSource = processor.CreateCollageIfNotExist(256,3),
                    albumName = model.Name,
                    description = model.Description,
                    dateOfCreation = model.DateOfCreation,
                    ownerId = model.OwnerId,
                    id = model.Id,
                    Photos = new List<PhotoViewModel>()
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