using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Web.ViewModels.Upload
{
    public class MovePhotosViewModel
    {
        public string AlbumName { get; set; }
        public IEnumerable<int> PhotosId { get; set; }
    }
}