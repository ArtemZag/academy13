using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class AlbumsViewModel
    {
        public string UserEmail { get; set; }
        public int totalPhotos{ get; set; }
        public ICollection<PhotoViewModel> lastPhotos{ get; set; }
        public ICollection<AlbumViewModel> Models { get; set; }
    }
}