using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Web.ViewModels.Photo;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class PublicPhotosViewModel
    {
        public int UserId { get; set; }
        public List<PhotoViewModel> Photos { get; set; }
    }
}