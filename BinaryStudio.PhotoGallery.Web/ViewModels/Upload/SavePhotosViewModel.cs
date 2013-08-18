using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Web.ViewModels.Upload
{
    public class SavePhotosViewModel
    {
        public string AlbumName { get; set; }
        public IEnumerable<int> PhotosId { get; set; }
    }
}