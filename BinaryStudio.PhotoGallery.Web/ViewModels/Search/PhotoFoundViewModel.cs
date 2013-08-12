using System;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;

namespace BinaryStudio.PhotoGallery.Web.ViewModels.Search
{
    public class PhotoFoundViewModel : IFoundViewModel
    {
        public string PhotoName { get; set; }

        public string PhotoViewUrl { get; set; }

        public string AlbumName { get; set; }

        public string AlbumViewUrl { get; set; }

        public string UserName { get; set; }

        public string UserViewUrl { get; set; }

        public int Rating { get; set; }

        public string ThumbnailPath { get; set; }

        public DateTime DateOfCreation { get; set; }

        public string Type
        {
            get { return "photo"; }
        }
    }
}