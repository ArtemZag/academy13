using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Web.ViewModels.Upload
{
    public class SavePhotosViewModel
    {
        public int AlbumId { get; set; }
        public IEnumerable<string> PhotoNames { get; set; }
    }
}