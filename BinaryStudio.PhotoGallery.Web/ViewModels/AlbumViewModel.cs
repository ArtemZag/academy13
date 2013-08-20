using System;
using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class AlbumViewModel
    {
        public string CollageSource { get; set; }
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string AlbumName { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreation { get; set; }
        public int UserModelId { get; set; }
        public virtual ICollection<AlbumTagModel> AlbumTags { get; set; }
        public List<PhotoViewModel> Photos { get; set; }

        public static AlbumViewModel FromModel(AlbumModel mAlbum)
        {
            var viewModel = new AlbumViewModel
            {
                AlbumName = mAlbum.Name,
                AlbumTags = mAlbum.AlbumTags,
                Description = mAlbum.Description,
                Id = mAlbum.Id,
                Photos = new List<PhotoViewModel>()
            };

            return viewModel;
        }
    }
}