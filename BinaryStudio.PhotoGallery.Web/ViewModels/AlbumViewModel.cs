using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class AlbumViewModel
    {
        public static AlbumViewModel FromModel(AlbumModel mAlbum)
        {
            var viewModel = new AlbumViewModel()
                {
                    AlbumName = mAlbum.Name,
                    Description = mAlbum.Description,
                    Id = mAlbum.Id,
                    Photos = new List<PhotoViewModel>()
                };

            return viewModel;
        }

        public string collageSource { get; set; }
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string AlbumName { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreation { get; set; }
        public int UserModelId { get; set; }
        public virtual ICollection<AlbumTagModel> AlbumTags { get; set; }
        public List<PhotoViewModel> Photos { get; set; }
    }
}