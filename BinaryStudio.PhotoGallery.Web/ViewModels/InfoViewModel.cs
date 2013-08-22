using System;
using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class InfoViewModel
    {
        public int UserId { get; set; }
        public List<PhotoViewModel> Photos { get; set; }
    }
}