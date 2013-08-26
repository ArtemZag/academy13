using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Web.ViewModels.Photo
{
    public class PhotoPageViewModel
    {
        public PhotoViewModel Photo { get; set; }
        public IEnumerable<PhotoCommentViewModel> PhotoComment { get; set; }
    }
}