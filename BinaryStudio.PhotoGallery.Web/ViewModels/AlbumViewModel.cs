using System;
using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Web.ViewModels.Photo;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class AlbumViewModel
    {   
        public int Id { get; set; }
        
        public int OwnerId { get; set; }

        public int PhotosCount { get; set; }

        public string CollagePath { get; set; }

        public string AlbumName { get; set; }

        public string Description { get; set; }

        public DateTime DateOfCreation { get; set; }

        public List<PhotoViewModel> Photos { get; set; }
    }
}