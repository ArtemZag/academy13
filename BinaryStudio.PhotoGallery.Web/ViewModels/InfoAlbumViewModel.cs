using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class InfoAlbumViewModel
    {
        public AlbumModel AlbumModel { get; set; }
        public List<PhotoViewModel> Photos { get; set; }
    }
}