using System;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search.Results
{
    public class AlbumFound : IFound
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }

        public string Name { get; set; }

        public DateTime DateOfCreation { get; set; }

        public ItemType Type
        {
            get { return ItemType.Album; }
        }

        public int Relevance { get; set; }
    }
}