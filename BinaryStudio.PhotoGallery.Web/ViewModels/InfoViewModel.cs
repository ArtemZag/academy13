using System;
using System.Collections.Generic;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class InfoViewModel
    {
        public string UserEmail { get; set; }
        public List<PhotoViewModel> Photos { get; set; }
    }
}