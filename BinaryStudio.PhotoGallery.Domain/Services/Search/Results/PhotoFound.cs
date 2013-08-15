using System;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search.Results
{
    public class PhotoFound : IFound
    {
        public int Id { get; set; }

        /// <summary>
        ///     User that added photo
        /// </summary>
        public int UserId { get; set; }

        public int AlbumId { get; set; }

        public string PhotoName { get; set; }

        // todo: delete
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