namespace BinaryStudio.PhotoGallery.Domain.Services.Search.FoundItems
{
    public class PhotoFoundItem : IFoundItem
    {
        public int Id { get; set; }

        public int Rating { get; set; }

        /// <summary>
        ///     User that added photo
        /// </summary>
        public int AuthorId { get; set; }

        public int AlbumId { get; set; }

        public string PhotoName { get; set; }

        public ItemType Type
        {
            get { return ItemType.Photo; }
        }

        public int Relevance { get; set; }
    }
}