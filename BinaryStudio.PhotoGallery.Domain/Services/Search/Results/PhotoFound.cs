using System;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search.Results
{
    public class PhotoFound : IFound
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }

        public int AlbumId { get; set; }

        public string Format { get; set; }

        public int Rating { get; set; }

        public DateTime DateOfCreation { get; set; }

        public ItemType Type
        {
            get { return ItemType.Photo; }
        }

        public int Relevance { get; set; }
    }
}