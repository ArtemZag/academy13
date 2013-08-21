using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class AlbumsViewModel
    {
        public string UserEmail { get; set; }
        public int TotalPhotos { get; set; }
        public ICollection<PhotoViewModel> LastPhotos { get; set; }
        public ICollection<AlbumViewModel> Models { get; set; }
    }
}