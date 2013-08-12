using System;
using BinaryStudio.PhotoGallery.Domain.Services.Search.Results;

namespace BinaryStudio.PhotoGallery.Web.ViewModels.Search
{
    public class PhotoFoundViewModel : IFoundViewModel
    {
        public string PhotoViewPath { get; set; }

        public string AlbumPath { get; set; }

        public string AuthorPath { get; set; }

        public int Rating { get; set; }

        public string ThumbnailPath { get; set; }

        public DateTime DateOfCreation { get; set; }

        public ItemType Type
        {
            get { return ItemType.Photo; }
        }
    }
}